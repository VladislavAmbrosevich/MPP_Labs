using System;
using System.Runtime.InteropServices;

namespace Lab4.OSHandling
{
    public class OsHandle : IDisposable
    {
        private bool _disposed;


        public IntPtr Handle { get; set; }

        ~OsHandle()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources.
                }
                // Release unmanaged resources.
                if (Handle != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(Handle);
                    Handle = IntPtr.Zero;
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}