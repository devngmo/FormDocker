using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFormDocker
{
    public partial class FrmDemo : Form
    {
        private FormDocker.FormDocker mFormDocker;
        public FrmDemo()
        {
            InitializeComponent();
            FormDocker.DockSetting setting = new FormDocker.DockSetting() { 
                Portrait = new FormDocker.OrientationSetting() {
                    MaxWidth = new FormDocker.DockValue() { Type = FormDocker.DockValueTypes.Percent, Percent = 0.3f },
                    MaxHeight = new FormDocker.DockValue() { Type = FormDocker.DockValueTypes.Percent, Percent = 0.8f }
                },
                Landscape = new FormDocker.OrientationSetting()
                {
                    MaxWidth = new FormDocker.DockValue() { Type = FormDocker.DockValueTypes.Percent, Percent = 0.8f },
                    MaxHeight = new FormDocker.DockValue() { Type = FormDocker.DockValueTypes.Percent, Percent = 0.3f }
                }
            };
            mFormDocker = new FormDocker.FormDocker(this, setting);
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.Dock(DockStyle.Left);
            updateInfo();
        }

        private void nextScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.NextScreen();
            updateInfo();
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.Dock(DockStyle.Right); 
            updateInfo();
        }

        private void updateInfo()
        {
            lblInfo.Text = 
                $"Screen {mFormDocker.ScreenIndex}\nWorkingArea {mFormDocker.CurrentScreen.WorkingArea}" +
                $"Bounds {mFormDocker.CurrentScreen.Bounds}" +
                $"\n\nForm:\nLocation {Location}\nSize {Size}"
                ;
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.Dock(DockStyle.Top);
            updateInfo();

        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.Dock(DockStyle.Bottom);
            updateInfo();
        }

        private void freeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFormDocker.Dock(DockStyle.None);
            updateInfo();
        }

        private void FrmDemo_Move(object sender, EventArgs e)
        {
            updateInfo();
        }


        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        protected override void WndProc(ref Message message)
        {
            if (mFormDocker.DockStyle == DockStyle.None)
                base.WndProc(ref message);
            
            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

        private void FrmDemo_FormClosing(object sender, FormClosingEventArgs e)
        {
            mFormDocker.Close();
        }
    }
}
