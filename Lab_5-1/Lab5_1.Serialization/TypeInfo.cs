using System.Collections.Generic;
using Lab5_1.Serialization.Interfaces;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization
{
    public class TypeInfo : ITypeInfo
    {
        public string Name { get; set; }

        public IReadOnlyCollection<string> InheritedTypes { get; }

        public IReadOnlyCollection<string> ImplementedInterfaces { get; }

        public IReadOnlyCollection<FieldDescription> Fields { get; }

        public IReadOnlyCollection<MethodDescription> Methods { get; }


        public TypeInfo()
        {
            Fields = new List<FieldDescription>();
            Methods = new List<MethodDescription>();
            InheritedTypes = new List<string>();
            ImplementedInterfaces = new List<string>();
            Name = "";
        }
    }
}