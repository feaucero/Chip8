namespace Chip8.Core

{
    public enum Opcode
    {
        CLS = 0x00E0, //00E0
        RET = 0x00EE, //00EE
        JP_NNN = 0x1000, //1nnn
        CALL_NNN = 0x2000, //2nnn
        SE_VX_NN = 0x3000, //3xkk
        SNE_VX_NN = 0x4000, //4xkk
        SE_VX_VY = 0x5000, //5xy0
        LD_VX_NN = 0x6000, //6xkk
        ADD_VX_NN = 0x7000, //7xkk
        LD_VX_VY = 0x8000, //8xy0
        OR_VX_VY = 0x8001, //8xy1
        AND_VX_VY = 0x8002, //8xy2
        XOR_VX_VY = 0x8003, //8xy3
        ADD_VX_VY = 0x8004, //8xy4
        SUB_VX_VY = 0x8005, //8xy5
        SHR_VX_VY = 0x8006, //8xy6
        SUBN_VX_VY = 0x8007, //8xy7
        SHL_VX_VY = 0x800E, //8xyE
        SNE_VX_VY = 0x9000, //9xy0
        LD_I = 0xA000, //Annn
        JP_V0 = 0xB000, //Bnnn
        RND_VX_N = 0XC0000, //Cxkk
        DRW_VX_VY_N = 0xD000, //Dxyn
        SKP_VX = 0xE09E, // Ex9E
        SKNP_VX = 0xE0A1, // ExA1
        LD_VX_DT = 0xF007, //Fx07
        LD_VX_K = 0xF00A, //Fx0A
        LD_DT_VX = 0xF015, //Fx15
        LD_ST_VX = 0xF018, // Fx18
        ADD_I_VX = 0xF01E, //Fx1E
        LD_F_VX = 0xF029, //Fx29
        LD_B_VX = 0xF033, //Fx33
        LD_I_VX = 0xF055, //Fx55
        LD_VX_I = 0xF065, //Fx65

    }
}
