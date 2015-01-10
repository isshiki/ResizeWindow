using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResizeWindow
{
    public partial class MainWindow : Form
    {
        private static string thisTitle = "thistitle";

        public MainWindow()
        {
            InitializeComponent();
            thisTitle = this.Text;
            GetAllActiveWindow();
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern UInt64 GetWindowLongA(IntPtr hWnd, Int32 nIndex);
        private static readonly UInt64 WS_VISIBLE = 0x10000000L;
        private static readonly UInt64 WS_BORDER = 0x00800000L;
        private static readonly UInt64 DESIRED_WS = WS_BORDER | WS_VISIBLE;

        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        
        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int wFlags);

        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern UInt32 GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        private static Int32 GetWindowProcessID(IntPtr hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private static string GetAppName(IntPtr hWnd)
        {
            var caption = new StringBuilder(0x1000);
            GetWindowText(hWnd, caption, caption.Capacity);
            var captitle = caption.ToString();
            if (String.IsNullOrEmpty(captitle)) return String.Empty;

            Int32 pid = GetWindowProcessID(hWnd);
            Process p = Process.GetProcessById(pid);
            var appname = p.ProcessName;
            var maintitle = p.MainWindowTitle;

            if (captitle != maintitle) {
                if ((captitle == thisTitle) && String.IsNullOrEmpty(maintitle)) return String.Empty;
                if (appname == "explorer") {
                    if ((captitle == "Program Manager") && String.IsNullOrEmpty(maintitle)) return String.Empty;
                }
                else {
                    return String.Empty;
                }
            }

            return appname + (String.IsNullOrEmpty(captitle) ? "" : (" ｜ " + captitle));          
        }

        private static Dictionary<string, IntPtr> activeWindow = new Dictionary<string, IntPtr>();
        private static bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd)) {
                var appname = GetAppName(hWnd);
                if ((String.IsNullOrEmpty(appname) == false) && (activeWindow.ContainsKey(appname) == false)) {
                    activeWindow.Add(appname, hWnd);
                }
            }
            return true;
        }
        private void GetAllActiveWindow()
        {
            activeWindow.Clear();
            this.listBoxSelectedWindow.Items.Clear();
            EnumWindows(EnumerateWindow, IntPtr.Zero);
            foreach (var win in activeWindow) {
                this.listBoxSelectedWindow.Items.Add(win.Key);
            }
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            int left = SystemInformation.VirtualScreen.Left;
            int top = SystemInformation.VirtualScreen.Top;
            int width = SystemInformation.VirtualScreen.Width;
            int height = SystemInformation.VirtualScreen.Height;

            int iWidth;
            if (!int.TryParse(this.txtWidth.Text, out iWidth) || (iWidth < left | iWidth > width)) {
                MessageBox.Show("Please input correct width.");
                this.txtWidth.Focus();
                return;
            }

            int iHeight;
            if (!int.TryParse(this.txtHeight.Text, out iHeight) || (iHeight < top | iHeight > height)) {
                MessageBox.Show("Please input correct height.");
                    this.txtHeight.Focus();
                    return;
            }

            string sel = (string)this.listBoxSelectedWindow.SelectedItem;
            if (String.IsNullOrEmpty(sel)) {
                MessageBox.Show("Please select the target window to resize.");
                this.listBoxSelectedWindow.Focus();
                return;
            }

            try {
                var hWndTarget = activeWindow[sel];
                MainWindow.SetWindowPos(hWndTarget, IntPtr.Zero, 0, 0, iWidth, iHeight, 2);
            }
            catch (Exception ex) {
                MessageBox.Show("[Error] Cannot resize window.\n\n" + ex.Message);
                GetAllActiveWindow();
                return;
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            GetAllActiveWindow();
        }


    }
}
