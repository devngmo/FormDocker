using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormDocker
{
    public enum FormDockStyles { 
        /// <summary>
        /// top of the screen
        /// </summary>
        TopMid, TopLeft, TopRight,
        /// <summary>
        /// bottom of the screen
        /// </summary>
        BottomMid, BottomLeft, BottomRight,
        /// <summary>
        /// Left of the screen
        /// </summary>
        LeftMid, LeftTop, LeftBottom,
        /// <summary>
        /// Right of the screen
        /// </summary>
        RightMid, RightTop, RightBottom,
        /// <summary>
        /// Fill the screen
        /// </summary>
        Fill,
        Center
    }
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

        FormDockStyles mDockStyle;
        public FormDockStyles DockStyle { get => mDockStyle; }

        public void Dock(FormDockStyles style)
        {
            mDockStyle = style;
            ApplyDockStyle();
        }

        private void ApplyDockStyle()
        {
            if (IsDockLeft)
            {
                DockOnLeft();
            }
            else if (IsDockRight)
            {
                DockOnRight();
            }
            else if (IsDockBottom)
            {
                DockOnBottom();
            }
            else if (IsDockTop)
            {
                DockOnTop();
            }
            else if (mDockStyle == FormDockStyles.Fill)
            {
                DockFill();
            }
        }

        private void DockFill()
        {
            mHost.Location = new System.Drawing.Point(0,0);
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
        }

        public int ScreenIndex { get => mScreenIndex; }
        public Screen CurrentScreen
        {
            get => Screen.AllScreens[mScreenIndex];
        }

        public bool IsDockLeft => mDockStyle == FormDockStyles.LeftBottom || mDockStyle == FormDockStyles.LeftMid || mDockStyle == FormDockStyles.LeftTop;
        public bool IsDockRight => mDockStyle == FormDockStyles.RightBottom || mDockStyle == FormDockStyles.RightMid || mDockStyle == FormDockStyles.RightTop;
        public bool IsDockTop => mDockStyle == FormDockStyles.TopLeft || mDockStyle == FormDockStyles.TopMid || mDockStyle == FormDockStyles.TopRight;
        public bool IsDockBottom => mDockStyle == FormDockStyles.BottomLeft || mDockStyle == FormDockStyles.BottomMid || mDockStyle == FormDockStyles.BottomRight;
        public int MaxWidth
        {
            get
            {
                if (IsDockLeft || IsDockRight)
                    return mDockSetting.Portrait.CalcMaxWidth(mScreenIndex);
                return mDockSetting.Landscape.CalcMaxWidth(mScreenIndex);
            }
        }

        public int MaxHeight
        {
            get
            {
                if (IsDockLeft || IsDockRight)
                    return mDockSetting.Portrait.CalcMaxHeight(mScreenIndex);
                return mDockSetting.Landscape.CalcMaxHeight(mScreenIndex);
            }
        }

        private void DockOnLeft()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int top = (CurrentScreen.Bounds.Bottom - mHost.Size.Height) / 2; // mid
            if (mDockStyle == FormDockStyles.LeftTop) top = 0; // top
            else if (mDockStyle == FormDockStyles.LeftBottom) top = CurrentScreen.Bounds.Bottom - mHost.Size.Height; // bottom
            mHost.Location = new System.Drawing.Point(CurrentScreen.Bounds.Left, top);
        }

        private void DockOnRight()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int top = (CurrentScreen.Bounds.Bottom - mHost.Size.Height) / 2; // mid
            if (mDockStyle == FormDockStyles.RightTop) top = 0; // top
            else if (mDockStyle == FormDockStyles.RightBottom) top = CurrentScreen.Bounds.Bottom - mHost.Size.Height; // bottom
            mHost.Location = new System.Drawing.Point(CurrentScreen.Bounds.Right - mHost.Width, top);
        }

        private void DockOnBottom()
        {
            mHost.Size = new System.Drawing.Size(MaxWidth, MaxHeight);
            int left = CurrentScreen.Bounds.Left + (CurrentScreen.Bounds.Width - mHost.Width) / 2; // mid
            if (mDockStyle == FormDockStyles.BottomLeft) left = 0; // left
            else if (mDockStyle == FormDockStyles.BottomRight) left = CurrentScreen.Bounds.Right - mHost.Size.Width; // right
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
            if (mDockStyle == FormDockStyles.TopLeft) left = 0; // left
            else if (mDockStyle == FormDockStyles.TopRight) left = CurrentScreen.Bounds.Right - mHost.Size.Width; // right
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
