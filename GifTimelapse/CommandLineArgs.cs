using System;
using System.IO;
using Domain;
using GifTimelapse.App;
using PowerArgs;

namespace GifTimelapse {
    public class CommandLineArgs : ITimelapseBuilderArgs {
        private string _destinyFolder;
        private string _destinyFilename;

        [ArgRange(1, 120)]
        public double? FrameRate { get; set; }
        public TimeSpan? TotalTime { get; set; }
        [ArgIgnore]
        public FramesPerSecond FramesPerSecond => TotalTime != null
            ? (FramesPerSecond)new FramesPerSecond.DurationFit(TotalTime.Value)
            : new FramesPerSecond.Fixed(FrameRate ?? 24);

        [ArgRange(1, 3000)]
        public int? Width { get; set; }
        [ArgRange(1, 3000)]
        public int? Height { get; set; }
        [ArgIgnore]
        public Resolution Resolution {
            get {
                if (Width.HasValue && Height.HasValue) {
                    return new Resolution.Fixed(Width.Value, Height.Value);
                }
                if (Width.HasValue) {
                    return new Resolution.WidthFix(Width.Value);
                }
                if (Height.HasValue) {
                    return new Resolution.HeightFix(Height.Value);
                }
                return null;
            }
        }

        [ArgRequired(PromptIfMissing = true)]
        [ArgExistingDirectory]
        public string SourceFolder { get; set; }
        public string SourcePattern { get; set; } = "*.jpg";

        public string DestinyFolder {
            get => _destinyFolder ?? SourceFolder;
            set => _destinyFolder = value;
        }
        public string DestinyFilename {
            get {
                var result = _destinyFilename ?? "timelapse.gif";

                if (!Path.HasExtension(result)) {
                    result += ".gif";
                }

                return result;
            }
            set => _destinyFilename = value;
        }
    }
}