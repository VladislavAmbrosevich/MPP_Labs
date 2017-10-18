using Lab5_1.Common;
using Lab5_1.TestAssembly.Interfaces;

namespace Lab5_1.TestAssembly
{
    [ExportXml]
    internal class InheritedTestExportedClass : PublicTestExportedClass, IPublicTestInterface, IInternalTestInterface
    {
        public TestNotExportedClass ReferenceNotExportedTypeField;
    }
}