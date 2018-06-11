using System;
using Newtonsoft.Json;
using PowerArgs;
using VideoTimeLapse.App;

namespace VideoTimeLapse {
    class Program {
        static void Main(string[] args) {
            var parsed = Args.Parse<CommandLineArgs>(args);

            Console.WriteLine(JsonConvert.SerializeObject(parsed, Formatting.Indented));
            Console.WriteLine();

            var builder = new VideoTimelapse(parsed);
            var filename = builder.Build();

            Console.WriteLine($"{filename.Name} done!");
        }
    }
}
