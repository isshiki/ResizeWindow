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

        #region Win32APIs        

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

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
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int wFlags);

        [DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern UInt32 GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetAncestor(IntPtr hWnd, UInt32 gaFlags);
        private static readonly UInt32 GA_ROOTOWNER = 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        #endregion

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

            return String.Format("{0,-15} [{1:D7}]", appname, (int)hWnd) + (String.IsNullOrEmpty(captitle) ? "" : (" ｜ " + captitle));          
        }

        private static Dictionary<string, IntPtr> activeWindow = new Dictionary<string, IntPtr>();
        private static bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd) && IsAltTabWindow(hWnd)) {
                var appname = GetAppName(hWnd);
                if (String.IsNullOrEmpty(appname) == false) {
                    if (activeWindow.ContainsKey(appname) == false) {
                        activeWindow.Add(appname, hWnd);
                    }
                }
            }
            return true;
        }

        private static bool IsAltTabWindow(IntPtr hWnd)
        {
            IntPtr hWndWalk = GetAncestor(hWnd, GA_ROOTOWNER);

            IntPtr hWndTry;
            while ((hWndTry = GetLastActivePopup(hWndWalk)) != hWndTry) {
                if (IsWindowVisible(hWndTry)) break;
                hWndWalk = hWndTry;
            }
            return hWndWalk == hWnd;
        }

        private static bool IsEnumeratingWindows = false;
        private static Object lockObj = new Object();

        private void GetAllActiveWindow()
        {
            if (IsEnumeratingWindows) return;
            lock (lockObj) {
                IsEnumeratingWindows = true;
                var prevActiveWinKeys = this.listBoxSelectedWindow.Items.Cast<string>();
                activeWindow.Clear();
                EnumWindows(EnumerateWindow, IntPtr.Zero);
                var itemsToAdd = activeWindow.Keys.Except(prevActiveWinKeys).ToArray();
                var itemsToRemove = prevActiveWinKeys.Except(activeWindow.Keys).ToArray();
                if (itemsToAdd.Count() > 0) {
                    foreach (var winKey in itemsToAdd) {
                        if (this.listBoxSelectedWindow.Items.Contains(winKey) == false) {
                            this.listBoxSelectedWindow.Items.Add(winKey);
                        }
                    }
                }
                if (itemsToRemove.Count() > 0) {
                    foreach (var winKey in itemsToRemove) {
                        if (this.listBoxSelectedWindow.Items.Contains(winKey)) {
                            this.listBoxSelectedWindow.Items.Remove(winKey);
                        }
                    }
                }
                IsEnumeratingWindows = false;
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

            string sel = GetSelectedWindow();
            if (sel == null) return;

            try {
                var hWndTarget = activeWindow[sel];
                MainWindow.SetWindowPos(hWndTarget, IntPtr.Zero, 0, 0, iWidth, iHeight, 2);
            }
            catch (Exception ex) {
                MessageBox.Show("[Error] Cannot resize window.\n\n" + ex.Message, "Error");
                GetAllActiveWindow();
                return;
            }
        }

        private string GetSelectedWindow()
        {
            string sel = (string)this.listBoxSelectedWindow.SelectedItem;
            if (String.IsNullOrEmpty(sel)) {
                MessageBox.Show("Please select the target window to resize.");
                this.listBoxSelectedWindow.Focus();
                return null;
            }
            return sel;
        }

        private void listBoxSelectedWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sel = GetSelectedWindow();
            if (sel == null) return;

            try {
                var hWndTarget = activeWindow[sel];
                RECT rct;
                if (!GetWindowRect(hWndTarget, out rct)) {
                    MessageBox.Show("[Error] Cannot get window size.", "Error");
                    return;
                }
                this.txtWidth.Text = (rct.Right - rct.Left).ToString();
                this.txtHeight.Text = (rct.Bottom - rct.Top).ToString();

            }
            catch (Exception ex) {
                MessageBox.Show("[Error] Cannot get window size.\n\n" + ex.Message, "Error");
                GetAllActiveWindow();
                return;
            }
        }

        private void timerReload_Tick(object sender, EventArgs e)
        {
            GetAllActiveWindow();
        }


    }
}
