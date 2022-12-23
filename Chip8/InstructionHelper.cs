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

            Instruction instruction = new Instruction((ushort)fetchedInstruction);

            // CLR
            if (instruction.Data == 0x00E0) 
            {
                instruction.Opcode = Opcode.CLS;
                return instruction;
            }

            switch (instruction.FirstNibble)
            {
                case 0x1:
                    instruction.Opcode = Opcode.JP_NNN;
                    break;
                case 0x6:
                    instruction.Opcode = Opcode.LD_VX_NN;
                    break;
                case 0x7:
                    instruction.Opcode = Opcode.ADD_VX_NN;
                    break;
                case 0xA:
                    instruction.Opcode = Opcode.LD_I;
                    break;
                case 0xD:
                    instruction.Opcode = Opcode.DRW_VX_VY_N;
                    break;
            }

            Console.WriteLine($"Instrução {"0x" + instruction.ToHexString()} lida");

            return instruction;
        }
    }
}
