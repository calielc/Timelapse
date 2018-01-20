using System;
using System.Drawing;
using Timelapse.Domain;

namespace Timelapse {
    public sealed class CommandLineTextDrawArgs : ITextDrawArgs {
        public string FontFamily { get; set; } = "Tahome";
        public float FontSize { get; set; } = 8;
        public Color Color { get; set; } = Color.White;
        public HorizontalPosition Horizontal { get; set; } = HorizontalPosition.Left;
        public VertialPosition Vertical { get; set; } = VertialPosition.Top;

        public static CommandLineTextDrawArgs Parse(string text) {
            var result = new CommandLineTextDrawArgs();

            var parts = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts) {
                if (float.TryParse(part, out var size)) {
                    result.FontSize = size;
                    continue;
                }
                if (Enum.TryParse(part, true, out HorizontalPosition horizontal)) {
                    result.Horizontal = horizontal;
                    continue;
                }
                if (Enum.TryParse(part, true, out VertialPosition vertical)) {
                    result.Vertical = vertical;
                    continue;
                }

                var color = Color.FromName(part);
                if (color.IsKnownColor) {
                    result.Color = color;
                    continue;
                }

                result.FontFamily = part;
            }

            return result;
        }
    }
}