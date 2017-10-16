using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Lab5_1.Common;
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
                        new XAttribute(XmlNames.NameAttribute, typeInfo.Name),

                        typeInfo.Fields.Count != 0
                            ? new XElement(XmlNames.FieldsTag, 
                                from FieldDescription field in typeInfo.Fields
                                select new XElement(XmlNames.FieldTag,
                                    new XAttribute(XmlNames.NameAttribute, field.Name),
                                    new XAttribute(XmlNames.AccessModifierAttribute, field.AccessModifier.GetString()),
                                    new XAttribute(XmlNames.TypeAttribute, field.Type.Name)))
                            : null,

                        typeInfo.Methods.Count != 0
                            ? new XElement(XmlNames.MethodsTag,
                                from MethodDescription method in typeInfo.Methods
                                select new XElement(XmlNames.MethodTag,
                                    new XAttribute(XmlNames.NameAttribute, method.Name),
                                    new XAttribute(XmlNames.AccessModifierAttribute, method.AccessModifier.GetString()),
                                    new XAttribute(XmlNames.ReturnTypeAttribute, method.ReturnType.Name),
                                    method.Params.Count != 0
                                        ? new XElement(XmlNames.ParametersTag,
                                            from ParameterInfo parameter in method.Params
                                            select new XElement(XmlNames.ParameterTag,
                                                new XAttribute(XmlNames.NameAttribute, parameter.Name),
                                                new XAttribute(XmlNames.TypeAttribute, parameter.Type.Name)))
                                        : null ))
                            : null,

                        typeInfo.ImplementedInterfaces.Count != 0
                            ? new XElement(XmlNames.ImplementedInterfacesTag,
                                from Type implementedInterface in typeInfo.ImplementedInterfaces
                                select new XElement(XmlNames.InterfaceTag,
                                    new XAttribute(XmlNames.NameAttribute, implementedInterface.Name))) 
                            : null,

                        typeInfo.InheritedTypes.Count != 0
                            ? new XElement(XmlNames.InheritedClassesTag,
                                from Type inheritedClass in typeInfo.InheritedTypes
                                select new XElement(XmlNames.InheritedClassTag,
                                    new XAttribute(XmlNames.NameAttribute, inheritedClass.Name)))
                            : null

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
            var types = assembly.GetTypes().Where(type => type.IsDefined(typeof(ExportXmlAttribute), false)).ToList();
            foreach (var type in types)
            {
                var typeInfo = TypeParser.GetTypeInfo(type);
                typesInfo.Add(typeInfo);
            }

            return typesInfo;
        }
    }
}