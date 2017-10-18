using System.Collections.Generic;

namespace Lab5_1.Serialization.TypeMembersDescriptions
{
    public class ConstructorDescription
    {
        public string Name { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public IReadOnlyCollection<ParameterInfo> Params { get; set; }
    }
}