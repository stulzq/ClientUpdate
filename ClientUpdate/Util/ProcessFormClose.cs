using System;
using System.Runtime.InteropServices;

namespace ClientUpdate.Util
{
    public class ProcessFormClose
    {
        /// <summary>
        /// 设定样式用的常量
        /// </summary>
        private const int SC_CLOSE = 0xF060;

        private const int MF_ENABLED = 0x00000000;
        private const int MF_GRAYED = 0x00000001;
        private const int MF_DISABLED = 0x00000002;

        /// <summary>
        /// 获取指定句柄的窗体的标题栏
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

        /// <summary>
        /// 设置标题栏的关闭按钮的样式
        /// </summary>
        [DllImport("User32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

        public static void DisableCloseButton(IntPtr ptr)
        {
            IntPtr hMenu = ProcessFormClose.GetSystemMenu(ptr, 0);
            EnableMenuItem(hMenu, SC_CLOSE, (MF_DISABLED + MF_GRAYED) | MF_ENABLED);
        }
    }
}