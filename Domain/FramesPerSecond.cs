using System;
using System.Diagnostics;

namespace Domain {
    public abstract class FramesPerSecond {
        [DebuggerDisplay("{Value}fps")]
        public sealed class Fixed : FramesPerSecond {
            public Fixed(double value) {
                Value = value;
            }

            public double Value { get; }

            public override string ToString() => $"{Value}fps";
        }

        [DebuggerDisplay("{Value}")]
        public sealed class DurationFit : FramesPerSecond {
            public DurationFit(TimeSpan value) {
                Value = value;
            }

            public TimeSpan Value { get; }

            public Fixed Resolve(int frames) {
                return new Fixed(frames / Value.TotalSeconds);
            }

            public override string ToString() => Value.ToString();
        }
    }
}