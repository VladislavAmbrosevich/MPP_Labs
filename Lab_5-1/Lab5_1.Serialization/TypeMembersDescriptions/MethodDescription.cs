using System;
using System.Collections.Generic;

namespace Lab5_1.Serialization.TypeMembersDescriptions
{
    public class MethodDescription
    {
        public string Name { get; }

        public Type ReturnType { get; }

        public string AccessModifier { get; }

        public IReadOnlyCollection<ParameterInfo> Params { get; } = new List<ParameterInfo>();
    }
}