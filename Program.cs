using System;
using System.Threading;
using Newtonsoft.Json;
using PowerArgs;
using Timelapse.Domain;

namespace Timelapse {
    class Program {
        static void Main(string[] args) {
            var parsed = Args.Parse<CommandLineArgs>(args);

            Console.WriteLine(JsonConvert.SerializeObject(parsed, Formatting.Indented));
            Console.WriteLine();
            Thread.Sleep(3000);

            var builder = new TimelapseBuilder(parsed);
            var filename = builder.Build();
            Console.WriteLine($"{filename.Name} done!");
        }
    }
}
