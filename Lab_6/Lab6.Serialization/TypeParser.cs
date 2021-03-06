﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lab6.Serialization.TypeMembersDescriptions;

namespace Lab6.Serialization
{
    public static class TypeParser
    {
        private const BindingFlags BindingFlagsSet = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;


        public static TypeInfo GetTypeInfo(Type type)
        {
            var typeInfo = new TypeInfo
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                Name = type.Name,
                Namespace = type.Namespace,
                ImplementedInterfaces = type.GetInterfaces().ToList(),
                InheritedTypes = GetInheritedTypes(type),
                Fields = GetFields(type),
                Methods = GetMethods(type)
            };

            return typeInfo;
        }


        private static List<Type> GetInheritedTypes(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            var inheritedTypes = assembly.GetTypes().Where(asmType => asmType.IsSubclassOf(type)).ToList();

            return inheritedTypes;
        }

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