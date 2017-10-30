using System;
using System.Collections.Generic;

namespace Lab6.Serialization.TypeMembersDescriptions
{
    public class MethodDescription
    {
        public string Name { get; set; }

        public Type ReturnType { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public IList<ParameterInfo> Params { get; set; }


        public MethodDescription()
        {
            Params = new List<ParameterInfo>();
        }
    }
}