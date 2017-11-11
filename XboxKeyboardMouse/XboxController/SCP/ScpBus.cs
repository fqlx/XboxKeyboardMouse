/*
 * ScpDriverInterface - by Mogzol (and of course Scarlet.Crush) - Jan, 2016
 *
 * This is a simple little DLL which allows you to use Scarlet.Crush's SCP Virtual
 * Bus Driver to emulate Xbox 360 Controllers.
 * 
 * Most of the code here has been ripped out of his ScpControl source code, mostly
 * from the ScpDevice and BusDevice classes, so obviously credit and major props to
 * Scarlet.Crush, without him this wouldn't be possible. You can download his
 * original source code from here:
 * http://forums.pcsx2.net/Thread-XInput-Wrapper-for-DS3-and-Play-com-USB-Dual-DS2-Controller
 *
 * Note that for this to work the SCP Virtual Bus Driver must be installed. (Duh)
 */

using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
[assembly: CLSCompliant(true)]

namespace ScpDriverInterface {
    /// <summary>
    /// Emulates Xbox 360 controllers via Scarlet.Crush's SCP Virtual Bus Driver.
    /// </summary>
    public class ScpBus : IDisposable {
        private const string SCP_BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        private const int ReportSize = 28;

        private readonly SafeFileHandle _deviceHandle;

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        public ScpBus() : this(0) { }

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        /// <param name="instance">Specifies which SCP Virtual Bus device to use. This is 0-based.</param>
        public ScpBus(int instance) {
            string devicePath = "";

            if (Find(new Guid(SCP_BUS_CLASS_GUID), ref devicePath, instance)) {
                _deviceHandle = GetHandle(devicePath);
            } else {
                throw new IOException("SCP Virtual Bus Device not found");
            }
        }

        /// <summary>
        /// Creates a new ScpBus object, which will then try to get a handle to the specified SCP Virtual Bus device. If it is unable to get the handle, an IOException will be thrown.
        /// </summary>
        /// <param name="devicePath">The path to the SCP Virtual Bus device that you want to use.</param>
        public ScpBus(string devicePath) {
            _deviceHandle = GetHandle(devicePath);
        }

        /// <summary>
        /// Closes the handle to the SCP Virtual Bus device. Call this when you are done with your instance of ScpBus.
        /// 
        /// (This method does the same thing as the Dispose() method. Use one or the other.)
        /// </summary>
        public void Close() {
            Dispose();
        }

        /// <summary>
        /// Closes the handle to the SCP Virtual Bus device. Call this when you are done with your instance of ScpBus.
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (_deviceHandle != null && !_deviceHandle.IsInvalid) {
                _deviceHandle.Dispose();
            }
        }

        /// <summary>
        /// Plugs in an emulated Xbox 360 controller.
        /// </summary>
        /// <param name="controllerNumber">Used to identify the controller. Give each controller you plug in a different number. Number must be non-zero.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool PlugIn(int controllerNumber) {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4000, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }

        /// <summary>
        /// Unplugs an emulated Xbox 360 controller.
        /// </summary>
        /// <param name="controllerNumber">The controller you want to unplug.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool Unplug(int controllerNumber) {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new Byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            buffer[4] = (byte)((controllerNumber) & 0xFF);
            buffer[5] = (byte)((controllerNumber >> 8) & 0xFF);
            buffer[6] = (byte)((controllerNumber >> 16) & 0xFF);
            buffer[7] = (byte)((controllerNumber >> 24) & 0xFF);

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }

        /// <summary>
        /// Unplugs all emulated Xbox 360 controllers.
        /// </summary>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool UnplugAll() {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            int transfered = 0;
            byte[] buffer = new byte[16];

            buffer[0] = 0x10;
            buffer[1] = 0x00;
            buffer[2] = 0x00;
            buffer[3] = 0x00;

            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A4004, buffer, buffer.Length, null, 0, ref transfered, IntPtr.Zero);
        }

        /// <summary>
        /// Sends an input report for the current state of the specified emulated Xbox 360 controller. Note: Only use this if you don't care about rumble data, otherwise use the 3-parameter version of Report().
        /// </summary>
        /// <param name="controllerNumber">The controller to report.</param>
        /// <param name="controllerReport">The controller report. If using the included X360Controller class, this can be generated with the GetReport() method. Otherwise see http://free60.org/wiki/GamePad#Input_report for details.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool Report(int controllerNumber, byte[] controllerReport) {
            return Report(controllerNumber, controllerReport, null);
        }

        /// <summary>
        /// Sends an input report for the current state of the specified emulated Xbox 360 controller. If you care about rumble data, make sure you check the output report for rumble data every time you call this.
        /// </summary>
        /// <param name="controllerNumber">The controller to report.</param>
        /// <param name="controllerReport">The controller report. If using the included X360Controller class, this can be generated with the GetReport() method. Otherwise see http://free60.org/wiki/GamePad#Input_report for details.</param>
        /// <param name="outputBuffer">The buffer for the output report, which takes the form specified here: http://free60.org/wiki/GamePad#Output_report. Use an 8-byte buffer if you care about rumble data, or null otherwise.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public bool Report(int controllerNumber, byte[] controllerReport, byte[] outputBuffer) {
            if (_deviceHandle.IsInvalid)
                throw new ObjectDisposedException("SCP Virtual Bus device handle is closed");

            byte[] head = new byte[8];

            head[0] = 0x1C;
            head[4] = (byte)((controllerNumber) & 0xFF);
            head[5] = (byte)((controllerNumber >> 8) & 0xFF);
            head[6] = (byte)((controllerNumber >> 16) & 0xFF);
            head[7] = (byte)((controllerNumber >> 24) & 0xFF);

            byte[] fullReport = new byte[28];

            Buffer.BlockCopy(head, 0, fullReport, 0, head.Length);
            Buffer.BlockCopy(controllerReport, 0, fullReport, head.Length, controllerReport.Length);

            int transferred = 0;
            return NativeMethods.DeviceIoControl(_deviceHandle, 0x2A400C, fullReport, fullReport.Length, outputBuffer, outputBuffer?.Length ?? 0, ref transferred, IntPtr.Zero) && transferred > 0;
        }

        private static bool Find(Guid target, ref string path, int instance = 0) {
            IntPtr detailDataBuffer = IntPtr.Zero;
            IntPtr deviceInfoSet = IntPtr.Zero;

            try {
                NativeMethods.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new NativeMethods.SP_DEVICE_INTERFACE_DATA(), da = new NativeMethods.SP_DEVICE_INTERFACE_DATA();
                int bufferSize = 0, memberIndex = 0;

                deviceInfoSet = NativeMethods.SetupDiGetClassDevs(ref target, IntPtr.Zero, IntPtr.Zero, NativeMethods.DIGCF_PRESENT | NativeMethods.DIGCF_DEVICEINTERFACE);

                DeviceInterfaceData.cbSize = da.cbSize = Marshal.SizeOf(DeviceInterfaceData);

                while (NativeMethods.SetupDiEnumDeviceInterfaces(deviceInfoSet, IntPtr.Zero, ref target, memberIndex, ref DeviceInterfaceData)) {
                    NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, ref da);
                    detailDataBuffer = Marshal.AllocHGlobal(bufferSize);

                    Marshal.WriteInt32(detailDataBuffer, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);

                    if (NativeMethods.SetupDiGetDeviceInterfaceDetail(deviceInfoSet, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, ref da)) {
                        IntPtr pDevicePathName = detailDataBuffer + 4;

                        path = Marshal.PtrToStringAuto(pDevicePathName).ToUpper(CultureInfo.InvariantCulture);
                        Marshal.FreeHGlobal(detailDataBuffer);

                        if (memberIndex == instance) return true;
                    } else Marshal.FreeHGlobal(detailDataBuffer);


                    memberIndex++;
                }
            } finally {
                if (deviceInfoSet != IntPtr.Zero) {
                    NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
                }
            }

            return false;
        }

        private static SafeFileHandle GetHandle(string devicePath) {
            devicePath = devicePath.ToUpper(CultureInfo.InvariantCulture);

            SafeFileHandle handle = NativeMethods.CreateFile(devicePath, (NativeMethods.GENERIC_WRITE | NativeMethods.GENERIC_READ), NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_ATTRIBUTE_NORMAL | NativeMethods.FILE_FLAG_OVERLAPPED, UIntPtr.Zero);

            if (handle == null || handle.IsInvalid) {
                throw new IOException("Unable to get SCP Virtual Bus Device handle");
            }

            return handle;
        }
    }

    internal static class NativeMethods {
        [StructLayout(LayoutKind.Sequential)]
        internal struct SP_DEVICE_INTERFACE_DATA {
            internal int cbSize;
            internal Guid InterfaceClassGuid;
            internal int Flags;
            internal IntPtr Reserved;
        }

        internal const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        internal const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const uint FILE_SHARE_READ = 1;
        internal const uint FILE_SHARE_WRITE = 2;
        internal const uint GENERIC_READ = 0x80000000;
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint OPEN_EXISTING = 3;
        internal const int DIGCF_PRESENT = 0x0002;
        internal const int DIGCF_DEVICEINTERFACE = 0x0010;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, UIntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeviceIoControl(SafeFileHandle hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, ref int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern int SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        [DllImport("setupapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, int memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, IntPtr enumerator, IntPtr hwndParent, int flags);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, int deviceInterfaceDetailDataSize, ref int requiredSize, ref SP_DEVICE_INTERFACE_DATA deviceInfoData);
    }
}


