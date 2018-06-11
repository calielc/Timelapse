using System;
using Domain;

namespace GitTimelapse.App {
    internal static class Extensions {
        public static TimeSpan Resolve(this FramesPerSecond self, ImagesLoader imagesLoader) {
            switch (self) {
                case null:
                    return TimeSpan.FromSeconds(2500);
                case FramesPerSecond.Fixed @fixed:
                    return TimeSpan.FromSeconds(1 / @fixed.Value);
                case FramesPerSecond.DurationFit durationFit: {
                        var @fixed = durationFit.Resolve(imagesLoader.Count);
                        return TimeSpan.FromSeconds(1 / @fixed.Value);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(self));
            }
        }

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
    }
}