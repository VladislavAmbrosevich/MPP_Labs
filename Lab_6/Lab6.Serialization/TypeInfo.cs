using System;
using System.Collections.Generic;
using Lab6.Serialization.TypeMembersDescriptions;

namespace Lab6.Serialization
{
    public class TypeInfo
    {
        public string AssemblyQualifiedName { get; set; }

        public string Name { get; set; }

        public string Namespace { get; set; }

        public List<Type> InheritedTypes { get; set; }

        public List<Type> ImplementedInterfaces { get; set; }

        public List<FieldDescription> Fields { get; set; }

        public List<MethodDescription> Methods { get; set; }


        public TypeInfo()
        {
            Fields = new List<FieldDescription>();
            Methods = new List<MethodDescription>();
            InheritedTypes = new List<Type>();
            ImplementedInterfaces = new List<Type>();
            Name = "";
            Namespace = "";
            AssemblyQualifiedName = "";
        }
    }
}