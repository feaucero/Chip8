using Silk.NET.Maths;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public unsafe class SdlRenderer : IDisposable
    {
        private Sdl sdl;
        private Window* window;
        private Surface* surface;
        private uint off;
        private uint on;
        private int ScreenWidth = 640;
        private int ScreenHeight = 320;

        public bool[,] Screen { get; private set; }

        public bool Quit { get; private set; }
        public bool Redraw = false;

        public SdlRenderer()
        {
            sdl = Sdl.GetApi();
            Screen = new bool[64, 32];
        }

        public bool Init()
        {
            if (sdl.Init(Sdl.InitVideo) < 0)
            {
                Console.WriteLine($"Error: Failed to initialize SDL. {sdl.GetErrorS()}");
                return false;
            }

            window = sdl.CreateWindow("Chip-8 Emulator",
            Sdl.WindowposCentered,
            Sdl.WindowposCentered,
            ScreenWidth,
            ScreenHeight,
            (uint)(WindowFlags.AllowHighdpi | WindowFlags.Shown));
            if (window == null)
            {
                Console.WriteLine($"Error: Failed to create SDL Windows.  {sdl.GetErrorS()}");
                return false;
            }

            surface = sdl.GetWindowSurface(window);

            on = sdl.MapRGB(surface->Format, 20, 120, 170);
            off = sdl.MapRGB(surface->Format, 0, 0, 0);

            ClearScreen();
            sdl.UpdateWindowSurface(window);

            return true;
        }

        public void ClearScreen()
        {
            sdl.FillRect(surface, null, off);
        }

        public void DrawScreen()
        {
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    DrawPixel(x, y, Screen[x, y]);
                }
            }
            sdl.UpdateWindowSurface(window);
        }

        public void Update()
        {
            ProcessEvents();
            if(Redraw)
            {
                DrawScreen();
            }
            sdl.UpdateWindowSurface(window);
        }

        private void ProcessEvents()
        {
            Event e = new Event();
            while (sdl.PollEvent(ref e) != 0)
            {
                switch ((EventType)e.Type)
                {
                    case EventType.Quit:
                        Quit = true;
                        break;
                }
            }
        }

        public bool GetPixel(int x, int y) => Screen[x, y];

        public void SetPixel(int x, int y, bool on)
        {
            Screen[x, y] = on;
            Redraw = true;
        }

        void DrawPixel(int x, int y, bool pixelOn)
        {
            Rectangle<int> pixel = new Rectangle<int>(x * 10, y * 10, 10, 10);
            sdl.FillRect(surface, ref pixel, pixelOn ? on : off);
        }

        public void Dispose()
        {
            sdl.FreeSurface(surface);
            sdl.DestroyWindow(window);
            sdl.Quit();
        }
    }
}
