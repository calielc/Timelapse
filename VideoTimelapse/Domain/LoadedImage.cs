using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace VideoTimeLapse.Domain {
    public struct LoadedImage : IDisposable {
        private readonly Bitmap _bitmap;

        public LoadedImage(string path) {
            Filename = Path.GetFileNameWithoutExtension(path);
            _bitmap = new Bitmap(path);
            DateTaken = GetDateTaken(_bitmap);
        }

        public LoadedImage(string path, IResolution resolution) {
            Filename = Path.GetFileNameWithoutExtension(path);
            _bitmap = new Bitmap(path);
            DateTaken = GetDateTaken(_bitmap);

            if (resolution.Width != _bitmap.Width || resolution.Height != _bitmap.Height) {
                var newBitMap = new Bitmap(_bitmap, resolution.Width, resolution.Height);
                _bitmap.Dispose();

                _bitmap = newBitMap;
            }
        }

        public string Filename { get; }

        public DateTime? DateTaken { get; }

        void IDisposable.Dispose() {
            _bitmap?.Dispose();
        }

        private static DateTime? GetDateTaken(Bitmap bitmap) {
            var property = bitmap.GetPropertyItem(36867);
            if (property is null) {
                return default;
            }

            var result = Encoding.UTF8.GetString(property.Value);
            try {
                return DateTime.ParseExact(result, "yyyy:MM:d HH:mm:ss\0", CultureInfo.InvariantCulture);
            }
            catch {
                return default;
            }
        }

        public static implicit operator Bitmap(LoadedImage self) => self._bitmap;
    }
}