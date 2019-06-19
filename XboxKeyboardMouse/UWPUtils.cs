using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XboxKeyboardMouse
{
    internal struct WINDOWINFO
    {
        public uint ownerpid;
        public uint childpid;
    }

    public class UWPUtils
    {
        #region User32
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        
        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);
        #endregion

        #region Kernel32
        public const UInt32 PROCESS_QUERY_INFORMATION = 0x400;
        public const UInt32 PROCESS_VM_READ = 0x010;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool QueryFullProcessImageName([In]IntPtr hProcess, [In]int dwFlags, [Out]StringBuilder lpExeName, ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)]
            Boolean bInheritHandle,
            Int32 dwProcessId
        );
        #endregion

        /// <summary>
        /// Gets the path of application running in foreground
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <returns>Path of the application</returns>
        public static string GetProcessName(IntPtr hWnd)
        {
            string processName = null;

            hWnd = GetForegroundWindow();

            if (hWnd == IntPtr.Zero)
                return null;

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);

            IntPtr proc;
            if ((proc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, (int)pID)) == IntPtr.Zero)
                return null;

            int capacity = 2000;
            StringBuilder sb = new StringBuilder(capacity);
            QueryFullProcessImageName(proc, 0, sb, ref capacity);

            processName = sb.ToString(0, capacity);

            // UWP apps are wrapped in another app called, if this has focus then try and find the child UWP process
            if (Path.GetFileName(processName).Equals("ApplicationFrameHost.exe"))
            {
                processName = UWP_AppName(hWnd, pID);
            }

            return processName;
        }

        #region Get UWP Application Name

        /// <summary>
        /// Find child process for uwp apps, edge, mail, etc.
        /// </summary>
        /// <param name="hWnd">hWnd</param>
        /// <param name="pID">pID</param>
        /// <returns>The application name of the UWP.</returns>
        private static string UWP_AppName(IntPtr hWnd, uint pID)
        {
            WINDOWINFO windowinfo = new WINDOWINFO();
            windowinfo.ownerpid = pID;
            windowinfo.childpid = windowinfo.ownerpid;

            IntPtr pWindowinfo = Marshal.AllocHGlobal(Marshal.SizeOf(windowinfo));

            Marshal.StructureToPtr(windowinfo, pWindowinfo, false);

            EnumWindowProc lpEnumFunc = new EnumWindowProc(EnumChildWindowsCallback);
            EnumChildWindows(hWnd, lpEnumFunc, pWindowinfo);

            windowinfo = (WINDOWINFO)Marshal.PtrToStructure(pWindowinfo, typeof(WINDOWINFO));

            IntPtr proc;
            if ((proc = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, (int)windowinfo.childpid)) == IntPtr.Zero)
                return null;

            int capacity = 2000;
            StringBuilder sb = new StringBuilder(capacity);
            QueryFullProcessImageName(proc, 0, sb, ref capacity);

            Marshal.FreeHGlobal(pWindowinfo);

            return sb.ToString(0, capacity);
        }

        /// <summary>
        /// Callback for enumerating the child windows.
        /// </summary>
        /// <param name="hWnd">hWnd</param>
        /// <param name="lParam">lParam</param>
        /// <returns>always <c>true</c>.</returns>
        private static bool EnumChildWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            WINDOWINFO info = (WINDOWINFO)Marshal.PtrToStructure(lParam, typeof(WINDOWINFO));

            uint pID;
            GetWindowThreadProcessId(hWnd, out pID);

            if (pID != info.ownerpid)
                info.childpid = pID;

            Marshal.StructureToPtr(info, lParam, true);

            return true;
        }
        #endregion
    }
}
