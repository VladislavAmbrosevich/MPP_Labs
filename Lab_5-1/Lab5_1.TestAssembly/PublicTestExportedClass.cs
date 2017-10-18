using System;
using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    public class PublicTestExportedClass
    {
        internal InternalTestExportedClass ReferenceTypeField;


        public int Property { get; set; }


        public event EventHandler Event;


        public delegate void TestDelegate();
    }
}