﻿using VisioAutomation.Text.Markup;
using VA = VisioAutomation;
using IVisio = Microsoft.Office.Interop.Visio;
using VisioAutomation.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace VisioAutomationSamples
{
    public static class extensions
    {
        public static VA.Text.Markup.TextElement AddElementEx(this VA.Text.Markup.TextElement p, string text,
                                                              string font, double? size, int? color,
                                                              VA.Drawing.AlignmentHorizontal? halign,
                                                              VA.Text.CharStyle? cs)
        {
            var el = p.AppendElement(text);
            if (font != null)
            {
                el.TextCharFormat.Font = font;
            }
            if (size.HasValue)
            {
                el.TextCharFormat.FontSize = size.Value;
            }
            if (color.HasValue)
            {
                el.TextCharFormat.Color = new VA.Drawing.ColorRGB(color.Value);
            }
            if (halign.HasValue)
            {
                el.TextParaFormat.HAlign = halign.Value;
            }

            if (cs.HasValue)
            {
                el.TextCharFormat.CharStyle = cs;
            }

            return el;
        }

    }

    public static class TextMarkpSamples
    {

        public static void TextMarkup11()
        {
            var page = SampleEnvironment.Application.ActiveDocument.Pages.Add();

            // Create the Shapes that will hold the text
            var s1 = page.DrawRectangle(0, 0, 8.5, 1);
            var s2 = page.DrawRectangle(0, 1, 8.5, 2);
            var s3 = page.DrawRectangle(0, 2, 8.5, 4);

            var e1 = get_markup_1();
            e1.SetText(s1);


            var e2 = get_markup_2();
            e2.SetText(s2);

            var e3 = get_markup_3();
            e3.SetText(s3);

        }

        private static TextElement get_markup_1()
        {
            var e1 = new VA.Text.Markup.TextElement();
            e1.TextCharFormat.Color = new VA.Drawing.ColorRGB(0xff0000);
            e1.TextCharFormat.Font = "Times New Roman";
            e1.TextCharFormat.FontSize = 20;
            e1.AppendText("Hello World");
            return e1;
        }

        private static TextElement get_markup_2()
        {
            var e1 = new VA.Text.Markup.TextElement();
            e1.TextCharFormat.Color = new VA.Drawing.ColorRGB(0xff0000);
            e1.TextCharFormat.Font = "Times New Roman";
            e1.TextCharFormat.FontSize = 20;
            e1.AppendText("Hello ");

            var e2 = e1.AddElementEx("World", null, null, null, null, VA.Text.CharStyle.Italic);
            return e1;
        }

        private static TextElement get_markup_3()
        {
            var e1 = new VA.Text.Markup.TextElement();
            e1.AppendText("When, from behind that craggy steep\n");
            e1.AppendText("till then the horizon’s bound\n");
            var e2 = e1.AddElementEx("a huge peak, black and huge\n", null, null, null, VA.Drawing.AlignmentHorizontal.Left, VA.Text.CharStyle.Italic);
            var e3 = e1.AddElementEx("As if with voluntary power instinct\n", null, null, null, VA.Drawing.AlignmentHorizontal.Center, VA.Text.CharStyle.Italic);
            var e4 = e1.AddElementEx("Upreared its head.\n", null, null, null, VA.Drawing.AlignmentHorizontal.Right, VA.Text.CharStyle.Italic);
            e1.AppendText("-William Wordsworth, the Prelude");
            return e1;
        }

    }
}