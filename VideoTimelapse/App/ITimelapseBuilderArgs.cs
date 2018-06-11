using Accord.Video.FFMPEG;
using Domain;

namespace VideoTimeLapse.App {
    internal interface ITimelapseBuilderArgs {
        VideoCodec Codec { get; }
        Resolution Resolution { get; }
        FramesPerSecond FramesPerSecond { get; }

        string SourceFolder { get; }
        string SourcePattern { get; }

        string DestinyFolder { get; }
        string DestinyFilename { get; }

        string DateTakenFormat { get; }
        ITextDrawArgs DateTakenDraw { get; }
    }
}