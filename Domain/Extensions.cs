using System;
using System.Drawing;

namespace Timelapse.Domain {
    internal static class Extensions {
        public static Resolution.Fixed Resolve(this Resolution self, ImagesLoader imagesLoader) {
            var resolutionFromFirstFile = imagesLoader.GetResolutionFromFirstFile();

            switch (self) {
                case null:
                    return resolutionFromFirstFile;
                case Resolution.Fixed @fixed:
                    return @fixed;
                case Resolution.HeightFix heightFix:
                    return heightFix.Resolve(resolutionFromFirstFile);
                case Resolution.WidthFix widthFix:
                    return widthFix.Resolve(resolutionFromFirstFile);
                default:
                    throw new ArgumentOutOfRangeException(nameof(self));
            }
        }

        public static FramesPerSecond.Fixed Resolve(this FramesPerSecond self, ImagesLoader imagesLoader) {
            switch (self) {
                case null:
                    return new FramesPerSecond.Fixed(24);
                case FramesPerSecond.Fixed @fixed:
                    return @fixed;
                case FramesPerSecond.DurationFit durationFit:
                    return durationFit.Resolve(imagesLoader.Count);
                default:
                    throw new ArgumentOutOfRangeException(nameof(self));
            }
        }

        public static TextDrawer Resolve(this ITextDrawArgs self, IResolution resolution, string sample) {
            const float padding = 10;

            var font = new Font(self.FontFamily, self.FontSize);

            SizeF size;
            using (var bitmap = new Bitmap(resolution.Width, resolution.Height)) {
                var grafics = Graphics.FromImage(bitmap);
                size = grafics.MeasureString(sample, font);
            }

            var x = GetHorizontal();
            var y = GetVertical();

            return new TextDrawer {
                Font = font,
                Color = new SolidBrush(self.Color),
                Position = new RectangleF(x, y, size.Width, size.Height)
            };

            float GetHorizontal() {
                switch (self.Horizontal) {
                    case HorizontalPosition.Left:
                        return padding;
                    case HorizontalPosition.Center:
                        return (resolution.Width - size.Width) / 2;
                    case HorizontalPosition.Right:
                        return resolution.Width - size.Width - padding;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            float GetVertical() {
                switch (self.Vertical) {
                    case VertialPosition.Top:
                        return padding;
                    case VertialPosition.Middle:
                        return (resolution.Height - size.Height) / 2;
                    case VertialPosition.Bottom:
                        return resolution.Height - size.Height - padding;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}