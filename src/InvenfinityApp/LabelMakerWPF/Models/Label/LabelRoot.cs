using LabelMaker.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace LabelMaker.Models.Label
{
    internal class LabelRoot
    {
        public double LabelLength { get; set; }
        public List<ILabelElement> Elements { get; set; } = new List<ILabelElement>();
        public List<ILabelElement> VisibleElements { get => Elements
                .Where(e => e.MinWidthMm == null || e.MinWidthMm <= LabelLength)
                .ToList();}
        public LabelRoot(double labelLength) 
        {
            LabelLength = labelLength;
        }

        public DrawingGroup BuildVector(double labelHeight)
        {
            var group = new DrawingGroup();
            var elements = VisibleElements;

            if (!elements.Any()) return group;

            // 1. Berechne die individuellen Skalierungswerte für die feste Länge
            var scales = CalcScale(labelHeight);

            double xOffset = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                double currentScale = scales[i];

                // Rendern mit der berechneten Skalierung
                element.Render(group, xOffset, labelHeight, currentScale);

                // Offset erhöhen
                xOffset += element.GetWidth(labelHeight, currentScale);

                if (element.Padding.HasValue)
                    xOffset += Converter.mmtoUnits(element.Padding.Value);
            }

            return group;
        }
        public List<double> CalcScale(double labelHeight)
        {
            var elements = VisibleElements;
            double targetUnits = Converter.mmtoUnits(LabelLength);

            // Initialbereiche für die Suche nach dem optimalen 't'
            double tMin = 0.0;
            double tMax = 1.0;
            double tBest = 0.0;

            // Wir führen eine binäre Suche (Bisektion) durch, um das t zu finden,
            // das der Zielbreite am nächsten kommt. 20 Iterationen sind extrem präzise.
            for (int i = 0; i < 20; i++)
            {
                double tMid = (tMin + tMax) / 2;
                double currentWidth = CalculateTotalWidth(elements, labelHeight, tMid);

                if (currentWidth < targetUnits)
                {
                    tBest = tMid;
                    tMin = tMid; // Wir müssen größer werden
                }
                else
                {
                    tBest = tMid;
                    tMax = tMid; // Wir müssen kleiner werden
                }
            }

            // Finale Skalierungen basierend auf dem gefundenen tBest erstellen
            return elements.Select(e => e.MinScale + tBest * (e.MaxScale - e.MinScale)).ToList();
        }

        // Hilfsmethode zur Berechnung der Gesamtbreite für ein gegebenes t
        private double CalculateTotalWidth(List<ILabelElement> elements, double labelHeight, double t)
        {
            double total = 0;
            foreach (var e in elements)
            {
                double scale = e.MinScale + t * (e.MaxScale - e.MinScale);
                total += e.GetWidth(labelHeight, scale);

                if (e.Padding.HasValue)
                    total += Converter.mmtoUnits(e.Padding.Value);
            }
            return total;
        }
    }
}
