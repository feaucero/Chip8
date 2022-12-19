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
        JP, //1nnn
        //CALL, //2nnn
        //SEVX, //3xkk
        //SNE, //4xkk
        //SEVXVY, //5xy0
        LDVX, //6xkk
        ADD, //7xkk
        LDI, //Annn
        DRW, //Dxyn

    }
}
