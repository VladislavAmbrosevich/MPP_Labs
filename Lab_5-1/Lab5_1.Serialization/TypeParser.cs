using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lab5_1.Serialization.Interfaces;
using Lab5_1.Serialization.TypeMembersDescriptions;

namespace Lab5_1.Serialization
{
    public static class TypeParser
    {
        private const BindingFlags BindingFlagsSet = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;


        public static ITypeInfo GetTypeInfo(Type type)
        {
            var typeInfo = new TypeInfo();
            typeInfo.Type = type;
            typeInfo.Name = type.FullName;
            typeInfo.ImplementedInterfaces = type.GetInterfaces().ToList();
            typeInfo.InheritedTypes = GetInheritedTypes(type);

            typeInfo.Fields = GetFields(type);
            typeInfo.Methods = GetMethods(type);

            return typeInfo;
        }


        private static List<Type> GetInheritedTypes(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            var inheritedTypes = assembly.GetTypes().Where(asmType => asmType.IsSubclassOf(type)).ToList();

            return inheritedTypes;
        }

        // Need to remove fields if they are not fields (custom events)
        private static List<FieldDescription> GetFields(Type type)
        {
            var fieldsList = new List<FieldDescription>();
            var fields = type.GetFields(BindingFlagsSet);
            foreach (var field in fields)
            {
                var fieldDescription = new FieldDescription
                {
                    Name = field.Name,
                    AccessModifier = GetAccessModifier(field),
                    Type = field.FieldType
                };
                fieldsList.Add(fieldDescription);
            }

            return fieldsList;
        }

        // Need to remove methods for getters, setters, add, remove
        private static List<MethodDescription> GetMethods(Type type)
        {
            var methodsList = new List<MethodDescription>();
            var methods = type.GetMethods(BindingFlagsSet);
            foreach (var method in methods)
            {
                var methodDescription = new MethodDescription
                {
                    Name = method.Name,
                    AccessModifier = GetAccessModifier(method),
                    ReturnType = method.ReturnType
                };
                foreach (var parameter in method.GetParameters())
                {
                    var paramInfo = new ParameterInfo
                    {
                        Name = parameter.Name,
                        Type = parameter.ParameterType
                    };
                    methodDescription.Params.Add(paramInfo);
                }

                methodsList.Add(methodDescription);
            }

            return methodsList;
        }

        private static AccessModifiers GetAccessModifier(dynamic memberInfo)
        {
            if (memberInfo.IsPublic)
            {
                return AccessModifiers.Public;
            }
            if (memberInfo.IsAssembly)
            {
                return AccessModifiers.Internal;
            }
            if (memberInfo.IsFamilyOrAssembly)
            {
                return AccessModifiers.ProtectedInternal;
            }
            if (memberInfo.IsFamily)
            {
                return AccessModifiers.Protected;
            }
            if (memberInfo.IsPrivate)
            {
                return AccessModifiers.Private;
            }

            throw new Exception(nameof(memberInfo));
        }
    }
}