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

            if(instruction.Data == 0x00EE)
            {
                instruction.Opcode = Opcode.RET;
                return instruction;
            }

            switch (instruction.FirstNibble)
            {
                case 0x1:
                    instruction.Opcode = Opcode.JP_NNN;
                    break;
                case 0x2:
                    instruction.Opcode = Opcode.CALL_NNN;
                    break;
                case 0x3:
                    instruction.Opcode = Opcode.SE_VX_NN;
                    break;
                case 0x4:
                    instruction.Opcode = Opcode.SNE_VX_NN;
                    break;
                case 0x5:
                    instruction.Opcode = Opcode.SE_VX_VY;
                    break;
                case 0x6:
                    instruction.Opcode = Opcode.LD_VX_NN;
                    break;
                case 0x7:
                    instruction.Opcode = Opcode.ADD_VX_NN;
                    break;
                case 0x8:
                    switch (instruction.N)
                    {
                        case 0x0:
                            instruction.Opcode = Opcode.LD_VX_VY;
                            break;
                        case 0x1:
                            instruction.Opcode = Opcode.OR_VX_VY;
                            break;
                        case 0x2:
                            instruction.Opcode = Opcode.AND_VX_VY;
                            break;
                        case 0x3:
                            instruction.Opcode = Opcode.XOR_VX_VY;
                            break;
                        case 0x4:
                            instruction.Opcode = Opcode.ADD_VX_VY;
                            break;
                        case 0x5:
                            instruction.Opcode = Opcode.SUB_VX_VY;
                            break;
                        case 0x6:
                            instruction.Opcode = Opcode.SHR_VX_VY;
                            break;
                        case 0x7:
                            instruction.Opcode = Opcode.SUBN_VX_VY;
                            break;
                        case 0xE:
                            instruction.Opcode = Opcode.SHL_VX_VY;
                            break;
                    }
                    break;
                case 0x9:
                    instruction.Opcode = Opcode.SNE_VX_VY;
                    break;
                case 0xA:
                    instruction.Opcode = Opcode.LD_I;
                    break;
                case 0xB:
                    instruction.Opcode = Opcode.SNE_VX_VY;
                    break;
                case 0xC:
                    instruction.Opcode = Opcode.RND_VX_N;
                    break;
                case 0xD:
                    instruction.Opcode = Opcode.DRW_VX_VY_N;
                    break;
                case 0xE:
                    switch (instruction.NN)
                    {
                        case 0x9E:
                            instruction.Opcode = Opcode.SKP_VX;
                            break;
                        case 0xA1:
                            instruction.Opcode = Opcode.SKNP_VX;
                            break;
                    }
                    break;
                case 0xF:
                    switch (instruction.NN)
                    {
                        case 0x07:
                            instruction.Opcode = Opcode.LD_VX_DT;
                            break;
                        case 0x0A:
                            instruction.Opcode = Opcode.LD_VX_K;
                            break;
                        case 0x15:
                            instruction.Opcode = Opcode.LD_DT_VX;
                            break;
                        case 0x18:
                            instruction.Opcode = Opcode.LD_ST_VX;
                            break;
                        case 0x1E:
                            instruction.Opcode = Opcode.ADD_I_VX;
                            break;
                        case 0x29:
                            instruction.Opcode = Opcode.LD_F_VX;
                            break;
                        case 0x33:
                            instruction.Opcode = Opcode.LD_B_VX;
                            break;
                        case 0x55:
                            instruction.Opcode = Opcode.LD_I_VX;
                            break;
                        case 0x65:
                            instruction.Opcode = Opcode.LD_VX_I;
                            break;
                    }
                    break;
                default: throw new Exception("Opcode não identificado durante etapa de decodificação");
            }

            //Console.WriteLine($"Instrução {"0x" + instruction.ToHexString()} - {instruction.Opcode.ToString()} lida");

            return instruction;
        }
    }
}
