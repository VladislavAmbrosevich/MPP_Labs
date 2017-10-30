using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab6.WindowsForms
{
    public static class XmlHandler
    {
        private const string OneTab = "  ";
        private const string XmlFileDeclaration = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>";


        public static void SaveTreeViewToXmlFile(TreeView treeView, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine(XmlFileDeclaration);
            RecursiveSaveNode(sb, "", treeView.Nodes);

            File.WriteAllText(filePath, sb.ToString());
        }


        private static void RecursiveSaveNode(StringBuilder sb, string tabs, TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode treeNode in treeNodeCollection)
            {
                sb.AppendLine(tabs + treeNode.Text);
                if (treeNode.Nodes.Count > 0)
                {
                    RecursiveSaveNode(sb, tabs + OneTab, treeNode.Nodes);
                }
            }
        }
    }
}