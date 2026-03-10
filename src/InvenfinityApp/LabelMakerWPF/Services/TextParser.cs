using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace LabelMakerWPF.Services
{
    internal static class TextParser
    {
        public static string SplitTextCenter(string Text, string SplitChar)
        {
            // ... (dein bestehender Code zum Einfügen von \n bleibt gleich)
            int count = 0;
            for (int i = 0; i < Text.Length; i++)
                if (Text[i].ToString() == SplitChar) count++;

            if (count > 0)
            {
                int targetOccurrence = (count + 1) / 2;
                int occurrence = 0;
                for (int i = 0; i < Text.Length; i++)
                {
                    if (Text[i].ToString() == SplitChar)
                    {
                        occurrence++;
                        if (occurrence == targetOccurrence)
                        {
                            Text = Text.Substring(0, i) + "\n" + SplitChar + Text.Substring(i + 1);
                            break;
                        }
                    }
                }
            }
            return Text;
        }
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
