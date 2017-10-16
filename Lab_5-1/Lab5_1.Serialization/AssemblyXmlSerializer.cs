using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization
{
    public static class AssemblyXmlSerializer
    {
        public static void SerializeAssemblyToFile(Assembly assembly, string filePath)
        {
            var typesInfo = GetAssemblyTypesInfo(assembly); 

            var xmlDocument = new XDocument(
                new XElement(XmlNames.AssemblyTag,
                    from TypeInfo typeInfo in typesInfo
                    orderby typeInfo.Name descending
                    select new XElement(XmlNames.ClassTag,
                        from FieldDescription field in typeInfo.Fields
                        select new XElement(XmlNames.FieldTag,
                            new XAttribute(XmlNames.NameAttribute, field.Name),
                            new XAttribute(XmlNames.AccessModifierAttribute, field.AccessModifier.GetString()),
                            new XAttribute(XmlNames.TypeAttribute, field.Type.Name)),
                        new XAttribute(XmlNames.NameAttribute, typeInfo.Name)

                    )
                ));

            xmlDocument.Save(filePath);
        }

        public static void SerializeType(Type type)
        {
            
        }


        private static List<TypeInfo> GetAssemblyTypesInfo(Assembly assembly)
        {
            var typesInfo = new List<TypeInfo>();
            foreach (var type in assembly.GetTypes())
            {
                var typeInfo = TypeParser.GetTypeInfo(type);
                typesInfo.Add(typeInfo);
            }

            return typesInfo;
        }
    }
}