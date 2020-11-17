using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormDocker
{
    public class FormDocker
    {
        Form mHost;
        DockSetting mDockSetting;
        int mScreenIndex = 0;
        public FormDocker(Form host, DockSetting setting)
        {
            mHost = host;
            mDockSetting = setting;
        }

        DockStyle mDockStyle;
        public DockStyle DockStyle { get => mDockStyle; }

        public void Dock(DockStyle style)
        {
            mDockStyle = style;
            ApplyDockStyle();
        }

        private void ApplyDockStyle()
        {
            if (mDockStyle == DockStyle.Left)
            {
                DockOnLeft();
            }
            else if (mDockStyle == DockStyle.Right)
            {
                DockOnRight();
            }
            else if (mDockStyle == DockStyle.Bottom)
            {
                DockOnBottom();
            }
            else if (mDockStyle == DockStyle.Top)
            {
                DockOnTop();
            }
            else if (mDockStyle == DockStyle.None)
            {
            }
        }

        public int ScreenIndex { get => mScreenIndex; }
        public Screen CurrentScreen
        {
            get => Screen.AllScreens[mScreenIndex];
        }

        public int MaxWidth
        {
            get
            {
                if (mDockStyle == DockStyle.Left || mDockStyle == DockStyle.Right)
                    return mDockSetting.Portrait.CalcMaxWidth(mScreenIndex);
                return mDockSetting.Landscape.CalcMaxWidth(mScreenIndex);
            }
        }

        public int MaxHeight
        {
            get
            {
                if (mDockStyle == DockStyle.Left || mDockStyle == DockStyle.Right)
                    return mDockSetting.Portrait.CalcMaxHeight(mScreenIndex);
                return mDockSetting.Landscape.CalcMaxHeight(mScreenIndex);
            }
        }

        private void DockOnLeft()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int top = (CurrentScreen.Bounds.Bottom - mHost.Size.Height) / 2;
            mHost.Location = new System.Drawing.Point(CurrentScreen.Bounds.Left, top);
        }

        private void DockOnRight()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int top = (CurrentScreen.Bounds.Bottom - mHost.Size.Height) / 2;
            mHost.Location = new System.Drawing.Point(CurrentScreen.Bounds.Right - mHost.Width, top);
        }

        private void DockOnBottom()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int left = CurrentScreen.Bounds.Left + (CurrentScreen.Bounds.Width - mHost.Width) / 2;
            mHost.Location = new System.Drawing.Point(left, CurrentScreen.Bounds.Bottom - MaxHeight);
        }

        bool hasCloseRequest = false;
        public void Close()
        {
            hasCloseRequest = true;
        }

        private void DockOnTop()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int left = CurrentScreen.Bounds.Left + (CurrentScreen.Bounds.Width - mHost.Width) / 2;
            mHost.Location = new System.Drawing.Point(left, 0);
        }

        public void NextScreen()
        {
            if (mScreenIndex + 1 < Screen.AllScreens.Length)
                mScreenIndex++;
            else
                mScreenIndex = 0;
            ApplyDockStyle();
        }
    }
}
