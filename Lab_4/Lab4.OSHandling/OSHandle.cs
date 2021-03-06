﻿using System;

namespace Lab4.OSHandling
{
    public abstract class OsHandle<T>: IDisposable where T : class
    {
        private bool _disposed;


        protected OsHandle()
        {
            _disposed = false;
        }


        ~OsHandle()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected abstract void ReleaseHandle();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                ReleaseHandle();
                _disposed = true;
            }
        }
    }
}