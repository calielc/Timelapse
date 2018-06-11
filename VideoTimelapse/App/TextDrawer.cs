using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VideoTimeLapse.App {
    public class TextDrawer {
        public Font Font { get; set; }
        public Brush Color { get; set; }
        public RectangleF Position { get; set; }

        public void Draw(Image image, string text) {
            if (string.IsNullOrWhiteSpace(text)) {
                return;
            }

            var graphics = Graphics.FromImage(image);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawString(text, Font, Color, Position);
            graphics.Flush();
        }

        public void Draw(Image image, DateTime dateTime, string format = null) {
            Draw(image, dateTime.ToString(format));
        }

        public void Draw(Image image, DateTime? dateTime, string format = null) {
            if (dateTime is null) {
                return;
            }
            Draw(image, dateTime.Value.ToString(format));
        }
    }
}