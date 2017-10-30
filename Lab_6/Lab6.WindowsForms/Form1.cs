using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using Lab6.Serialization;

namespace Lab6.WindowsForms
{
    public partial class Form1 : Form
    {
        private const string FilePath =
            @"D:\БГУИР\СПП\5сем\MPP_Labs\Lab_6\Lab6.TestConsoleApp\bin\Debug\Lab5_1.TestAssembly.dll.xml";

        private TreeNode _selectedNode;
        private bool _isFilledFirstRow;
        private List<VisualAttributeItem> _visualAttributeItems;
        private List<string> _loadedFiles;
        private List<TreeView> _treeViews;


        public Form1()
        {
            InitializeComponent();
            _visualAttributeItems = new List<VisualAttributeItem>();
            _loadedFiles = new List<string>();
            _treeViews = new List<TreeView>();
        }


        private void LoadTreeViewToVisualElement(TreeView treeView, List<TreeNode> treeViewNode)
        {
            treeView.Nodes.AddRange(treeViewNode.ToArray());
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
            var topMargin = 20;

            var attributes = AttributeHandler.ParseStringOnAttributes(selectedNode.Text);
            pnlAttributes.Controls.Clear();
            _visualAttributeItems.Clear();
            foreach (AttributeItem t in attributes)
            {
                var textBox = new TextBox();
                var label = new Label
                {
                    Parent = pnlAttributes,
                    Top = topMargin,
                    Text = t.Key
                };
                textBox.Parent = pnlAttributes;
                textBox.Top = topMargin + 30;
                textBox.Text = t.Value;
                textBox.Width = 300;
                topMargin += 60;

                var visualAttributeItem = new VisualAttributeItem(textBox, label);
                _visualAttributeItems.Add(visualAttributeItem);
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
            foreach (var visualAttributeItem in _visualAttributeItems)
            {
                var attribute = new AttributeItem(visualAttributeItem.KeyLabel.Text, visualAttributeItem.ValueTextBox.Text);
                attributes.Add(attribute);
            }

            selectedNode.Text = parts[0] + " " + AttributeHandler.TransformAttributesListToString(attributes) + endPart;
        }

        private void OnNewFileLoaded(string fileName, string safeFileName)
        {
            var tabpage = new TabPage(safeFileName);
            
            tabControl1.TabPages.Add(tabpage);

            var treeview = new TreeView();
            treeview.AfterSelect += (sender, e) =>
            {
                _selectedNode = e.Node;
                OnNodeSelected(_selectedNode);
            };

            treeview.Parent = tabpage;
            treeview.Dock = DockStyle.Fill;

            var assemblyInfo = AssemblyXmlSerializer.ParseFromXmlFile(fileName);

            var assemblyNode = TreeViewBuilder.BuildTreeViewFromAssembly(assemblyInfo);
            LoadTreeViewToVisualElement(treeview, assemblyNode);

        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _loadedFiles.Add(openFileDialog1.FileName);
                OnNewFileLoaded(openFileDialog1.FileName, openFileDialog1.SafeFileName);
            }
        }


        private sealed class VisualAttributeItem
        {
            public TextBox ValueTextBox { get; }

            public Label KeyLabel { get; }


            public VisualAttributeItem(TextBox textBox, Label label)
            {
                ValueTextBox = textBox;
                KeyLabel = label;
            }
        }

        private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var index = tabControl1.SelectedIndex;
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            _loadedFiles.RemoveAt(index);
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            var index = tabControl1.SelectedIndex;
            var treeView = (TreeView)tabControl1.SelectedTab.Controls[0];

            XmlHandler.SaveTreeViewToXmlFile(treeView, _loadedFiles[index]);
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var treeView = (TreeView)tabControl1.SelectedTab.Controls[0];
                XmlHandler.SaveTreeViewToXmlFile(treeView, saveFileDialog1.FileName);
            }
        }
    }
}