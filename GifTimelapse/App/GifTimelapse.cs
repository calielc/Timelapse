using System;
using System.IO;
using Domain;
using ImageMagick;

namespace GifTimelapse.App {
    internal class GifTimelapse {
        public GifTimelapse() { }

        public GifTimelapse(ITimelapseBuilderArgs args) {
            Resolution = args.Resolution;
            FramesPerSecond = args.FramesPerSecond;

            SourceFolder = args.SourceFolder;
            SourcePattern = args.SourcePattern;

            DestinyFolder = args.DestinyFolder;
            DestinyFilename = args.DestinyFilename;
        }

        public Resolution Resolution { get; set; }
        public FramesPerSecond FramesPerSecond { get; set; }

        public string SourceFolder { get; set; }
        public string SourcePattern { get; set; }

        public string DestinyFolder { get; set; }
        public string DestinyFilename { get; set; }

        public event EventHandler<LoadedImage> Updatings;

        public FileInfo Build() {
            var imagesLoader = new ImagesLoader(SourceFolder, SourcePattern);

            var frameRate = (int)(FramesPerSecond.Resolve(imagesLoader).TotalSeconds * 100);
            var resolution = Resolution.Resolve(imagesLoader).AsEven();

            var loadedImages = imagesLoader.Load(resolution);

            Directory.CreateDirectory(DestinyFolder);
            var destiny = Path.Combine(DestinyFolder, DestinyFilename);

            using (var magickImageCollection = new MagickImageCollection()) {
                foreach (var image in loadedImages) {
                    magickImageCollection.Add(new MagickImage(image) {
                        AnimationDelay = frameRate
                    });
                    OnUpdatings(image);
                }

                magickImageCollection.Quantize(new QuantizeSettings {
                    Colors = int.MaxValue,
                    ColorSpace = ColorSpace.RGB,
                });

                magickImageCollection.Write(destiny);
            }

            return new FileInfo(destiny);
        }

        protected virtual void OnUpdatings(LoadedImage image) => Updatings?.Invoke(this, image);
    }
}