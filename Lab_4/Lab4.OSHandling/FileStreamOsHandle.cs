﻿using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Lab4.OSHandling
{
    public class FileStreamOsHandle : OsHandle<FileStream>
    {
        private bool _disposed;


        public delegate FileStream FileStreamReceiveDelegate();


        public sealed override FileStream ManagedObject { get; protected set; }
        public sealed override IntPtr Handle { get; protected set; }


        public FileStreamOsHandle(FileStreamReceiveDelegate expression)
        {
            _disposed = false;
            ManagedObject = expression();
            Handle = ManagedObject.Handle;
        }


        protected override void ReleaseHandle()
        {
            Marshal.FreeCoTaskMem(Handle);
            base.ReleaseHandle();
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Release managed resources.
                    ManagedObject.Dispose();
                    Handle = IntPtr.Zero;
                }
                // Release unmanaged resources.
                ReleaseHandle();
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}