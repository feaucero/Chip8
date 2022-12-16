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

        public bool Quit { get; private set; }

        public SdlRenderer()
        {
            sdl = Sdl.GetApi();
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

            off = sdl.MapRGB(surface->Format, 0, 0, 0);
            on = sdl.MapRGB(surface->Format, 0, 255, 0);

            sdl.FillRect(surface, null, off);
            sdl.UpdateWindowSurface(window);

            return true;
        }

        public void Update()
        {
            ProcessEvents();
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

        public void Dispose()
        {
            sdl.FreeSurface(surface);
            sdl.DestroyWindow(window);
            sdl.Quit();
        }
    }
}
