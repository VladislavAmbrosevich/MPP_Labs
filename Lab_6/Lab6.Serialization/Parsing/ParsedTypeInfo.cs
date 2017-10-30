using System.Collections.Generic;
using Lab6.Serialization.Parsing.ParsedTypeMembersDescriptions;

namespace Lab6.Serialization.Parsing
{
    public class ParsedTypeInfo
    {
        public string AssemblyQualifiedName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<SimpleTypeDescription> InheritedTypes { get; set; }

        public List<SimpleTypeDescription> ImplementedInterfaces { get; set; }

        public List<ParsedFieldDescription> Fields { get; set; }

        public List<ParsedMethodDescription> Methods { get; set; }


        public ParsedTypeInfo()
        {
            Fields = new List<ParsedFieldDescription>();
            Methods = new List<ParsedMethodDescription>();
            InheritedTypes = new List<SimpleTypeDescription>();
            ImplementedInterfaces = new List<SimpleTypeDescription>();
            Name = "";
            Namespace = "";
            AssemblyQualifiedName = "";
        }
    }
}