using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lab6.Serialization;

namespace Lab6.WindowsForms
{
    public partial class Form1 : Form
    {
        private const string FilePath = @"D:\Projects\repos\labs\MPP_Labs\Lab_6\Lab6.TestConsoleApp\bin\Debug\Lab5_1.TestAssembly.dll.xml";

        private TreeNode _selectedNode;
        private bool _isFilledFirstRow;

        public Form1()
        {
            InitializeComponent();
            drvwAttributes.Rows.Clear();


            var assemblyInfo = AssemblyXmlSerializer.ParseFromXmlFile(FilePath);

            var assemblyNode = TreeViewBuilder.BuildTreeViewFromAssembly(assemblyInfo);
            LoadTreeViewToVisualElement(assemblyNode);

            XmlHandler.SaveTreeViewToXmlFile(trvwAssemblyInfo, @"C:\Users\u.ambrasevich\Desktop\TreeView.xml");
        }


        private void LoadTreeViewToVisualElement(List<TreeNode> treeViewNode)
        {
            trvwAssemblyInfo.Nodes.AddRange(treeViewNode.ToArray());
        }

        private void trvwAssemblyInfo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectedNode = e.Node;
            OnNodeSelected(_selectedNode);
        }

        private void btnEditNode_Click(object sender, System.EventArgs e)
        {
            OnEditNodeStarted(_selectedNode);
        }

        private void btnSaveAttributes_Click(object sender, System.EventArgs e)
        {
            OnSaveAttributes(_selectedNode);
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

        private void OnEditNodeStarted(TreeNode selectedNode)
        {
            drvwAttributes.Rows.Clear();
            drvwAttributes.RowCount = 1;

            var attributes = AttributeHandler.ParseStringOnAttributes(selectedNode.Text);
            foreach (var attribute in attributes)
            {
//                drvwAttributes.Rows.Add();
                if (_isFilledFirstRow)
                {
                    drvwAttributes.Rows.Add();
                }
                drvwAttributes.Rows[drvwAttributes.Rows.Count - 1].Cells[0].Value = attribute.Key;
                drvwAttributes.Rows[drvwAttributes.Rows.Count - 1].Cells[1].Value = attribute.Value;
                _isFilledFirstRow = true;
//                drvwAttributes.Rows.Add();

            }
        }

        private void OnSaveAttributes(TreeNode selectedNode)
        {
            string endPart;
            if (selectedNode.Text.EndsWith("/>"))
            {
                endPart = " />";
            }
            else
            {
                endPart = ">";
            }
            var parts = selectedNode.Text.Split(' ');

            var attributes = new List<AttributeItem>();
            foreach (DataGridViewRow row in drvwAttributes.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    var attribute = new AttributeItem(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString());
                    attributes.Add(attribute);
                }


            }

            selectedNode.Text = parts[0] + " " + AttributeHandler.TransformAttributesListToString(attributes) + endPart;
        }
    }
}