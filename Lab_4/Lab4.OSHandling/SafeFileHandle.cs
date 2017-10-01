using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Lab4.OSHandling
{
    public class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle)
            : base(ownsHandle)
        {
            SetHandle(preexistingHandle);
        }


        [DllImport("kernel32")]
        private static extern bool CloseHandle(IntPtr handle);


        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }
    }
}