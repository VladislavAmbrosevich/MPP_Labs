using System;

namespace Lab6.Serialization.TypeMembersDescriptions
{
    public class PropertyDescription
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public string GetterAccessModifier { get; set; }

        public string SetterAccessModifier { get; set; }
    }
}