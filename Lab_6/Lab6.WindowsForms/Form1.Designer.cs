namespace Lab6.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.trvwAssemblyInfo = new System.Windows.Forms.TreeView();
            this.btnEditNode = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.drvwAttributes = new System.Windows.Forms.DataGridView();
            this.Keys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Values = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.drvwAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // trvwAssemblyInfo
            // 
            this.trvwAssemblyInfo.Location = new System.Drawing.Point(0, 48);
            this.trvwAssemblyInfo.Name = "trvwAssemblyInfo";
            this.trvwAssemblyInfo.Size = new System.Drawing.Size(883, 356);
            this.trvwAssemblyInfo.TabIndex = 1;
            this.trvwAssemblyInfo.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvwAssemblyInfo_AfterSelect);
            // 
            // btnEditNode
            // 
            this.btnEditNode.Location = new System.Drawing.Point(997, 83);
            this.btnEditNode.Name = "btnEditNode";
            this.btnEditNode.Size = new System.Drawing.Size(107, 49);
            this.btnEditNode.TabIndex = 2;
            this.btnEditNode.Text = "Edit";
            this.btnEditNode.UseVisualStyleBackColor = true;
            this.btnEditNode.Click += new System.EventHandler(this.btnEditNode_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(916, 165);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 296);
            this.panel1.TabIndex = 3;
            // 
            // drvwAttributes
            // 
            this.drvwAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.drvwAttributes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Keys,
            this.Values});
            this.drvwAttributes.Location = new System.Drawing.Point(415, 444);
            this.drvwAttributes.Name = "drvwAttributes";
            this.drvwAttributes.Size = new System.Drawing.Size(364, 106);
            this.drvwAttributes.TabIndex = 4;
            // 
            // Keys
            // 
            this.Keys.HeaderText = "Keys";
            this.Keys.Name = "Keys";
            this.Keys.ReadOnly = true;
            // 
            // Values
            // 
            this.Values.HeaderText = "Values";
            this.Values.Name = "Values";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 675);
            this.Controls.Add(this.drvwAttributes);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnEditNode);
            this.Controls.Add(this.trvwAssemblyInfo);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.drvwAttributes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView trvwAssemblyInfo;
        private System.Windows.Forms.Button btnEditNode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView drvwAttributes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Keys;
        private System.Windows.Forms.DataGridViewTextBoxColumn Values;
    }
}

