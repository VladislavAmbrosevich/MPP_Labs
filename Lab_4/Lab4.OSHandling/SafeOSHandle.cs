using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.OSHandling
{
    public class SafeOsHandle : SafeHandle
    {
        public SafeOsHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle)
        {
        }

        protected override bool ReleaseHandle()
        {
            throw new NotImplementedException();
        }

        public override bool IsInvalid { get; }
    }
}
