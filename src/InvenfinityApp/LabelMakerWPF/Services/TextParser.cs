using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LabelMakerWPF.Services
{
    internal static class TextParser
    {
        public static (List<(int start, int length)>, StringBuilder) BoldFinder(string Text)
        {
            var boldRanges = new List<(int start, int length)>();
            var finalBuilder = new StringBuilder();
            bool isBoldActive = false;
            int boldStart = 0;

            foreach (char c in Text)
            {
                if (c == '*')
                {
                    if (!isBoldActive)
                    {
                        // Start des Fettdrucks
                        boldStart = finalBuilder.Length;
                        isBoldActive = true;
                    }
                    else
                    {
                        // Ende des Fettdrucks
                        int length = finalBuilder.Length - boldStart;
                        if (length > 0) boldRanges.Add((boldStart, length));
                        isBoldActive = false;
                    }
                }
                else
                {
                    finalBuilder.Append(c);
                }
            }
            return (boldRanges, finalBuilder);
        }
    }
}
