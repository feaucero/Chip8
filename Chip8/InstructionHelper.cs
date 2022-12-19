using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public static class InstructionHelper
    {
        public static Instruction Decode(ushort firstInstruction, ushort secondInstruction)
        {
            var fetchedInstruction = (firstInstruction << 8) + secondInstruction;

            Instruction instruction = new Instruction(fetchedInstruction);

            // CLR
            if (instruction.Data == 0x00E0) 
            {
                instruction.Opcode = Opcode.CLS;
                return instruction;
            }

            Opcode opcode = Opcode.CLS;

            switch (instruction.FirstNibble)
            {
                case 0x1:
                    instruction.Opcode = Opcode.JP;
                    break;
                case 0x6:
                    instruction.Opcode = Opcode.LDVX;
                    break;
                case 0x7:
                    instruction.Opcode = Opcode.ADD;
                    break;
                case 0xA:
                    instruction.Opcode = Opcode.LDI;
                    break;
                case 0xD:
                    instruction.Opcode = Opcode.DRW;
                    break;
            }

            Console.WriteLine($"Instrução {"0x" + instruction.ToHexString()} lida");

            return instruction;
        }
    }
}
