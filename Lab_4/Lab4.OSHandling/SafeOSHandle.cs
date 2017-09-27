using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace Lab4.OSHandling
{
    public abstract class SafeOsHandle : CriticalFinalizerObject, IDisposable
    {
        private readonly int _state;
        private bool _ownsHandle;
        private bool _fullyInitialized;

        protected IntPtr Handle;


        public bool IsClosed => (_state & 1) == 1;

        public abstract bool IsInvalid { get; }


        protected SafeOsHandle(IntPtr invalidHandleValue, bool ownsHandle)
        {
            Handle = invalidHandleValue;
            _state = 4;
            _ownsHandle = ownsHandle;
            if (!ownsHandle)
            {
                GC.SuppressFinalize(this);
            }
            _fullyInitialized = true;
        }


        ~SafeOsHandle()
        {
            Dispose(false);
        }


        public IntPtr DangerousGetHandle()
        {
            return Handle;
        }

        public void Close()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetHandleAsInvalid();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void DangerousAddRef(ref bool success);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void DangerousRelease();


        protected void SetHandle(IntPtr handle)
        {
            Handle = handle;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InternalDispose();
            }
            else
            {
                InternalFinalize();
            }
        }


        protected abstract bool ReleaseHandle();


        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void InternalFinalize();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void InternalDispose();
    }
}