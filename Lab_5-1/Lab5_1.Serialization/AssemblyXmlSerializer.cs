using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Lab5_1.Common;
using Lab5_1.Serialization.Interfaces;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization
{
    public static class AssemblyXmlSerializer
    {
        private const string SystemNamespace = "System";


        private static readonly List<Type> _referenceTypeFields = new List<Type>(); // Also need for props


        public static void SerializeAssemblyToFile(Assembly assembly, string filePath)
        {
            _referenceTypeFields.Clear();

            var typesInfo = GetAssemblyTypesInfo(assembly);
            GetReferenceTypeFields(typesInfo);

            var xmlDocument = ConstructAssemblyXmlDocument(typesInfo);
            xmlDocument.Save(filePath);
        }

        public static void SerializeType(Type type)
        {
            
        }


        private static void GetReferenceTypeFields(List<ITypeInfo> typesInfo)
        {
            foreach (var typeInfo in typesInfo)
            {
                foreach (var field in typeInfo.Fields)
                {
                    if (field.Type.Namespace != null && !field.Type.Namespace.StartsWith(SystemNamespace) && !_referenceTypeFields.Contains(field.Type))
                    {
                        _referenceTypeFields.Add(field.Type);
                    }
                }
            }
        }

        private static XDocument ConstructAssemblyXmlDocument(List<ITypeInfo> typesInfo)
        {
            var xmlDocument = new XDocument(
                new XElement(XmlNames.AssemblyTag,
                    from TypeInfo typeInfo in typesInfo
                    orderby typeInfo.Name
                    select ConstructClassXmlElement(typeInfo, new List<Type> {typeInfo.Type})));

            return xmlDocument;
        }

        private static XElement ConstructClassXmlElement(ITypeInfo typeInfo, List<Type> parentTypes)
        {
            var xElement =
                new XElement(XmlNames.ClassTag,
                    new XAttribute(XmlNames.NameAttribute, typeInfo.Name),

                    typeInfo.Fields.Count != 0
                        ? ConstructFieldsXmlElement(typeInfo.Fields, parentTypes)
                        : null//,

//                    typeInfo.Methods.Count != 0
//                        ? ConstructMethodsXmlElement(typeInfo.Methods)
//                        : null,

//                    typeInfo.ImplementedInterfaces.Count != 0
//                        ? ConstructImplementedInterfacesXmlElement(typeInfo.ImplementedInterfaces)
//                        : null,

//                    typeInfo.InheritedTypes.Count != 0
//                        ? ConstructInheritedTypesXmlElement(typeInfo.InheritedTypes)
//                        : null
                );

            return xElement;
        }

        private static XElement ConstructFieldsXmlElement(IList<FieldDescription> fieldsInfo, List<Type> parentTypes)
        {
//            foreach (var type in parentTypes)
//            {
//                Console.WriteLine(type.Name);
//            }
//            Console.WriteLine();
//            var newList = new List<Type>();
//            newList.AddRange(parentTypes);

            var simpleTypeFields =
                from FieldDescription field in fieldsInfo
                where !_referenceTypeFields.Contains(field.Type)
                select ConstructSimpleTypeFieldXmlElement(field);
            var complexTypeFields =
                from FieldDescription field in fieldsInfo
                where _referenceTypeFields.Contains(field.Type)
                select ConstructComplexTypeFieldXmlElement(field, parentTypes);

            var xElement =
                new XElement(XmlNames.FieldsTag, simpleTypeFields, complexTypeFields);

            return xElement;
        }

        private static XElement ConstructComplexTypeFieldXmlElement(FieldDescription fieldInfo, List<Type> parentTypes)
        {
            if (!parentTypes.Contains(fieldInfo.Type))
            {
                parentTypes.Add(fieldInfo.Type);
                var typeInfo = TypeParser.GetTypeInfo(fieldInfo.Type);
                var newList = new List<Type>();
                newList.AddRange(parentTypes);

                var xElement =
                    new XElement(XmlNames.FieldTag,
                        ConstructClassXmlElement(typeInfo, newList));
//                    new XAttribute(XmlNames.NameAttribute, fieldInfo.Name),
//                    new XAttribute(XmlNames.AccessModifierAttribute, fieldInfo.AccessModifier.GetString()),
//                    new XAttribute(XmlNames.TypeAttribute, fieldInfo.Type.Name));

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

        private static XElement ConstructImplementedInterfacesXmlElement(IList<Type> interfaces)
        {
            var xElement =
                new XElement(XmlNames.ImplementedInterfacesTag,
                    from Type implementedInterface in interfaces
                    select new XElement(XmlNames.InterfaceTag,
                        new XAttribute(XmlNames.NameAttribute, implementedInterface.Name)));

            return xElement;
        }

        private static XElement ConstructInheritedTypesXmlElement(IList<Type> types)
        {
            var xElement =
                new XElement(XmlNames.InheritedClassesTag,
                    from Type inheritedClass in types
                    select new XElement(XmlNames.InheritedClassTag,
                        new XAttribute(XmlNames.NameAttribute, inheritedClass.Name)));

            return xElement;
        }

        private static XElement ConstructMethodsXmlElement(IList<MethodDescription> methodsInfo)
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

        private static List<ITypeInfo> GetAssemblyTypesInfo(Assembly assembly)
        {
            var typesInfo = new List<ITypeInfo>();
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