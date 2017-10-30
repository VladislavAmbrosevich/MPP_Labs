namespace Lab6.Serialization.Parsing.ParsedTypeMembersDescriptions
{
    public class ParsedFieldDescription
    {
        public string TypeName { get; set; }

        public string Name { get; set; }

        public string AccessModifier { get; set; }

        public ParsedTypeInfo ClassType { get; set; }
    }
}