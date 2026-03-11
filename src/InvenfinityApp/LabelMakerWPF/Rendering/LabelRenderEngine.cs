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
using System.Printing;
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
        public void PrintVekor(DrawingGroup toPint, IPrinter Printer, bool showDialog = true)
        {
            double actualContentWidth = toPint.Bounds.Width + Printer.XOffset;

            // Erstelle ein Visual, das wir manuell verschieben können
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                double xOffset = Printer.XOffset;
                double yOffset = Printer.YOffset;
                dc.PushTransform(new TranslateTransform(xOffset, yOffset));
                dc.DrawDrawing(toPint);

                dc.Pop();
            }

            var pd = new PrintDialog();

            if (showDialog)
            {
                if (pd.ShowDialog() != true) return;
            }
            else
            {
                pd.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            }

            var ticket = pd.PrintTicket;
            ticket.PageOrientation = System.Printing.PageOrientation.Landscape;
            double widthInUnits = Converter.mmtoUnits(12);
            double lengthInUnits = actualContentWidth;

            ticket.PageMediaSize = new PageMediaSize(widthInUnits, lengthInUnits);
            pd.PrintTicket = ticket;
            pd.PrintVisual(visual, "Label Druck");
        }
    }
}