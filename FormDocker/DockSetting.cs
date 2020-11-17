using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormDocker
{
    public enum DockValueTypes
    {
        Pixels, Percent
    }
    public class DockValue
    {
        public DockValueTypes Type { get; set; }
        float mValue;

        public int Pixels
        {
            get => (int)mValue; set
            {
                mValue = (int)value;
            }
        }

        public float Percent
        {
            get => mValue; 
            set {
                mValue = value;
                if (mValue < 0) mValue = 0;
                if (mValue > 1) mValue = 1;
            }
        }
    }

    public class OrientationSetting
    {
        public DockValue MaxHeight { get; set; }
        public DockValue MaxWidth { get; set; }

        public virtual int CalcMaxHeight(int screenIndex)
        {
            if (MaxHeight.Type == DockValueTypes.Percent) 
                return (int)(MaxHeight.Percent * Screen.AllScreens[screenIndex].WorkingArea.Height);
            return MaxHeight.Pixels;
        }

        public virtual int CalcMaxWidth(int screenIndex)
        {
            if (MaxWidth.Type == DockValueTypes.Percent)
                return (int)(MaxWidth.Percent * Screen.AllScreens[screenIndex].WorkingArea.Width);
            return MaxWidth.Pixels;
        }
    }

    public class DockSetting
    {
        public OrientationSetting Portrait { get; set; }
        public OrientationSetting Landscape { get; set; }
    }
}
