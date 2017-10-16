using System;

namespace Lab5_1.Serialization.TypeMembersDescriptions
{
    public class EventDescription
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public AccessModifiers AccessModifier { get; set; }
    }
}