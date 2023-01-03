using Microsoft.VisualBasic;
using Silk.NET.SDL;
using System;
using System.Diagnostics;
using System.Threading;
using Thread = System.Threading.Thread;

namespace Chip8
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using (var renderer = new SdlRenderer())
            {
                if (!renderer.Init())
                    throw new Exception();

                Emulator emulator = new Emulator(renderer);
                emulator.LoadRom("roms/Tetris [Fran Dachille, 1991].ch8");
                emulator.Run();
            }
        }
    }
}