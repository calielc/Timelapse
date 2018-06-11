using Domain;

namespace GitTimelapse.App {
    internal interface ITimelapseBuilderArgs {
        Resolution Resolution { get; }
        FramesPerSecond FramesPerSecond { get; }

        string SourceFolder { get; }
        string SourcePattern { get; }

        string DestinyFolder { get; }
        string DestinyFilename { get; }
    }
}