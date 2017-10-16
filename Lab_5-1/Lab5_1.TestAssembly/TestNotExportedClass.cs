using Lab5_1.Common;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    public class TestNotExportedClass
    {
        internal InternalTestExportedClass refField;
    }
}