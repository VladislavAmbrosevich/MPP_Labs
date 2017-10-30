using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    public class PublicTestExportedClass
    {
        internal InternalTestExportedClass ReferenceTypeField;


        public int PublicField;
    }
}