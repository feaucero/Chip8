using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public class Emulator
    {
        private SdlRenderer _renderer;

        /// <summary>
        /// Memoria do Chip 8
        /// </summary>
        private byte[] Memory;


        /// <summary>
        /// Armazena o endereço inicial da ROM
        /// </summary>
        private ushort ProgramMemory = 0x200;

        /// <summary>
        /// Registradores V(0-F)
        /// </summary>
        private ushort[] V;

        /// <summary>
        /// Registradore de endereço de memoria.
        /// </summary>
        private ushort I;

        /// <summary>
        /// ProgramCounter. Armazena o endereço de execução atual
        /// </summary>
        private ushort PC;

        /// <summary>
        /// Stack. Mantem a informação da stack
        /// </summary>
        private ushort[] Stack;

        /// <summary>
        /// StackPointer. Aponta para o topo da stack
        /// </summary>
        private byte SP;

        /// <summary>
        /// SoundTimer. Timer basico para execuçao de bip sonoro.
        /// </summary>
        private byte ST;

        /// <summary>
        /// DelayTimer. Timer basico
        /// </summary>
        private byte DT;

        private byte[] Fonts;

        private int InstructionsPerSecond = 700;

        public Emulator(SdlRenderer renderer)
        {
            _renderer = renderer;
            Memory = new byte[4096];
            V = new ushort[16];
            Stack = new ushort[16];
            I = 0;
            PC = ProgramMemory;
            SP = 0;
            ST = 0;
            DT = 0;

            SetupFonts();
        }

        public void LoadRom(string romPath)
        {
            if (!File.Exists(romPath)) 
            {
                throw new FileNotFoundException(romPath);
            }

            var rom = File.ReadAllBytes(romPath);

            Array.Copy(rom, 0, Memory, ProgramMemory, rom.Length);
        }

        private void SetupFonts()
        {
            Fonts = new byte[80] { 0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                                    0x20, 0x60, 0x20, 0x20, 0x70, // 1
                                    0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                                    0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                                    0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                                    0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                                    0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                                    0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                                    0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                                    0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                                    0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                                    0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                                    0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                                    0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                                    0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                                    0xF0, 0x80, 0xF0, 0x80, 0x80 }; // F;
            var fontAdress = 0x50;
            foreach (var font in Fonts)
            {
                Memory[fontAdress] = font;
                fontAdress++;
            }


        }

        public void Run()
        {
            while (!_renderer.Quit)
            {
                var watch = Stopwatch.StartNew();

                Console.WriteLine("Buscando");
                var instructions = 0;
                for (int seconds = 0; seconds < 60; seconds++)
                {
                    for (int i = 0; i < InstructionsPerSecond / 60; i++)
                    {
                        // Fetch
                        var firstInstruction = Memory[PC];
                        var secondInstruction = Memory[PC+1];
                        PC += 2;

                        var instruction = InstructionHelper.Decode(firstInstruction, secondInstruction);

                        switch (instruction.Opcode)
                        {
                            case Opcode.CLS:
                                break;
                            case Opcode.JP:
                                break;
                            case Opcode.LDVX:
                                break;
                            case Opcode.ADD:
                                break;
                            case Opcode.LDI:
                                break;
                            case Opcode.DRW:
                                break;
                            default:
                                break;
                        }

                        // Execute
                        _renderer.Update();
                        instructions++;
                    }

                    if (watch.ElapsedMilliseconds <= 1000)
                    {
                        Thread.Sleep(1000 - (int)watch.ElapsedMilliseconds);
                        Console.WriteLine("Atrasando");
                    }
                }

                Console.WriteLine($"Finalizando {instructions} ciclos em {watch.ElapsedMilliseconds}ms");
            }
        }
    }
}
