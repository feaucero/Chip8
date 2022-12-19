using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public class Instruction
    {
        public int Data;
        public int FirstNibble { get { return Data & 0xF000; } }
        public int SecondNibble { get { return Data & 0xF00; } }
        public int ThirdNibble { get { return Data & 0xF0; } }
        public int FourthNibble { get { return Data & 0xF; } }
        public Opcode Opcode { get; set; }

        public Instruction(int data)
        {
            Data = data;
        }

        public string ToHexString()
        {
            return Data.ToString("x4");
        }
    }
}
