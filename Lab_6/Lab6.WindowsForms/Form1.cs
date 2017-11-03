using System.Collections.Generic;
using System.Windows.Forms;
using Lab6.Serialization;

namespace Lab6.WindowsForms
{
    public partial class Form1 : Form
    {
        private TreeNode _selectedNode;
        private readonly List<VisualAttributeItem> _visualAttributeItems;
        private readonly List<string> _loadedFiles;


        public Form1()
        {
            InitializeComponent();
            _visualAttributeItems = new List<VisualAttributeItem>();
            _loadedFiles = new List<string>();

            btnSaveAttributes.Enabled = false;
            btnEditNode.Enabled = false;
        }


        private void LoadTreeViewToVisualElement(TreeView treeView, List<TreeNode> treeViewNode)
        {
            treeView.Nodes.AddRange(treeViewNode.ToArray());
        }

        private void btnEditNode_Click(object sender, System.EventArgs e)
        {
            btnEditNode.Enabled = false;
            btnSaveAttributes.Enabled = true;
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
            const int leftMargin = 35;

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
                    Text = t.Key,
                    Left = leftMargin,
                    AutoSize = true
                };
                textBox.Left = leftMargin;
                textBox.Parent = pnlAttributes;
                textBox.Top = topMargin + 20;
                textBox.Text = t.Value;
                textBox.Width = 450;
                textBox.MaxLength = 82;

                textBox.KeyPress += (sender, e) =>
                {
                    if (e.KeyChar == '"')
                    {
                        e.Handled = true;
                    }
                };

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

            selectedNode.Text = parts[0] + @" " + AttributeHandler.TransformAttributesListToString(attributes) + endPart;
        }

        private void OnNewFileLoaded(string fileName, string safeFileName)
        {
            var assemblyInfo = AssemblyXmlSerializer.ParseFromXmlFile(fileName);
            if (assemblyInfo != null)
            {
                var tabPage = new TabPage(safeFileName);

                tabControl1.TabPages.Add(tabPage);

                var treeView = new TreeView();
                treeView.AfterSelect += (sender, e) =>
                {
                    ClearEditAttributesArea();
                    _selectedNode = e.Node;
                    OnNodeSelected(_selectedNode);
                };

                treeView.Parent = tabPage;
                treeView.Dock = DockStyle.Fill;

                var assemblyNode = TreeViewBuilder.BuildTreeViewFromAssembly(assemblyInfo);
                LoadTreeViewToVisualElement(treeView, assemblyNode);
            }
            else
            {
                MessageBox.Show($"Can't open file \"{safeFileName}\".", "Error.", MessageBoxButtons.OK);
            }
        }

        private void openToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ClearEditAttributesArea();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!_loadedFiles.Contains(openFileDialog1.FileName))
                {
                    _loadedFiles.Add(openFileDialog1.FileName);
                    OnNewFileLoaded(openFileDialog1.FileName, openFileDialog1.SafeFileName);
                }
                else
                {
                    var index = _loadedFiles.IndexOf(openFileDialog1.FileName);
                    tabControl1.SelectedIndex = index;
                }
            }
        }

        private void ClearEditAttributesArea()
        {
            btnEditNode.Enabled = false;
            pnlAttributes.Controls.Clear();
            btnSaveAttributes.Enabled = false;
        }

        private void closeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                var index = tabControl1.SelectedIndex;
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                _loadedFiles.RemoveAt(index);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                var index = tabControl1.SelectedIndex;
                var treeView = (TreeView) tabControl1.SelectedTab.Controls[0];

                XmlHandler.SaveTreeViewToXmlFile(treeView, _loadedFiles[index]);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (tabControl1.TabCount > 0)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var treeView = (TreeView) tabControl1.SelectedTab.Controls[0];
                    XmlHandler.SaveTreeViewToXmlFile(treeView, saveFileDialog1.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ClearEditAttributesArea();
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
    }
}