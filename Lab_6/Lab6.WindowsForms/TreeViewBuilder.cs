using System.Collections.Generic;
using System.Windows.Forms;
using Lab6.Serialization;
using Lab6.Serialization.Parsing;
using Lab6.Serialization.Parsing.ParsedTypeMembersDescriptions;

namespace Lab6.WindowsForms
{
    public static class TreeViewBuilder
    {
        public static List<TreeNode> BuildTreeViewFromAssembly(AssemblyInfo assemblyInfo)
        {
            var assemblyNode = new List<TreeNode>
            {
                new TreeNode($"<{XmlNames.AssemblyTag} {XmlNames.FullNameAttribute}=\"{assemblyInfo.FullName}\">", BuildClassNodesCollection(assemblyInfo).ToArray()),
                new TreeNode($"</{XmlNames.AssemblyTag}>")
            };

            return assemblyNode;
        }

        private static List<TreeNode> BuildClassNodesCollection(AssemblyInfo assemblyInfo)
        {
            var classNodes = new List<TreeNode>();

            foreach (var classInfo in assemblyInfo.ClassesList)
            {
                classNodes.AddRange(BuildClassNode(classInfo));
            }

            return classNodes;
        }

        private static List<TreeNode> BuildClassNode(ParsedTypeInfo classInfo)
        {
            var classNodes = new List<TreeNode>();
            var classNode = new TreeNode($"<{XmlNames.ClassTag} {XmlNames.NamespaceAttribute}=\"{classInfo.Namespace}\" {XmlNames.NameAttribute}=\"{classInfo.Name}\">");


            var fieldsNode = BuildFieldsTreeNode(classInfo.Fields);
            if (fieldsNode != null)
            {
                classNode.Nodes.AddRange(fieldsNode.ToArray());
            }

            var methodsNode = BuildMethodsTreeNode(classInfo.Methods);
            if (methodsNode != null)
            {
                classNode.Nodes.AddRange(methodsNode.ToArray());
            }

            var interfacesNode = BuildImplementedInterfaces(classInfo.ImplementedInterfaces);
            if (interfacesNode != null)
            {
                classNode.Nodes.AddRange(interfacesNode.ToArray());
            }

            var inheritedClassesNode = BuildInheritedClasses(classInfo.InheritedTypes);
            if (inheritedClassesNode != null)
            {
                classNode.Nodes.AddRange(inheritedClassesNode.ToArray());
            }

            classNodes.Add(classNode);
            classNodes.Add(new TreeNode($"</{XmlNames.ClassTag}>"));

            return classNodes;
        }

        private static List<TreeNode> BuildMethodsTreeNode(List<ParsedMethodDescription> methods)
        {
            if (methods.Count != 0)
            {
                var methodsNode = new List<TreeNode>();
                var methodsNodeOpen = new TreeNode($"<{XmlNames.MethodsTag}>");

                foreach (var methodInfo in methods)
                {
                    var methodNode = new TreeNode($"<{XmlNames.MethodTag} {XmlNames.NameAttribute}=\"{methodInfo.Name}\" {XmlNames.AccessModifierAttribute}=\"{methodInfo.AccessModifier}\" " +
                                                 $"{XmlNames.ReturnTypeAttribute}=\"{methodInfo.ReturnTypeName}\"");
                    if (methodInfo.Params.Count == 0)
                    {
                        methodNode.Text += @" />";
                        methodsNodeOpen.Nodes.Add(methodNode);
                    }
                    else
                    {
                        methodNode.Nodes.AddRange(BuildParameters(methodInfo.Params).ToArray());
                        methodNode.Text += @">";
                        methodsNodeOpen.Nodes.Add(methodNode);
                        methodsNodeOpen.Nodes.Add(new TreeNode($"</{XmlNames.MethodTag}>"));
                    }
                }
                methodsNode.Add(methodsNodeOpen);
                methodsNode.Add(new TreeNode($"</{XmlNames.MethodsTag}>"));

                return methodsNode;
            }

            return null;
        }

        private static List<TreeNode> BuildParameters(List<ParsedParameterInfo> parameters)
        {
            var parametersNode = new List<TreeNode>();
            var parametersNodeOpen = new TreeNode($"<{XmlNames.ParametersTag}>");
            foreach (var parameterInfo in parameters)
            {
                parametersNodeOpen.Nodes.Add(new TreeNode(
                    $"<{XmlNames.ParameterTag} {XmlNames.NameAttribute}=\"{parameterInfo.Name}\" {XmlNames.TypeAttribute}=\"{parameterInfo.TypeName}\" />"));
            }
            parametersNode.Add(parametersNodeOpen);
            parametersNode.Add(new TreeNode($"</{XmlNames.ParametersTag}>"));

            return parametersNode;
        }

        private static List<TreeNode> BuildFieldsTreeNode(List<ParsedFieldDescription> fields)
        {
            if (fields.Count != 0)
            {
                var fieldsNode = new List<TreeNode>();
                var fieldsNodeOpen = new TreeNode($"<{XmlNames.FieldsTag}>");

                foreach (var fieldInfo in fields)
                {
                    var fieldNode = new TreeNode($"<{XmlNames.FieldTag} {XmlNames.NameAttribute}=\"{fieldInfo.Name}\" {XmlNames.AccessModifierAttribute}=\"{fieldInfo.AccessModifier}\" " +
                                                 $"{XmlNames.TypeAttribute}=\"{fieldInfo.TypeName}\"");
                    if (fieldInfo.ClassType == null)
                    {
                        fieldNode.Text += @" />";
                        fieldsNodeOpen.Nodes.Add(fieldNode);
                    }
                    else
                    {
                        fieldNode.Nodes.AddRange(BuildClassNode(fieldInfo.ClassType).ToArray());
                        fieldNode.Text += @">";
                        fieldsNodeOpen.Nodes.Add(fieldNode);
                        fieldsNodeOpen.Nodes.Add(new TreeNode($"</{XmlNames.FieldTag}>"));
                    }
                }

                fieldsNode.Add(fieldsNodeOpen);
                fieldsNode.Add(new TreeNode($"</{XmlNames.FieldsTag}>"));

                return fieldsNode;
            }

            return null;
        }

        private static List<TreeNode> BuildImplementedInterfaces(List<SimpleTypeDescription> interfaces)
        {
            if (interfaces.Count != 0)
            {
                var interfacesNode = new List<TreeNode>();
                var interfacesNodeOpen = new TreeNode($"<{XmlNames.ImplementedInterfacesTag}>");
                foreach (var interfaceInfo in interfaces)
                {
                    interfacesNodeOpen.Nodes.Add(new TreeNode(
                        $"<{XmlNames.InterfaceTag} {XmlNames.NamespaceAttribute}=\"{interfaceInfo.Namespace}\" {XmlNames.NameAttribute}=\"{interfaceInfo.Name}\" />"));
                }
                interfacesNode.Add(interfacesNodeOpen);
                interfacesNode.Add(new TreeNode($"</{XmlNames.ImplementedInterfacesTag}>"));

                return interfacesNode;
            }

            return null;
        }

        private static List<TreeNode> BuildInheritedClasses(List<SimpleTypeDescription> inheritedClasses)
        {
            if (inheritedClasses.Count != 0)
            {
                var classesNode = new List<TreeNode>();
                var classesNodeOpen = new TreeNode($"<{XmlNames.InheritedClassesTag}>");
                foreach (var classInfo in inheritedClasses)
                {
                    classesNodeOpen.Nodes.Add(new TreeNode(
                        $"<{XmlNames.InheritedClassTag} {XmlNames.NamespaceAttribute}=\"{classInfo.Namespace}\" {XmlNames.NameAttribute}=\"{classInfo.Name}\" />"));
                }
                classesNode.Add(classesNodeOpen);
                classesNode.Add(new TreeNode($"</{XmlNames.InheritedClassesTag}>"));

                return classesNode;
            }

            return null;
        }
    }
}