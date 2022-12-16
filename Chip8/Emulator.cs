using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8
{
    public class Emulator
    {
        private SdlRenderer _renderer;

        public Emulator(SdlRenderer renderer)
        {
            _renderer= renderer;
        }

        public void Run()
        {
            while(!_renderer.Quit)
            {
                _renderer.Update();
            }
        }
    }
}
