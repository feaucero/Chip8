namespace Chip8.Core

{
    public interface IRenderer
    {
        bool Quit { get; }
        bool[,] Screen { get; }
        byte? CurrentKey { get; }
        bool Redraw { get; set; }
        int CycleSpeed { get; set; }

        void Beep();
        void ClearScreen();
        void Dispose();
        void DrawScreen();
        bool GetPixel(int x, int y);
        void ProcessEvents();
        void SetPixel(int x, int y, bool on);
        void UpdateScreen();
    }
}
