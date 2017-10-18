using System;
using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    internal class InternalTestExportedClass
    {
        private int PrivateField;

        internal int InternalField;

        protected int ProtectedField;

        public int PublicField;

        protected internal int ProtectedInternalField;


        private int PrivateMethod(int a, string b)
        {
            return ++a;
        }

        public void PublicMethod(PublicTestExportedClass referenceTypeParameter)
        {
            return;
        }

        public new virtual string ToString()
        {
            return String.Empty;
        }
    }
}