using System;
using System.Collections.Generic;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization.Interfaces
{
    public interface ITypeInfo
    {
        string Name { get; set; }

        IList<Type> InheritedTypes { get; set; }

        IList<Type> ImplementedInterfaces { get; set; }

        IList<FieldDescription> Fields { get; set; }

        IList<MethodDescription> Methods{ get; set; }
    }
}