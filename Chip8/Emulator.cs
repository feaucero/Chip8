using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// Armazena o endereço inicial das fontes
        /// </summary>
        private ushort FontMemory = 0x50;

        /// <summary>
        /// Registradores V(0-F)
        /// </summary>
        private byte[] V;

        /// <summary>
        /// Registrador de endereço de memoria.
        /// </summary>
        private ushort I;

        /// <summary>
        /// ProgramCounter. Armazena o endereço de execução atual
        /// </summary>
        private ushort PC;

        /// <summary>
        /// Stack. Mantem a informação da stack
        /// </summary>
        private Stack<ushort> Stack;

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

        private Stopwatch _timersStopWatch;

        private bool _showLog = false;

        public Emulator(SdlRenderer renderer)
        {
            _renderer = renderer;
            Memory = new byte[4096];
            V = new byte[16];
            Stack = new Stack<ushort>(16);
            I = 0;
            PC = ProgramMemory;
            SP = 0;
            ST = 0;
            DT = 0;

            _timersStopWatch = Stopwatch.StartNew();

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
            var fontAdress = FontMemory;
            foreach (var font in Fonts)
            {
                Memory[fontAdress] = font;
                fontAdress++;
            }


        }

        public void Run()
        {

            var cycleStopwatch = Stopwatch.StartNew();
            while (!_renderer.Quit)
            {
                if(cycleStopwatch.ElapsedTicks > 1000 / 1)
                {
                    _renderer.ProcessEvents();

                    cycleStopwatch.Restart();
                    _renderer.Redraw = false;

                    // Fetch
                    var firstInstruction = Memory[PC];
                    var secondInstruction = Memory[PC + 1];
                    PC += 2;

                    var instruction = InstructionHelper.Decode(firstInstruction, secondInstruction);

                    ShowPreExecutionLog(instruction);
                    Execute(instruction);
                    ShowPostExecutionLog(instruction);

                    if (_timersStopWatch.ElapsedMilliseconds > 1000 / 60)
                    {
                        if (DT > 0) DT--;
                        if (ST > 0) ST--;

                        if(ST > 0)
                        {
                            _renderer.Beep();
                        }

                        _timersStopWatch.Restart();
                    }

                    _renderer.Update();

                    Thread.Sleep(_renderer.CycleSpeed);
                }
            }
        }

        private void ShowPreExecutionLog(Instruction instruction)
        {
            Console.Clear();
            string debugMessage = $"Proxima Instrução: {instruction.ToHexString()}";
            ShowMessage(debugMessage);
            DumpData();

        }

        private void DumpData()
        {
            string infoDump = "Registradores: ";
            for (int i = 0; i < V.Length; i++)
            {
                infoDump += $"\nV[{i}] = {V[i]}";
            };
            infoDump += "\n";
            infoDump += $"\nI = {I}";
            infoDump += $"\nPC = {PC}";
            infoDump += $"\nSP = {SP}\n";

            infoDump += String.Join("\n", Stack.Select(x => $"Stack = {x}").ToArray());

            infoDump += $"\n\nDT = {DT}";
            infoDump += $"\nST = {ST}";

            infoDump += $"\nCurrentKey = {_renderer.CurrentKey}";

            ShowMessage(infoDump);
        }

        private void ShowPostExecutionLog(Instruction instruction)
        {
            string debugMessage = $"Instrução {instruction.ToHexString()} executada: ";
            ShowMessage(debugMessage);
            DumpData();
        }

        private void ShowMessage(string message)
        {
            if (_showLog)
            {
                Console.WriteLine(message);
            }
        }

        private void Execute(Instruction instruction)
        {
            var currentKey = _renderer.CurrentKey;

            switch (instruction.Opcode)
            {
                case Opcode.CLS:
                    _renderer.ClearScreen();
                    break;
                case Opcode.RET:
                    PC = Stack.Pop();
                    SP -= 1; 
                    break;
                case Opcode.JP_NNN:
                    PC = instruction.NNN;
                    break;
                case Opcode.CALL_NNN:
                    Stack.Push(PC);
                    SP += 1;
                    PC = instruction.NNN;
                    break;
                case Opcode.SE_VX_NN:
                    SkipIfEqual(V[instruction.X], instruction.NN);
                    break;
                case Opcode.SNE_VX_NN:
                    if (V[instruction.X] != instruction.NN)
                        PC += 2;
                    break;
                case Opcode.SE_VX_VY:
                    SkipIfEqual(V[instruction.X], V[instruction.Y]);
                    break;
                case Opcode.LD_VX_NN:
                    V[instruction.X] = instruction.NN;
                    break;
                case Opcode.ADD_VX_NN:
                    V[instruction.X] += instruction.NN;
                    break;
                case Opcode.LD_VX_VY:
                    V[instruction.X] = V[instruction.Y];
                    break;
                case Opcode.OR_VX_VY:
                    V[instruction.X] = (byte)(V[instruction.X] | V[instruction.Y]);
                    break;
                case Opcode.AND_VX_VY:
                    V[instruction.X] = (byte)(V[instruction.X] & V[instruction.Y]);
                    break;
                case Opcode.XOR_VX_VY:
                    V[instruction.X] = (byte)(V[instruction.X] ^ V[instruction.Y]);
                    break;
                case Opcode.ADD_VX_VY:
                    var sum = V[instruction.X] + V[instruction.Y];
                    V[instruction.X] = (byte)sum;
                    V[0xF] = sum > 255 ? (byte)1 : V[0xf];
                    break;
                case Opcode.SUB_VX_VY:
                    V[instruction.X] = Subtraction(V[instruction.X], V[instruction.Y]);
                    break;
                case Opcode.SHR_VX_VY:
                    V[0xF] = (byte)((0x1 & V[instruction.X]) > 0 ? 1 : 0);
                    V[instruction.X] = (byte)(V[instruction.Y] >> 1);
                    break;
                case Opcode.SUBN_VX_VY:
                    Subtraction(V[instruction.Y], V[instruction.X]);
                    break;
                case Opcode.SHL_VX_VY:
                    V[0xF] = (byte)((0x80 & V[instruction.X]) > 0 ? 1 : 0);
                    V[instruction.X] = (byte)(V[instruction.X] << 1);
                    break;
                case Opcode.SNE_VX_VY:
                    if (V[instruction.X] != V[instruction.Y])
                        PC += 2;
                    break;
                case Opcode.LD_I:
                    I = instruction.NNN;
                    break;
                case Opcode.JP_V0:
                    PC = (ushort)(instruction.NNN + V[0]);
                    break;
                case Opcode.RND_VX_N:
                    Random rnd = new Random();           
                    V[instruction.X] = (byte)(rnd.Next(0, 256) & instruction.NN);
                    break;
                case Opcode.DRW_VX_VY_N:
                    Draw(instruction);
                    break;
                case Opcode.SKP_VX:
                    if(currentKey.HasValue && currentKey == V[instruction.X])
                    {
                        PC += 2;
                    }
                    break;
                case Opcode.SKNP_VX:
                    Console.WriteLine($"CurrentKey = {currentKey}");
                    Console.WriteLine($"PC = {PC}");
                    if (!currentKey.HasValue || currentKey != V[instruction.X])
                    {
                        PC += 2;
                    }
                    break;
                case Opcode.LD_VX_DT:
                    V[instruction.X] = DT;
                    break;
                case Opcode.LD_VX_K:
                    if (!currentKey.HasValue)
                    {
                        PC -= 2;
                    } else
                    {
                        V[instruction.X] = currentKey.Value;
                    }                 
                    break;
                case Opcode.LD_DT_VX:
                    DT = V[instruction.X];
                    break;
                case Opcode.LD_ST_VX:
                    ST = V[instruction.X];
                    break;
                case Opcode.ADD_I_VX:
                    I += V[instruction.X];
                    break;
                case Opcode.LD_F_VX:
                    I = (ushort)(FontMemory + (5*V[instruction.X]));
                    break;
                case Opcode.LD_B_VX:
                    var value = V[instruction.X];
                    byte digit = (byte)(value % 10);
                    value /= 10;
                    byte decimalDigit = (byte)(value % 10);
                    value /= 10;
                    byte hundredDigit = (byte)(value % 10);
                    Memory[I + 2] = digit;
                    Memory[I + 1] = decimalDigit;
                    Memory[I] = hundredDigit;
                    break;
                case Opcode.LD_I_VX:
                    for (byte i = 0; i <= instruction.X; i++)
                    {
                        Memory[I+i] = V[i];
                        var a = Memory[I + i];
                    }
                    break;
                case Opcode.LD_VX_I:
                    for (byte i = 0; i <= instruction.X; i++)
                    {
                        V[i] = Memory[I + i];
                        var a = V[i];
                    }
                    break;
                default:
                    throw new Exception("Opcode não identificado durante etapa de execução");
            }
        }

        private byte Subtraction(byte firstValue, byte secondValue)
        {
            V[0xF] = (byte)(firstValue > secondValue ? 1 : 0);
            firstValue -= secondValue;
            return firstValue;
        }

        private void SkipIfEqual(byte firstValue, byte secondValue)
        {
            if (firstValue == secondValue)
                PC += 2;
        }

        private void Draw(Instruction instruction)
        {
            var height = instruction.N;
            V[0xF] = 0;

            for (int row = 0; row < height; row++)
            {
                byte rowData = Memory[I + row];
                for (int column = 0; column < 8; column++)
                {
                    var bitIndex = 7 - column;
                    var shifted = 1 << bitIndex;
                    var pixelStatus = (shifted & rowData) == shifted;

                    var x = (V[instruction.X] + column) & 63;
                    var y = (V[instruction.Y] + row) & 31;

                    var oldPixelStatus = _renderer.GetPixel(x, y);

                    if (oldPixelStatus && pixelStatus)
                    {
                        V[0xf] = 1;
                        _renderer.SetPixel(x, y, false);
                    }
                    else if (!oldPixelStatus && pixelStatus)
                    {
                        V[0xf] = 0;
                        _renderer.SetPixel(x, y, true);
                    }

                    //pixelStatus = pixelStatus ^ oldPixelStatus;

                    //V[0xf] = (byte)(!pixelStatus ? 1 : 0);

                    //_renderer.SetPixel(x, y, pixelStatus);
                }
            }
        }
    }
}
