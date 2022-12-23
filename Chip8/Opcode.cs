using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8

{
    public enum Opcode
    {
        CLS, //00E0
        //RET,
        JP_NNN, //1nnn
        //CALL, //2nnn
        //SEVX, //3xkk
        //SNE, //4xkk
        //SEVXVY, //5xy0
        LD_VX_NN, //6xkk
        ADD_VX_NN, //7xkk
        LD_I, //Annn
        DRW_VX_VY_N, //Dxyn

    }
}
