using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public class Instruction
    {
        public ushort Data;
        public int FirstNibble { get; }
        public byte X { get; }
        public byte Y { get; }
        public byte N { get; }
        public byte NN { get; }
        public ushort NNN { get; }
        public Opcode Opcode { get; set; }

        public Instruction(ushort data)
        {
            Data = data;
            FirstNibble = (byte)((Data & 0xF000) >> 12);
            X = (byte)((Data & 0xF00) >> 8);
            Y = (byte)((Data & 0xF0) >> 4);
            N = (byte)(Data & 0xF);
            NN = (byte)((Y << 4) + N);
            NNN = (ushort)((X << 8) + NN);
        }

        public string ToHexString()
        {
            return Data.ToString("x4");
        }

        public string ToAssembly()
        {
            return Data.ToString("x4");
        }
    }
}
