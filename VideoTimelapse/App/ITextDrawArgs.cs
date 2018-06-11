using System.Drawing;

namespace VideoTimeLapse.App {
    public interface ITextDrawArgs {
        string FontFamily { get; }
        float FontSize { get; }
        Color Color { get; }
        HorizontalPosition Horizontal { get; }
        VertialPosition Vertical { get; }
    }

    public enum HorizontalPosition {
        Left, Center, Right
    }

    public enum VertialPosition {
        Top, Middle, Bottom
    }
}