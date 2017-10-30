﻿using System;
using System.Collections.Generic;

namespace Lab6.Serialization.TypeMembersDescriptions
{
    public class DelegateDescription
    {
        public string Name { get; set; }

        public Type ReturnType { get; set; }

        public AccessModifiers AccessModifier { get; set; }

        public IReadOnlyCollection<ParameterInfo> Params { get; set; }
    }
}