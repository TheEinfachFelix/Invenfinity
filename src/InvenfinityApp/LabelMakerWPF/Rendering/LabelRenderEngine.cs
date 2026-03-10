using LabelMaker.Models.Label;
using LabelMaker.Models.Label.Elements;
using LabelMaker.Services;
using LabelMakerWPF.Templates.Printer;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace LabelMaker.Rendering
{
    internal class LabelRenderEngine
    {
        public void Print(LabelRoot label, IPrinter Printer)
        {
            double labelHeightUnits = Converter.mmtoUnits(Math.Min(12,Printer.MaxYSize));
            double labelWidthUnits = Converter.mmtoUnits(label.LabelLength);

            var vector = label.BuildVector(labelHeightUnits);

            // Erstelle ein Visual, das wir manuell verschieben können
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                double xOffset = Printer.XOffset;
                double yOffset = Printer.YOffset; // Nutze hier deinen IPrinter-Wert

                // 2. Erstelle eine Verschiebung (Translation)
                // Damit wird der gesamte Vektor um x und y verschoben gezeichnet.
                dc.PushTransform(new TranslateTransform(xOffset, yOffset));

                // 3. Zeichne das Label
                dc.DrawDrawing(vector);

                // 4. Transform schließen
                dc.Pop();
            }

            var pd = new PrintDialog();

            var ticket = pd.PrintTicket;
            ticket.PageOrientation = System.Printing.PageOrientation.Landscape;
            pd.PrintTicket = ticket;

            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(visual, "Label Druck");
            }
        }

        public RenderTargetBitmap ToBitmap(DrawingGroup vector, double width, double height)
        {
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
                dc.DrawDrawing(vector);

            var bmp = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Pbgra32);

            bmp.Render(visual);

            return bmp;
        }
    }
}