namespace EasyScada.Winforms.Controls
{
    partial class CustomCollectionEditorControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomCollectionEditorControl));
            this.tv_Items = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pg_PropGrid = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SettingText = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingText)).BeginInit();
            this.SuspendLayout();
            // 
            // tv_Items
            // 
            this.tv_Items.Dock = System.Windows.Forms.DockStyle.Top;
            this.tv_Items.FullRowSelect = true;
            this.tv_Items.ImageIndex = 0;
            this.tv_Items.ImageList = this.imageList1;
            this.tv_Items.Location = new System.Drawing.Point(0, 0);
            this.tv_Items.Name = "tv_Items";
            this.tv_Items.SelectedImageIndex = 0;
            this.tv_Items.ShowLines = false;
            this.tv_Items.ShowRootLines = false;
            this.tv_Items.Size = new System.Drawing.Size(185, 253);
            this.tv_Items.TabIndex = 0;
            this.tv_Items.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tv_Items_BeforeSelect);
            this.tv_Items.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_Items_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Tag.png");
            // 
            // pg_PropGrid
            // 
            this.pg_PropGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pg_PropGrid.Location = new System.Drawing.Point(0, 12);
            this.pg_PropGrid.Name = "pg_PropGrid";
            this.pg_PropGrid.Size = new System.Drawing.Size(366, 279);
            this.pg_PropGrid.TabIndex = 1;
            this.pg_PropGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pg_PropGrid_PropertyValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Location = new System.Drawing.Point(12, 97);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRemove);
            this.splitContainer1.Panel1.Controls.Add(this.btnAdd);
            this.splitContainer1.Panel1.Controls.Add(this.tv_Items);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.pg_PropGrid);
            this.splitContainer1.Panel2.Controls.Add(this.button3);
            this.splitContainer1.Size = new System.Drawing.Size(555, 291);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(101, 258);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 27);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(9, 258);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 27);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Properties:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(259, 131);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 27);
            this.button3.TabIndex = 1;
            this.button3.Text = "OK";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(400, 392);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(492, 392);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 27);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Expression formula:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(497, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 27);
            this.button4.TabIndex = 1;
            this.button4.Text = "Tag ...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(497, 49);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 27);
            this.button5.TabIndex = 1;
            this.button5.Text = "Check";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Result of the expression formula:";
            // 
            // SettingText
            // 
            this.SettingText.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.SettingText.AutoScrollMinSize = new System.Drawing.Size(25, 13);
            this.SettingText.BackBrush = null;
            this.SettingText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SettingText.CharHeight = 13;
            this.SettingText.CharWidth = 7;
            this.SettingText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.SettingText.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.SettingText.Font = new System.Drawing.Font("Courier New", 9F);
            this.SettingText.IsReplaceMode = false;
            this.SettingText.Location = new System.Drawing.Point(15, 17);
            this.SettingText.Name = "SettingText";
            this.SettingText.Paddings = new System.Windows.Forms.Padding(0);
            this.SettingText.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SettingText.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("SettingText.ServiceColors")));
            this.SettingText.Size = new System.Drawing.Size(476, 59);
            this.SettingText.TabIndex = 4;
            this.SettingText.Zoom = 100;
            this.SettingText.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.SettingText_TextChanged);
            // 
            // CustomCollectionEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.splitContainer1);
            this.Name = "CustomCollectionEditorControl";
            this.Size = new System.Drawing.Size(579, 443);
            this.Load += new System.EventHandler(this.CustomCollectionEditorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SettingText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tv_Items;
        private System.Windows.Forms.PropertyGrid pg_PropGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label3;
        public FastColoredTextBoxNS.FastColoredTextBox SettingText;
        private System.Windows.Forms.ImageList imageList1;
    }
}