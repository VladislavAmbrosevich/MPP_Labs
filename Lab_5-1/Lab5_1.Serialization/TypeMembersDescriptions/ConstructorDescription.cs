using System.Collections.Generic;

namespace Lab5_1.Serialization.TypeMembersDescriptions
{
    public class ConstructorDescription
    {
        public string Name { get; }

        public AccessModifiers AccessModifier { get; }

        public IReadOnlyCollection<ParameterInfo> Params { get; } = new List<ParameterInfo>();
    }
}