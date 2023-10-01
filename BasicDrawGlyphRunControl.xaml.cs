using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Win2DDrawGlyphRunTest
{
    public sealed partial class BasicDrawGlyphRunControl : UserControl
    {
        private const string Text = "😀";
        private readonly CanvasGlyph[] _glyphs;
        private readonly IReadOnlyList<KeyValuePair<CanvasCharacterRange, CanvasScaledFont>> _font;
        private readonly CanvasTextFormat _textFormat = new()
        {
            FontSize = 36,
            Options = CanvasDrawTextOptions.EnableColorFont,
        };
        private CanvasSolidColorBrush? _textBrush;

        public BasicDrawGlyphRunControl()
        {
            DataContext = this;
            InitializeComponent();

            var textAnalyzer = new CanvasTextAnalyzer(Text, _textFormat.Direction);

            _font = textAnalyzer.GetFonts(_textFormat);
            var scripts = textAnalyzer.GetScript();
            _glyphs = textAnalyzer.GetGlyphs(_font[0].Key, _font[0].Value.FontFace, _textFormat.FontSize, isSideways: false, isRightToLeft: false, scripts[0].Value);
        }

        private void OnCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            _textBrush ??= new CanvasSolidColorBrush(sender, Colors.Black);

            // DrawText
            args.DrawingSession.DrawText(Text, new Vector2(0, 0), _textBrush, _textFormat);

            // DrawTextLayout
            var textLayout = new CanvasTextLayout(sender, Text, _textFormat, float.PositiveInfinity, float.PositiveInfinity)
            {
                Options = CanvasDrawTextOptions.EnableColorFont,
            };
            args.DrawingSession.DrawTextLayout(textLayout, new Vector2(50, 0), _textBrush);

            // DrawGlyphRun
            var fontFace = _font[0].Value.FontFace;
            var position = new Vector2(100, (fontFace.Ascent + fontFace.LineGap) * _textFormat.FontSize);
            _textFormat.Options = CanvasDrawTextOptions.EnableColorFont;
            args.DrawingSession.DrawGlyphRun(position, fontFace, _textFormat.FontSize, _glyphs, false, 0, _textBrush);
        }
    }
}