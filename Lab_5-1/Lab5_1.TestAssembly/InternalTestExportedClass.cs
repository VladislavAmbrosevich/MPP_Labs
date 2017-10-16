using System;
using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    internal class InternalTestExportedClass
    {
        private int _testPrivateField = 10;

        internal int InternalField;

        protected int ProtectedField;

        public int PublicField;

        protected internal int ProtectedInternalField;

        public PublicTestExportedClass RefField;

        public InternalTestExportedClass RefFieldTwo;
//
//        public delegate void TestDelegateHandler();
//
//        public event TestDelegateHandler CustomEvent {
//            add { }
//            remove { }
//        }
//
//        private int Property { get; set; }
//
//        private InternalTestExportedClass(int a)
//        {
//            
//        }
//
//        public InternalTestExportedClass(int a, string b)
//        {
//
//        }

        private int PrivateMethod(int a)
        {
            return ++a;
        }

        public new virtual string ToString()
        {
            return "f";
        }
    }
}