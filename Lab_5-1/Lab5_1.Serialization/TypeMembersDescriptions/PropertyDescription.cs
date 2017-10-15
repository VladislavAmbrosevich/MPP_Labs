using System;

namespace Lab5_1.Serialization.TypeMembersDescriptions
{
    public class PropertyDescription
    {
        public Type Type { get; }

        public string Name { get; }

        public string AccessModifier { get; }

        public string GetterAccessModifier { get; }

        public string SetterAccessModifier { get; }
    }
}