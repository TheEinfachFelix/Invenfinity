using LabelMaker.Models.Label;
using LabelMakerWPF.Models.Label.Elements;
using QRCoder;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using System.Windows;
using System.Windows.Media;

namespace LabelMakerWPF.Services
{
    internal static class LayoutHelper
    {
        public static DrawingGroup CreateDrawGroup(
     DrawingGroup svgSource,
     ILabelElement element,
     double targetLengthUnits,
     double targetHeightUnits)
        {
            var outputGroup = new DrawingGroup();
            if (svgSource == null) return outputGroup;

            Rect bounds = svgSource.Bounds;
            if (bounds.Width == 0 || bounds.Height == 0) return outputGroup;

            double angle = 0;
            switch (element.Orientation)
            {
                case OrientationCases.Vertical:
                    angle = 90;
                    break;
                case OrientationCases.Wide:
                    // Wenn es höher als breit ist, rotiere es ins Querformat
                    if (bounds.Width < bounds.Height) angle = 90;
                    break;
                case OrientationCases.Narrow:
                    // Wenn es breiter als hoch ist, rotiere es ins Hochformat
                    if (bounds.Width > bounds.Height) angle = 90;
                    break;
                case OrientationCases.Horizontal:
                default:
                    angle = 0;
                    break;
            }

            bool isRotated = Math.Abs(angle % 180) > 0.1;
            double currentWidth = isRotated ? bounds.Height : bounds.Width;
            double currentHeight = isRotated ? bounds.Width : bounds.Height;

            // Contain Scaling basierend auf den neuen Dimensionen
            double relScale = targetHeightUnits / currentHeight;
            double scale = Math.Min(targetLengthUnits / currentWidth, targetHeightUnits / currentHeight);

            double effectiveScale = Math.Round(scale / relScale, 3);
            if (effectiveScale > element.MaxScale || effectiveScale < element.MinScale)
                throw new ArgumentOutOfRangeException(nameof(scale));

            double scaledWidth = currentWidth * scale;
            double scaledHeight = currentHeight * scale;

            // Offsets basierend auf den rotierten, skalierten Maßen
            double x = element.getXOffest(targetLengthUnits, scaledWidth);
            double y = element.getYOffest(targetHeightUnits, scaledHeight);

            // 3. Transformationen anwenden
            var transformGroup = new TransformGroup();

            // Schritt A: Ursprung auf (0,0) korrigieren
            transformGroup.Children.Add(new TranslateTransform(-bounds.X, -bounds.Y));

            // Schritt B: Rotation
            if (isRotated)
            {
                transformGroup.Children.Add(new RotateTransform(angle));
                // Nach 90° Drehung liegt der Inhalt im negativen X-Bereich (WPF Logik)
                // Wir schieben ihn zurück in den positiven Bereich
                transformGroup.Children.Add(new TranslateTransform(bounds.Height, 0));
            }

            // Schritt C: Skalierung und finale Positionierung
            transformGroup.Children.Add(new ScaleTransform(scale, scale));
            transformGroup.Children.Add(new TranslateTransform(x, y));

            var contentGroup = new DrawingGroup { Transform = transformGroup };
            contentGroup.Children.Add(svgSource);

            // Hintergrund/Bounding Box
            var background = new GeometryDrawing
            {
                Geometry = new RectangleGeometry(new Rect(0, 0, targetLengthUnits + element.PaddingUnits, targetHeightUnits)),
                Brush = Brushes.Transparent,
                Pen = null
            };

            outputGroup.Children.Add(background);
            outputGroup.Children.Add(contentGroup);

            return outputGroup;
        }
        public static DrawingGroup CreateDrawGroup(
             Drawing svgSource,
             ILabelElement element,
             double targetLengthUnits,
             double targetHeightUnits)
        {
            DrawingGroup group = new DrawingGroup();
            group.Children.Add(svgSource);
            return CreateDrawGroup(group,element,targetLengthUnits,targetHeightUnits);
        }
        public static DrawingGroup concadGroups(List<DrawingGroup> groups)
        {
            if (groups == null || groups.Count == 0)
                throw new ArgumentException("groups darf nicht leer sein", nameof(groups));

            var result = new DrawingGroup();

            // Referenzhöhe festlegen (erste Gruppe)
            double referenceHeight = groups[0].Bounds.Height;

            double currentX = 0;

            foreach (var group in groups)
            {
                if (group == null)
                    continue;

                var bounds = group.Bounds;

                // Höhencheck
                if (Math.Abs(bounds.Height - referenceHeight) > 0.001)
                    throw new ArgumentException("Alle DrawingGroups müssen die gleiche Höhe haben");

                // Neue Gruppe erzeugen, um Original nicht zu verändern
                var wrapper = new DrawingGroup();

                // Verschiebung setzen
                wrapper.Transform = new TranslateTransform(currentX - bounds.X, -bounds.Y);

                // Original hinzufügen
                wrapper.Children.Add(group);

                // Zum Ergebnis hinzufügen
                result.Children.Add(wrapper);

                // X-Offset erhöhen (Breite der aktuellen Gruppe)
                currentX += bounds.Width;
            }

            return result;
        }
        public static Drawing GenerateQrCode(string value)
        {
            var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(value, QRCodeGenerator.ECCLevel.Q);
            var svgQr = new SvgQRCode(data);
            string svg = svgQr.GetGraphic(1, "#000000", "#FFFFFF", false);

            var reader = new FileSvgReader(new WpfDrawingSettings());

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(svg)))
            {
                return reader.Read(stream);
            }
        }

    public static DrawingGroup TrimWhitespace(DrawingGroup input)
    {
        if (input == null) return null;

        // 1. Alle transparenten Hintergrund-Rechtecke rekursiv entfernen
        var cleanedDrawing = RemoveTransparentBackgrounds(input) as DrawingGroup;

        // Wenn nach dem Bereinigen nichts mehr übrig ist, leere Gruppe zurückgeben
        if (cleanedDrawing == null) return new DrawingGroup();

        // 2. Jetzt die ECHTEN Bounds des rein sichtbaren Inhalts berechnen
        Rect bounds = cleanedDrawing.Bounds;

        if (bounds.IsEmpty) return cleanedDrawing;

        // 3. Den sichtbaren Inhalt exakt auf (0,0) verschieben
        var transformGroup = new TransformGroup();
        if (cleanedDrawing.Transform != null)
        {
            transformGroup.Children.Add(cleanedDrawing.Transform);
        }

        // Verschieben um die tatsächliche obere linke Kante des sichtbaren Inhalts
        transformGroup.Children.Add(new TranslateTransform(-bounds.X, -bounds.Y));

        cleanedDrawing.Transform = transformGroup;

        return cleanedDrawing;
    }

    // Rekursive Hilfsmethode, die den Baum durchläuft und unsichtbare Elemente aussortiert
    private static Drawing RemoveTransparentBackgrounds(Drawing drawing)
    {
        if (drawing == null) return null;

        if (drawing is DrawingGroup group)
        {
            // Klonen, um die Eigenschaften (Transform, Opacity etc.) zu behalten
            var clonedGroup = group.Clone();
            clonedGroup.Children.Clear(); // Children leeren und nur die sichtbaren neu hinzufügen

            foreach (var child in group.Children)
            {
                var cleanedChild = RemoveTransparentBackgrounds(child);
                if (cleanedChild != null)
                {
                    clonedGroup.Children.Add(cleanedChild);
                }
            }

            // Wenn die Gruppe leer ist (z.B. weil sie nur transparente Elemente enthielt), ignoriere sie
            return clonedGroup.Children.Count > 0 ? clonedGroup : null;
        }

        if (drawing is GeometryDrawing geomDrawing)
        {
            // Prüfen, ob das Element sichtbar ist
            bool hasBrush = geomDrawing.Brush != null && geomDrawing.Brush != Brushes.Transparent;
            bool hasPen = geomDrawing.Pen != null && geomDrawing.Pen.Thickness > 0 && geomDrawing.Pen.Brush != Brushes.Transparent;

            // Dein Hintergrund hat Brush = Transparent und Pen = null. Er fällt hier durch.
            if (!hasBrush && !hasPen)
            {
                return null; // Unsichtbares Element verwerfen
            }
        }

        // Für alle anderen (sichtbaren GeometryDrawings, ImageDrawings, etc.) das Original klonen
        return drawing.Clone();
    }

    /// <summary>
    /// Berechnet die aufgeteilten Längen für die Elemente basierend auf der Ziel-Länge, Standard-Längen und Min/Max-Skalierungen.
    /// </summary>
    public static double[] CalculateDistributedLengths(double targetTotalLengthUnits, double[] standardLengthsUnits, double[] minScales, double[] maxScales)
        {
            int n = standardLengthsUnits.Length;
            double[] finalLengths = new double[n];
            bool[] locked = new bool[n];
            double[] scales = new double[n];

            double totalMinLength = 0;
            double totalMaxLength = 0;
            double remainingStandardLength = 0;

            // Vorab prüfen, ob die Ziellänge überhaupt theoretisch erreichbar ist
            for (int i = 0; i < n; i++)
            {
                totalMinLength += standardLengthsUnits[i] * minScales[i];
                totalMaxLength += standardLengthsUnits[i] * maxScales[i];
                remainingStandardLength += standardLengthsUnits[i];
            }

            // Toleranz für Floating-Point Ungenauigkeiten
            const double epsilon = 0.0001;

            if (targetTotalLengthUnits < totalMinLength - epsilon)
                throw new InvalidOperationException($"Skalierungsfehler: Die Ziellänge ({targetTotalLengthUnits}) ist kleiner als die minimal mögliche Länge ({totalMinLength}).");

            //if (targetTotalLengthUnits > totalMaxLength + epsilon)
            //    throw new InvalidOperationException($"Skalierungsfehler: Die Ziellänge ({targetTotalLengthUnits}) ist größer als die maximal mögliche Länge ({totalMaxLength}).");

            double remainingTargetLength = targetTotalLengthUnits;
            bool changed = true;

            // Iterative Verteilung des Platzes
            while (changed)
            {
                changed = false;
                // Aktuelles Skalierungsverhältnis für alle noch nicht gesperrten Elemente
                double currentRatio = remainingStandardLength > 0.0001 ? remainingTargetLength / remainingStandardLength : 1.0;
                for (int i = 0; i < n; i++)
                {
                    if (!locked[i])
                    {
                        double proposedScale = currentRatio;

                        // Prüfen ob Grenzen verletzt werden
                        if (proposedScale < minScales[i])
                        {
                            scales[i] = minScales[i];
                            locked[i] = true;
                            changed = true; // Neu berechnen, da Restplatz sich ändert
                        }
                        else if (proposedScale > maxScales[i])
                        {
                            scales[i] = maxScales[i];
                            locked[i] = true;
                            changed = true; // Neu berechnen, da Restplatz sich ändert
                        }

                        if (locked[i])
                        {
                            // Element ist an eine Grenze gestoßen. Seine Länge wird vom Rest abgezogen.
                            remainingTargetLength -= standardLengthsUnits[i] * scales[i];
                            remainingStandardLength -= standardLengthsUnits[i];
                        }
                        else
                        {
                            // Element liegt im gültigen Bereich (vorerst)
                            scales[i] = proposedScale;
                        }
                    }
                }
            }

            // Finale Längen berechnen
            for (int i = 0; i < n; i++)
            {
                finalLengths[i] = standardLengthsUnits[i] * scales[i];
            }

            return finalLengths;
        }

        public static DrawingGroup RenderAndScaleList(List<ILabelElement> elements, double labelHeightUnits, double labelLengthUnits)
        {
            int count = elements.Count;
            if (count == 0) return new DrawingGroup();

            double[] standardContentLengths = new double[count];
            double[] minScales = new double[count];
            double[] maxScales = new double[count];

            double totalPadding = 0;

            // 1. Content-Längen und Padding separieren
            for (int i = 0; i < count; i++)
            {
                DrawingGroup standardDrawing = elements[i].RenderStandardSize(labelHeightUnits);

                // WICHTIG: Padding von der ermittelten Breite abziehen, da wir nur den Content skalieren.
                // CreateDrawGroup erwartet den reinen Content-Wert für targetLengthUnits.
                double contentWidth = standardDrawing.Bounds.Width - elements[i].PaddingUnits;
                standardContentLengths[i] = Math.Max(0, contentWidth);

                minScales[i] = elements[i].MinScale;
                maxScales[i] = elements[i].MaxScale;
                totalPadding += elements[i].PaddingUnits;
            }

            // 2. Verfügbaren Platz berechnen (Padding wird vom Gesamtplatz abgezogen)
            double availableContentLength = labelLengthUnits - totalPadding;

            // 3. Verteilung berechnen (Wirft automatisch Exception, wenn nicht möglich)
            double[] calculatedContentLengths = LayoutHelper.CalculateDistributedLengths(availableContentLength, standardContentLengths, minScales, maxScales);

            // 4. Elemente mit ihren final berechneten Längen rendern
            var renderedGroups = new List<DrawingGroup>();
            for (int i = 0; i < count; i++)
            {
                // calculatedContentLengths[i] ist jetzt die Ziellänge OHNE Padding.
                renderedGroups.Add(elements[i].Render(labelHeightUnits, calculatedContentLengths[i]));
            }

            // 5. Die fertig skalierten DrawingGroups wieder zusammensetzen
            return LayoutHelper.concadGroups(renderedGroups);
        }


    }
}
