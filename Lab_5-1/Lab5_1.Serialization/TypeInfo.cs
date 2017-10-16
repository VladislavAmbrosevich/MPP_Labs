using System;
using System.Collections.Generic;
using Lab5_1.Serialization.Interfaces;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization
{
    public class TypeInfo : ITypeInfo
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public IList<Type> InheritedTypes { get; set; }

        public IList<Type> ImplementedInterfaces { get; set; }

        public IList<FieldDescription> Fields { get; set; }

        public IList<MethodDescription> Methods { get; set; }


        public TypeInfo()
        {
            Fields = new List<FieldDescription>();
            Methods = new List<MethodDescription>();
            InheritedTypes = new List<Type>();
            ImplementedInterfaces = new List<Type>();
            Name = "";
        }
    }
}