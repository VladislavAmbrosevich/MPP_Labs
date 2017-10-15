using System.Collections.Generic;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization.Interfaces
{
    public interface ITypeInfo
    {
        string Name { get; set; }

        IReadOnlyCollection<string> InheritedTypes { get; }

        IReadOnlyCollection<string> ImplementedInterfaces { get; }

        IReadOnlyCollection<FieldDescription> Fields { get; }

        IReadOnlyCollection<MethodDescription> Methods{ get; }
    }
}