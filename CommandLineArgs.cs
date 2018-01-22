using System;
using System.IO;
using Accord.Video.FFMPEG;
using PowerArgs;
using Timelapse.Domain;

namespace Timelapse {
    public class CommandLineArgs : ITimelapseBuilderArgs {
        private string _destinyFolder;
        private string _destinyFilename;

        public VideoCodec Codec { get; set; } = VideoCodec.H264;

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
                var result = _destinyFilename ?? "video.mov";

                if (!Path.HasExtension(result)) {
                    result += ".mov";
                }

                if (DestinyFilenameWithLocalTime) {
                    var name = Path.GetFileNameWithoutExtension(result);
                    var ext = Path.GetExtension(result);
                    result = $"{name}-{DateTime.Now:yyyyMMddHHmmss}{ext}";
                }

                return result;
            }
            set => _destinyFilename = value;
        }
        public bool DestinyFilenameWithLocalTime { get; set; }

        public string DateTakenFormat { get; set; }
        public CommandLineTextDrawArgs DateTakenDraw { get; set; } = new CommandLineTextDrawArgs();
        ITextDrawArgs ITimelapseBuilderArgs.DateTakenDraw => DateTakenDraw;

        [ArgReviver]
        public static CommandLineTextDrawArgs Revive(string key, string val) => CommandLineTextDrawArgs.Parse(val);
    }
}