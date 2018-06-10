using System;
using System.IO;
using Accord.Video.FFMPEG;

namespace VideoTimeLapse.Domain {
    internal sealed class TimelapseBuilder : ITimelapseBuilderArgs {
        public TimelapseBuilder() { }

        public TimelapseBuilder(ITimelapseBuilderArgs args) {
            Codec = args.Codec;
            Resolution = args.Resolution;
            FramesPerSecond = args.FramesPerSecond;

            SourceFolder = args.SourceFolder;
            SourcePattern = args.SourcePattern;

            DestinyFolder = args.DestinyFolder;
            DestinyFilename = args.DestinyFilename;

            DateTakenFormat = args.DateTakenFormat;
            DateTakenDraw = args.DateTakenDraw;
        }

        public VideoCodec Codec { get; set; } = VideoCodec.Default;
        public Resolution Resolution { get; set; }
        public FramesPerSecond FramesPerSecond { get; set; }

        public string SourceFolder { get; set; }
        public string SourcePattern { get; set; }

        public string DestinyFolder { get; set; }
        public string DestinyFilename { get; set; }

        public string DateTakenFormat { get; set; }
        public ITextDrawArgs DateTakenDraw { get; set; }

        public FileInfo Build() {
            var imagesLoader = new ImagesLoader(SourceFolder, SourcePattern);

            var frameRate = FramesPerSecond.Resolve(imagesLoader);
            var resolution = Resolution.Resolve(imagesLoader).AsEven();

            var dateTakenDrawer = DateTakenFormat is null
                ? null
                : DateTakenDraw.Resolve(resolution, DateTime.Now.ToString(DateTakenFormat));

            Directory.CreateDirectory(DestinyFolder);
            var destiny = Path.Combine(DestinyFolder, DestinyFilename);

            using (var writer = new VideoFileWriter()) {
                writer.Width = resolution.Width;
                writer.Height = resolution.Height;
                writer.FrameRate = frameRate;
                writer.VideoCodec = Codec;
                writer.BitRate = int.MaxValue;
                writer.AudioCodec = AudioCodec.None;
                writer.AudioBitRate = 0;
                writer.SampleRate = 0;
                writer.Open(destiny);

                foreach (var image in imagesLoader.Load(resolution)) {
                    dateTakenDrawer?.Draw(image, image.DateTaken, DateTakenFormat);

                    writer.WriteVideoFrame(image);
                }

                writer.Flush();
                writer.Close();
            }

            return new FileInfo(destiny);
        }
    }
}