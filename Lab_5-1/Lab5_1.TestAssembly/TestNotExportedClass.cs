using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    internal class TestNotExportedClass : InternalTestExportedClass
    {
        internal InternalTestExportedClass refField;
    }
}