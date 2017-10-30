using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Lab6.Common;
using Lab6.Serialization.Parsing;
using Lab6.Serialization.Parsing.ParsedTypeMembersDescriptions;
using Lab6.Serialization.TypeMembersDescriptions;

namespace Lab6.Serialization
{
    public static class AssemblyXmlSerializer
    {
        private const string SystemNamespace = "System";


        private static readonly List<Type> ReferenceTypeFields = new List<Type>();


        public static void SerializeAssemblyToFile(Assembly assembly, string filePath)
        {
            ReferenceTypeFields.Clear();

            var typesInfo = GetAssemblyTypesInfo(assembly);
            GetReferenceTypeFields(typesInfo);

            var xmlDocument = ConstructAssemblyXmlDocument(assembly, typesInfo);
            xmlDocument.Save(filePath);
        }

        public static AssemblyInfo ParseFromXmlFile(string fileName)
        {
            var assemblyInfo = new AssemblyInfo();
            var xDoc = XDocument.Load(fileName);

            var assemblyElement = xDoc.Element(XmlNames.AssemblyTag);
            assemblyInfo.ClassesList = ParseClassesFromAssemblyElement(assemblyElement);

            return assemblyInfo;
        }


        private static List<ParsedTypeInfo> ParseClassesFromAssemblyElement(XElement assemblyElement)
        {
            var classes = from classElement in assemblyElement.Elements(XmlNames.ClassTag)
                select ParseClass(classElement);

            return classes.ToList();
        }

        private static ParsedTypeInfo ParseClass(XElement classElement)
        {
            if (classElement != null)
            {
                var parsedClass = new ParsedTypeInfo
                {
                    AssemblyQualifiedName = String.Empty,
                    Namespace = classElement.Attribute(XmlNames.NamespaceAttribute)?.Value,
                    Name = classElement.Attribute(XmlNames.NameAttribute)?.Value,
                    Fields = ParseFields(classElement.Element(XmlNames.FieldsTag)),
                    ImplementedInterfaces = ParseImplementedInterfaces(classElement.Element(XmlNames.ImplementedInterfacesTag)),
                    InheritedTypes = ParseInheritedClasses(classElement.Element(XmlNames.InheritedClassesTag)),
                    Methods = ParseMethods(classElement.Element(XmlNames.MethodsTag))
                };

                return parsedClass;
            }

            return null;
        }

        private static List<SimpleTypeDescription> ParseImplementedInterfaces(XElement interfacesElement)
        {
            if (interfacesElement != null)
            {
                var interfaces = from interfaceElement in interfacesElement.Elements(XmlNames.InterfaceTag)
                    select new SimpleTypeDescription
                    {
                        Namespace = interfaceElement.Attribute(XmlNames.NamespaceAttribute)?.Value,
                        Name = interfaceElement.Attribute(XmlNames.NameAttribute)?.Value
                    };

                return interfaces.ToList();
            }

            return new List<SimpleTypeDescription>();
        }

        private static List<SimpleTypeDescription> ParseInheritedClasses(XElement inheritedClassesElement)
        {
            if (inheritedClassesElement != null)
            {
                var inheritedClasses = from classElement in inheritedClassesElement.Elements(XmlNames.InheritedClassTag)
                    select new SimpleTypeDescription
                    {
                        Namespace = classElement.Attribute(XmlNames.NamespaceAttribute)?.Value,
                        Name = classElement.Attribute(XmlNames.NameAttribute)?.Value
                    };

                return inheritedClasses.ToList();
            }

            return new List<SimpleTypeDescription>();
        }

        private static List<ParsedMethodDescription> ParseMethods(XElement methodsElement)
        {
            if (methodsElement != null)
            {
                var methods = from methodElement in methodsElement.Elements(XmlNames.MethodTag)
                    select new ParsedMethodDescription
                    {
                        Name = methodElement.Attribute(XmlNames.NameAttribute)?.Value,
                        AccessModifier = methodElement.Attribute(XmlNames.AccessModifierAttribute)?.Value,
                        ReturnTypeName = methodElement.Attribute(XmlNames.ReturnTypeAttribute)?.Value,
                        Params = ParseParameters(methodElement.Element(XmlNames.ParametersTag))
                    };

                return methods.ToList();
            }

            return new List<ParsedMethodDescription>();
        }

        private static List<ParsedParameterInfo> ParseParameters(XElement parametersElement)
        {
            if (parametersElement != null)
            {
                var parameters = from parameterElement in parametersElement.Elements(XmlNames.ParameterTag)
                    select new ParsedParameterInfo
                    {
                        Name = parameterElement.Attribute(XmlNames.NameAttribute)?.Value,
                        TypeName = parameterElement.Attribute(XmlNames.TypeAttribute)?.Value
                    };

                return parameters.ToList();
            }

            return new List<ParsedParameterInfo>();
        }

        private static List<ParsedFieldDescription> ParseFields(XElement fieldsElement)
        {
            var fields = new List<ParsedFieldDescription>();
            if (fieldsElement != null)
            {
                var fieldsOfSimpleType = from fieldElement in fieldsElement.Elements(XmlNames.FieldTag)
                    where fieldElement.Element(XmlNames.ClassTag) == null
                    select new ParsedFieldDescription
                    {
                        AccessModifier = fieldElement.Attribute(XmlNames.AccessModifierAttribute)?.Value,
                        Name = fieldElement.Attribute(XmlNames.NameAttribute)?.Value,
                        TypeName = fieldElement.Attribute(XmlNames.TypeAttribute)?.Value,
                        ClassType = null
                    };
                fields.AddRange(fieldsOfSimpleType);

                var fieldsOfClassType = from fieldElement in fieldsElement.Elements(XmlNames.FieldTag)
                    where fieldElement.Element(XmlNames.ClassTag) != null
                    select new ParsedFieldDescription
                    {
                        AccessModifier = fieldElement.Attribute(XmlNames.AccessModifierAttribute)?.Value,
                        Name = fieldElement.Attribute(XmlNames.NameAttribute)?.Value,
                        TypeName = fieldElement.Attribute(XmlNames.TypeAttribute)?.Value,
                        ClassType = ParseClass(fieldElement.Element(XmlNames.ClassTag))
                    };
                fields.AddRange(fieldsOfClassType);

                return fields.ToList();
            }

            return new List<ParsedFieldDescription>();
        }

        private static void GetReferenceTypeFields(List<TypeInfo> typesInfo)
        {
            foreach (var typeInfo in typesInfo)
            {
                foreach (var field in typeInfo.Fields)
                {
                    if (field.Type.Namespace != null && !field.Type.Namespace.StartsWith(SystemNamespace) && !ReferenceTypeFields.Contains(field.Type))
                    {
                        ReferenceTypeFields.Add(field.Type);
                    }
                }
            }
        }

        private static XDocument ConstructAssemblyXmlDocument(Assembly assembly, List<TypeInfo> typesInfo)
        {
            var xmlDocument = new XDocument(
                new XElement(XmlNames.AssemblyTag,
                    new XAttribute(XmlNames.FullNameAttribute, assembly.FullName),
                    from TypeInfo typeInfo in typesInfo
                    orderby typeInfo.Namespace, typeInfo.Name
                    select ConstructClassXmlElement(typeInfo, new List<string> {typeInfo.AssemblyQualifiedName})));

            return xmlDocument;
        }

        private static XElement ConstructClassXmlElement(TypeInfo typeInfo, List<string> parentTypes)
        {
            var xElement =
                new XElement(XmlNames.ClassTag,
                    new XAttribute(XmlNames.NamespaceAttribute, typeInfo.Namespace),
                    new XAttribute(XmlNames.NameAttribute, typeInfo.Name),

                    typeInfo.Fields.Count != 0
                        ? ConstructFieldsXmlElement(typeInfo.Fields, parentTypes)
                        : null,

                    typeInfo.Methods.Count != 0
                        ? ConstructMethodsXmlElement(typeInfo.Methods)
                        : null,

                    typeInfo.ImplementedInterfaces.Count != 0
                        ? ConstructImplementedInterfacesXmlElement(typeInfo.ImplementedInterfaces)
                        : null,

                    typeInfo.InheritedTypes.Count != 0
                        ? ConstructInheritedTypesXmlElement(typeInfo.InheritedTypes)
                        : null
                );

            return xElement;
        }

        private static XElement ConstructFieldsXmlElement(List<FieldDescription> fieldsInfo, List<string> parentTypes)
        {
            var simpleTypeFields =
                from FieldDescription field in fieldsInfo
                where !ReferenceTypeFields.Contains(field.Type)
                select ConstructSimpleTypeFieldXmlElement(field);
            var complexTypeFields =
                from FieldDescription field in fieldsInfo
                where ReferenceTypeFields.Contains(field.Type)
                select ConstructComplexTypeFieldXmlElement(field, parentTypes);

            var xElement =
                new XElement(XmlNames.FieldsTag, simpleTypeFields, complexTypeFields);

            return xElement;
        }

        private static XElement ConstructComplexTypeFieldXmlElement(FieldDescription fieldInfo, List<string> parentTypes)
        {
            if (!parentTypes.Contains(fieldInfo.Type.AssemblyQualifiedName))
            {
                parentTypes.Add(fieldInfo.Type.AssemblyQualifiedName);
                var typeInfo = TypeParser.GetTypeInfo(fieldInfo.Type);

                var xElement =
                    new XElement(XmlNames.FieldTag,
                        new XAttribute(XmlNames.NameAttribute, fieldInfo.Name),
                        new XAttribute(XmlNames.AccessModifierAttribute, fieldInfo.AccessModifier.GetString()),
                        new XAttribute(XmlNames.TypeAttribute, fieldInfo.Type.Name),
                        ConstructClassXmlElement(typeInfo, parentTypes));

                return xElement;
            }
            else
            {
                var xElement =
                    new XElement(XmlNames.FieldTag, new XAttribute(XmlNames.NameAttribute, fieldInfo.Name),
                        new XAttribute(XmlNames.AccessModifierAttribute, fieldInfo.AccessModifier.GetString()),
                        new XAttribute(XmlNames.TypeAttribute, fieldInfo.Type.Name));

                return xElement;
            }
        }

        private static XElement ConstructSimpleTypeFieldXmlElement(FieldDescription fieldInfo)
        {
            var xElement =
                new XElement(XmlNames.FieldTag,
                    new XAttribute(XmlNames.NameAttribute, fieldInfo.Name),
                    new XAttribute(XmlNames.AccessModifierAttribute, fieldInfo.AccessModifier.GetString()),
                    new XAttribute(XmlNames.TypeAttribute, fieldInfo.Type.Name));

            return xElement;
        }

        private static XElement ConstructImplementedInterfacesXmlElement(List<Type> interfaces)
        {
            var xElement =
                new XElement(XmlNames.ImplementedInterfacesTag,
                    from Type implementedInterface in interfaces
                    select new XElement(XmlNames.InterfaceTag,
                        new XAttribute(XmlNames.NamespaceAttribute, implementedInterface.Namespace),
                        new XAttribute(XmlNames.NameAttribute, implementedInterface.Name)));

            return xElement;
        }

        private static XElement ConstructInheritedTypesXmlElement(List<Type> types)
        {
            var xElement =
                new XElement(XmlNames.InheritedClassesTag,
                    from Type inheritedClass in types
                    select new XElement(XmlNames.InheritedClassTag,
                        new XAttribute(XmlNames.NamespaceAttribute, inheritedClass.Namespace),
                        new XAttribute(XmlNames.NameAttribute, inheritedClass.Name)));

            return xElement;
        }

        private static XElement ConstructMethodsXmlElement(List<MethodDescription> methodsInfo)
        {
            var xElement =
                    new XElement(XmlNames.MethodsTag,
                        from MethodDescription method in methodsInfo
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
                                : null));

            return xElement;
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