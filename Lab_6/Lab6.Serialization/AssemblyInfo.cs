using System.Collections.Generic;
using Lab6.Serialization.Parsing;

namespace Lab6.Serialization
{
    public class AssemblyInfo
    {
        public List<ParsedTypeInfo> ClassesList { get; set; }

        public string FullName { get; set; }


        public AssemblyInfo()
        {
            ClassesList = new List<ParsedTypeInfo>();
            FullName = "";
        }
    }
}