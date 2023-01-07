using Chip8.Core;
using Silk.NET.SDL;
using System.Runtime.InteropServices;

namespace Chip8.Forms
{
    public unsafe partial class Form : System.Windows.Forms.Form
    {
        private Sdl sdl;

        private Panel gamePanel;
        private Window* window; // For FNA, this is Game.Window.Handle
        private Surface* surface;
        private Emulator emulator;
        SdlRenderer renderer;

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(
        IntPtr handle,
        IntPtr handleAfter,
        int x,
        int y,
        int cx,
        int cy,
        uint flags
        );

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr handle, int command);

        public Form()
        {
            InitializeComponent();

            sdl = Sdl.GetApi();

            //gamePanel = new Panel();
            //gamePanel.Size = new Size(640, 320);
            //gamePanel.Location = new System.Drawing.Point(80, 10);

            //Controls.Add(gamePanel);

            sdl.Init(Sdl.InitVideo);

            window = sdl.CreateWindow("Chip-8 Emulator",
            0,
            0,
            emulatorPanel.Size.Width,
            emulatorPanel.Size.Height,
            (uint)(WindowFlags.Borderless | WindowFlags.Opengl));

            surface = sdl.GetWindowSurface(window);

            SysWMInfo wm_info = new SysWMInfo();
            sdl.GetWindowWMInfo(window, &wm_info);
            var winhandle = wm_info.Info.Win.Hwnd;

            SetWindowPos(winhandle, Handle, 0, 0, 0, 0, 0x0401);
            SetParent(winhandle, emulatorPanel.Handle);
            ShowWindow(winhandle, 1); // SHOWNORMAL
        }

        private void button1_Click(object sender, EventArgs e)
        {
            emulator.Run();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            emulator.LoadRom(@"D:\\Projetos\\net\\Chip8\\Chip8\\roms\\PONG.ch8");
            emulator.Run();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.Dispose();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            renderer = new SdlRenderer(sdl, window, surface);

            if (!renderer.Init())
                throw new Exception();

            emulator = new Emulator(renderer);
        }
    }
}