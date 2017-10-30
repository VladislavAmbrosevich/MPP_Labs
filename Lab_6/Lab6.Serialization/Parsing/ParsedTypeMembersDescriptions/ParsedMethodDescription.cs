using System.Collections.Generic;

namespace Lab6.Serialization.Parsing.ParsedTypeMembersDescriptions
{
    public class ParsedMethodDescription
    {
        public string Name { get; set; }

        public string ReturnTypeName { get; set; }

        public string AccessModifier { get; set; }

        public List<ParsedParameterInfo> Params { get; set; }


        public ParsedMethodDescription()
        {
            Params = new List<ParsedParameterInfo>();
        }
    }
}