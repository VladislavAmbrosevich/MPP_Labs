using System;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace Lab4.OSHandling
{
    public abstract class SafeOsHandle : CriticalFinalizerObject, IDisposable
    {
        private const int RefCountMask = 0x7ffffffc;
        private const int RefCountOne = 0x4;

        private readonly bool _ownsHandle;
        private readonly bool _fullyInitialized;

        private int _state;

        protected IntPtr Handle;


        private enum State
        {
            Closed = 0x00000001,
            Disposed = 0x00000002,
        }


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

        public void SetHandleAsInvalid()
        {
            try
            {

            }
            finally
            {
                int oldState, newState;

                do
                {
                    oldState = _state;
                    newState = oldState | (int) State.Closed;
                } while (Interlocked.CompareExchange(ref _state, newState, oldState) != oldState);

                GC.SuppressFinalize(this);
            }
        }

        public void DangerousAddRef(ref bool success)
        {
            try
            {

            }
            finally
            {
                if (!_fullyInitialized)
                {
                    throw new InvalidOperationException();
                }

                int oldState, newState;

                do
                {
                    oldState = _state;

                    if ((oldState & (int) State.Closed) != 0)
                    {
                        throw new ObjectDisposedException("handle");
                    }

                    newState = oldState + RefCountOne;
                } while (Interlocked.CompareExchange(ref _state, newState, oldState) != oldState);

                success = true;
            }
        }

        public void DangerousRelease()
        {
            DangerousReleaseInternal(false);
        }

        public void Close()
        {
            Dispose(true);
        }

        public void Dispose()
        {
            Dispose(true);
        }


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


        private void InternalDispose()
        {
            if (!_fullyInitialized)
            {
                throw new InvalidOperationException();
            }

            DangerousReleaseInternal(true);
            GC.SuppressFinalize(this);
        }

        private void InternalFinalize()
        {
            if (_fullyInitialized)
            {
                DangerousReleaseInternal(true);
            }
        }

        private void DangerousReleaseInternal(bool dispose)
        {
            try
            {

            }
            finally
            {
                if (!_fullyInitialized)
                {
                    throw new InvalidOperationException();
                }

                int oldState, newState;
                bool performRelease;

                do
                {
                    oldState = _state;

                    if (dispose && (oldState & (int) State.Disposed) != 0)
                    {
                        performRelease = false;
                        break;
                    }

                    if ((oldState & RefCountMask) == 0)
                    {
                        throw new ObjectDisposedException("handle");
                    }

                    if ((oldState & RefCountMask) != RefCountOne)
                    {
                        performRelease = false;
                    }
                    else if ((oldState & (int) State.Closed) != 0)
                    {
                        performRelease = false;
                    }
                    else if (!_ownsHandle)
                    {
                        performRelease = false;
                    }
                    else if (IsInvalid)
                    {
                        performRelease = false;
                    }
                    else
                    {
                        performRelease = true;
                    }

                    newState = (oldState & RefCountMask) - RefCountOne;
                    if ((oldState & RefCountMask) == RefCountOne)
                    {
                        newState |= (int) State.Closed;
                    }
                    if (dispose)
                    {
                        newState |= (int) State.Disposed;
                    }
                } while (Interlocked.CompareExchange(ref _state, newState, oldState) != oldState);

                if (performRelease)
                {
                    ReleaseHandle();
                }
            }
        }
    }
}