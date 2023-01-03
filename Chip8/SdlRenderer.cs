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

        public byte? CurrentKey { get; private set; }

        public bool[,] Screen { get; private set; }

        public bool Quit { get; private set; }
        public bool Redraw = false;

        public int CycleSpeed = 5;

        public SdlRenderer()
        {
            sdl = Sdl.GetApi();
            Screen = new bool[64, 32];
            //Console.SetBufferSize(64, 32);
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
            Screen = new bool[64, 32];
            Redraw = true;
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

        public void Beep()
        {
            Console.Beep();
        }

        //string DebugScreen(bool[,] screen)
        //{
        //    var builder = new StringBuilder();
        //    builder.AppendLine();
        //    builder.AppendLine("┌────────────────────────────────────────────────────────────────┐");
        //    for (int y = 0; y < 32; y++)
        //    {
        //        builder.Append('│');
        //        for (int x = 0; x < 64; x++)
        //        {
        //            builder.Append(screen[x, y] ? '█' : ' ');
        //        }
        //        builder.Append('│');
        //        builder.AppendLine();
        //    }
        //    builder.AppendLine("└────────────────────────────────────────────────────────────────┘");
        //    return builder.ToString();
        //}

        public void Update()
        {
            if(Redraw)
            {
                DrawScreen();
            }
            sdl.UpdateWindowSurface(window);
        }

        public void ProcessEvents()
        {
            Event e = new Event();
            while (sdl.PollEvent(ref e) != 0)
            {
                switch ((EventType)e.Type)
                {
                    case EventType.Quit:
                        Quit = true;
                        break;
                    case EventType.Keydown:
                        CurrentKey = GetKeyPress(e);
                        break;
                    case EventType.Keyup:
                        CurrentKey = null;
                        break;
                }
            }
        }

        private byte? GetKeyPress(Event e)
        {
            byte? key = null;
            switch ((KeyCode)e.Key.Keysym.Sym)
            {
                case KeyCode.K1:
                    key = 0x1;
                    break;
                case KeyCode.K2:
                    key = 0x2;
                    break;
                case KeyCode.K3:
                    key = 0x3;
                    break;
                case KeyCode.K4:
                    key = 0xC;
                    break;
                case KeyCode.KQ:
                    key = 0x4;
                    break;
                case KeyCode.KW:
                    key = 0x5;
                    break;
                case KeyCode.KE:
                    key = 0x6;
                    break;
                case KeyCode.KR:
                    key = 0xD;
                    break;
                case KeyCode.KA:
                    key = 0x7;
                    break;
                case KeyCode.KS:
                    key = 0x8;
                    break;
                case KeyCode.KD:
                    key = 0x9;
                    break;
                case KeyCode.KF:
                    key = 0xE;
                    break;
                case KeyCode.KZ:
                    key = 0xA;
                    break;
                case KeyCode.KX:
                    key = 0x0;
                    break;
                case KeyCode.KC:
                    key = 0xB;
                    break;
                case KeyCode.KV:
                    key = 0xF;
                    break;
                case KeyCode.KPlus:
                case KeyCode.KKPPlus:
                    CycleSpeed = CycleSpeed - 2 < 0 ? 0 : CycleSpeed - 2;
                    Console.WriteLine($"Cycle speed set to {CycleSpeed}");
                    break;
                case KeyCode.KMinus:
                case KeyCode.KKPMinus:
                    CycleSpeed = CycleSpeed + 2 > 1000 ? 0 : CycleSpeed + 2;
                    Console.WriteLine($"Cycle speed set to {CycleSpeed}");
                    break;
            }
            return key;
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
