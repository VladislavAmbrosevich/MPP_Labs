using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab6.Serialization;
using Lab6.Serialization.Parsing;

namespace Lab6.WindowsForms
{
    public partial class Form1 : Form
    {
        private const string FilePath = @"D:\БГУИР\СПП\5сем\MPP_Labs\Lab_6\Lab6.TestConsoleApp\bin\Debug\Lab5_1.TestAssembly.dll.xml";


        public Form1()
        {
            InitializeComponent();

            var assemblyInfo = AssemblyXmlSerializer.ParseFromXmlFile(FilePath);

            var assemblyNode = TreeViewBuilder.BuildTreeViewFromAssembly(assemblyInfo);
            LoadTreeViewToVisualElement(assemblyNode);
        }


        private void LoadTreeViewToVisualElement(List<TreeNode> treeViewNode)
        {
            treeView1.Nodes.AddRange(treeViewNode.ToArray());
        }
    }
}