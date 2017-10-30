using System.Collections.Generic;

namespace Lab6.Serialization.TypeMembersDescriptions
{
    public class ConstructorDescription
    {
        public string Name { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public IReadOnlyCollection<ParameterInfo> Params { get; set; }
    }
}