using System.Collections.Generic;
using System.Windows.Forms;
using Lab6.Serialization;

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

            XmlHandler.SaveTreeViewToXmlFile(trvwAssemblyInfo, @"C:\Users\vambr\Desktop\TreeView.xml");
        }


        private void LoadTreeViewToVisualElement(List<TreeNode> treeViewNode)
        {
            trvwAssemblyInfo.Nodes.AddRange(treeViewNode.ToArray());
        }

        private void trvwAssemblyInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnNodeSelected(e.Node);
        }

        private void btnEditNode_Click(object sender, System.EventArgs e)
        {
            OnEditNodeStarted();
        }

        private void OnNodeSelected(TreeNode selectedNode)
        {
            if (AttributeHandler.CheckIfHasAttributes(selectedNode.Text))
            {
                btnEditNode.Enabled = true;
            }
            else
            {
                btnEditNode.Enabled = false;
            }
        }

        private void OnEditNodeStarted()
        {
            var attributes = AttributeHandler.ParseStringOnAttributes(trvwAssemblyInfo.SelectedNode.Text);
        }
    }
}