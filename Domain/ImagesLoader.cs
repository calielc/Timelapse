using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Domain {
    [DebuggerDisplay("Folder: {Folder}, Pattern: {Pattern}, Count: {Count}")]
    public sealed class ImagesLoader {
        private readonly string[] _filenames;

        public ImagesLoader(string folder) : this(folder, "*.jpg") { }

        public ImagesLoader(string folder, string pattern) {
            Folder = folder;
            Pattern = pattern;

            _filenames = Directory.GetFiles(folder, pattern, SearchOption.TopDirectoryOnly).OrderBy(filename => filename).ToArray();
            Count = _filenames.Length;
        }

        public string Folder { get; }
        public string Pattern { get; }
        public int Count { get; }

        public Resolution.Fixed GetResolutionFromFirstFile() {
            var filename = _filenames.First();
            using (var bitmap = new Bitmap(filename)) {
                return new Resolution.Fixed(bitmap);
            }
        }

        public IEnumerable<LoadedImage> Load() {
            foreach (var filename in _filenames) {
                using (var image = new LoadedImage(filename)) {
                    yield return image;
                }
            }
        }

        public IEnumerable<LoadedImage> Load(IResolution resolution) {
            if (resolution is null) {
                throw new ArgumentNullException(nameof(resolution));
            }

            foreach (var filename in _filenames) {
                using (var image = new LoadedImage(filename, resolution)) {
                    yield return image;
                }
            }
        }
    }
}