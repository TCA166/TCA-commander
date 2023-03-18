using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace projektOOP
{
    //https://www.pinvoke.net/default.aspx/Enums/SHSTOCKICONID.html - codes for icons
    //based on https://stackoverflow.com/a/42933011/12520385
    internal class systemIcon
    {
        public const uint SHGSI_LARGEICON = 0x0;
        public const uint SHGSI_SMALLICON = 0x1;
        private const uint SHGSI_ICON = 0x100;

        public Icon icon;

        public uint code;
        public uint size;

        public systemIcon(uint code, uint size)
        {
            this.code = code;
            this.size = size;
            icon = GetStockIcon(code, size);
        }

        private static Icon GetStockIcon(uint type, uint size)
        {
            var info = new SHSTOCKICONINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            SHGetStockIconInfo(type, SHGSI_ICON | size, ref info);

            var icon = (Icon)Icon.FromHandle(info.hIcon).Clone(); // Get a copy that doesn't use the original handle
            DestroyIcon(info.hIcon); // Clean up native icon to prevent resource leak

            return icon;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public uint cbSize;
            public IntPtr hIcon;
            public int iSysIconIndex;
            public int iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }

        [DllImport("shell32.dll")]
        public static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);

    }
}
