using System;
using System.Diagnostics;
using System.Drawing;

namespace Timelapse.Domain {
    public abstract class Resolution {
        [DebuggerDisplay("Width: {Width}")]
        public sealed class WidthFix : Resolution {
            public WidthFix(int width) {
                Width = width;
            }

            public int Width { get; }

            public Fixed Resolve(Fixed original) {
                var ratio = 1d * original.Width / Width;
                var height = original.Height / ratio;
                return new Fixed(Width, Convert.ToInt32(height));
            }

            public override string ToString() => $"Width: {Width}";
        }

        [DebuggerDisplay("Height: {Height}")]
        public sealed class HeightFix : Resolution {
            public HeightFix(int height) {
                Height = height;
            }

            public int Height { get; }

            public Fixed Resolve(Fixed original) {
                var ratio = 1d * original.Height / Height;
                var width = original.Width / ratio;
                return new Fixed(Convert.ToInt32(width), Height);
            }

            public override string ToString() => $"Height: {Height}";
        }

        [DebuggerDisplay("Width: {Width}, Height: {Height}")]
        public sealed class Fixed : Resolution, IResolution {
            public Fixed(int width, int height) {
                Width = width;
                Height = height;
            }

            public Fixed(Image image) {
                Width = image.Width;
                Height = image.Height;
            }

            public int Width { get; }
            public int Height { get; }

            public Fixed AsEven() {
                var height = Height % 2 == 0 ? Height : Height - 1;
                var width = Width % 2 == 0 ? Width : Width - 1;

                return new Fixed(width, height);
            }

            public override string ToString() => $"Width: {Width}, Height: {Height}";
        }
    }
}