using System;
using Newtonsoft.Json;
using PowerArgs;

namespace GifTimelapse {
    class Program {
        static void Main(string[] args) {
            var parsed = Args.Parse<CommandLineArgs>(args);
            Console.WriteLine(JsonConvert.SerializeObject(parsed, Formatting.Indented));
            Console.WriteLine();

            var builder = new App.GifTimelapse(parsed);
            builder.Updatings += (sender, image) => Console.WriteLine($"file: {image.Filename}");
            var filename = builder.Build();

            Console.WriteLine($"{filename.Name} done!");
        }
    }
}
