namespace EasyScada.Winforms.Controls
{
    partial class frmProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProperties));
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeRoot = new System.Windows.Forms.TreeView();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.MainPanel1 = new System.Windows.Forms.Panel();
            this._AutoListIcons = new System.Windows.Forms.ImageList();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.button2);
            this.splitContainer3.Panel2.Controls.Add(this.button1);
            this.splitContainer3.Panel2.Controls.Add(this.Label);
            this.splitContainer3.Panel2.Controls.Add(this.MainPanel1);
            this.splitContainer3.Size = new System.Drawing.Size(819, 459);
            this.splitContainer3.SplitterDistance = 221;
            this.splitContainer3.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.treeRoot);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 459);
            this.panel1.TabIndex = 0;
            // 
            // treeRoot
            // 
            this.treeRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRoot.ImageIndex = 0;
            this.treeRoot.ImageList = this._AutoListIcons;
            this.treeRoot.Location = new System.Drawing.Point(0, 0);
            this.treeRoot.Name = "treeRoot";
            this.treeRoot.SelectedImageIndex = 0;
            this.treeRoot.Size = new System.Drawing.Size(217, 455);
            this.treeRoot.TabIndex = 0;
            this.treeRoot.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeRoot_AfterSelect);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(498, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 27);
            this.button2.TabIndex = 4;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(406, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OK_CLick);
            // 
            // Label
            // 
            this.Label.BackColor = System.Drawing.Color.Gray;
            this.Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label.ForeColor = System.Drawing.Color.White;
            this.Label.Location = new System.Drawing.Point(0, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(594, 20);
            this.Label.TabIndex = 3;
            this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainPanel1
            // 
            this.MainPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainPanel1.Location = new System.Drawing.Point(0, 22);
            this.MainPanel1.Name = "MainPanel1";
            this.MainPanel1.Size = new System.Drawing.Size(576, 398);
            this.MainPanel1.TabIndex = 0;
            // 
            // _AutoListIcons
            // 
            this._AutoListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_AutoListIcons.ImageStream")));
            this._AutoListIcons.TransparentColor = System.Drawing.Color.Magenta;
            this._AutoListIcons.Images.SetKeyName(0, "VSObject_Class.bmp");
            this._AutoListIcons.Images.SetKeyName(1, "VSObject_Class_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(2, "VSObject_Class_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(3, "VSObject_Class_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(4, "VSObject_Class_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(5, "VSObject_Class_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(6, "VSObject_Constant.bmp");
            this._AutoListIcons.Images.SetKeyName(7, "VSObject_Constant_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(8, "VSObject_Constant_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(9, "VSObject_Constant_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(10, "VSObject_Constant_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(11, "VSObject_Constant_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(12, "VSObject_Delegate.bmp");
            this._AutoListIcons.Images.SetKeyName(13, "VSObject_Delegate_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(14, "VSObject_Delegate_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(15, "VSObject_Delegate_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(16, "VSObject_Delegate_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(17, "VSObject_Delegate_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(18, "VSObject_Enum.bmp");
            this._AutoListIcons.Images.SetKeyName(19, "VSObject_Enum_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(20, "VSObject_Enum_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(21, "VSObject_Enum_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(22, "VSObject_Enum_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(23, "VSObject_Enum_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(24, "VSObject_EnumItem.bmp");
            this._AutoListIcons.Images.SetKeyName(25, "VSObject_EnumItem_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(26, "VSObject_EnumItem_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(27, "VSObject_EnumItem_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(28, "VSObject_EnumItem_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(29, "VSObject_EnumItem_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(30, "VSObject_Event.bmp");
            this._AutoListIcons.Images.SetKeyName(31, "VSObject_Event_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(32, "VSObject_Event_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(33, "VSObject_Event_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(34, "VSObject_Event_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(35, "VSObject_Event_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(36, "VSObject_Exception.bmp");
            this._AutoListIcons.Images.SetKeyName(37, "VSObject_Exception_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(38, "VSObject_Exception_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(39, "VSObject_Exception_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(40, "VSObject_Exception_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(41, "VSObject_Exception_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(42, "VSObject_Field.bmp");
            this._AutoListIcons.Images.SetKeyName(43, "VSObject_Field_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(44, "VSObject_Field_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(45, "VSObject_Field_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(46, "VSObject_Field_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(47, "VSObject_Field_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(48, "VSObject_Interface.bmp");
            this._AutoListIcons.Images.SetKeyName(49, "VSObject_Interface_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(50, "VSObject_Interface_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(51, "VSObject_Interface_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(52, "VSObject_Interface_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(53, "VSObject_Interface_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(54, "VSObject_Macro.bmp");
            this._AutoListIcons.Images.SetKeyName(55, "VSObject_Macro_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(56, "VSObject_Macro_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(57, "VSObject_Macro_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(58, "VSObject_Macro_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(59, "VSObject_Macro_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(60, "VSObject_Map.bmp");
            this._AutoListIcons.Images.SetKeyName(61, "VSObject_Map_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(62, "VSObject_Map_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(63, "VSObject_Map_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(64, "VSObject_Map_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(65, "VSObject_Map_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(66, "VSObject_MapItem.bmp");
            this._AutoListIcons.Images.SetKeyName(67, "VSObject_MapItem_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(68, "VSObject_MapItem_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(69, "VSObject_MapItem_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(70, "VSObject_MapItem_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(71, "VSObject_MapItem_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(72, "VSObject_Method.bmp");
            this._AutoListIcons.Images.SetKeyName(73, "VSObject_Method_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(74, "VSObject_Method_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(75, "VSObject_Method_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(76, "VSObject_Method_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(77, "VSObject_Method_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(78, "VSObject_MethodOverload.bmp");
            this._AutoListIcons.Images.SetKeyName(79, "VSObject_MethodOverload_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(80, "VSObject_MethodOverload_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(81, "VSObject_MethodOverload_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(82, "VSObject_MethodOverload_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(83, "VSObject_MethodOverload_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(84, "VSObject_Module.bmp");
            this._AutoListIcons.Images.SetKeyName(85, "VSObject_Module_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(86, "VSObject_Module_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(87, "VSObject_Module_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(88, "VSObject_Module_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(89, "VSObject_Module_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(90, "VSObject_Namespace.bmp");
            this._AutoListIcons.Images.SetKeyName(91, "VSObject_Namespace_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(92, "VSObject_Namespace_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(93, "VSObject_Namespace_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(94, "VSObject_Namespace_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(95, "VSObject_Namespace_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(96, "VSObject_Object.bmp");
            this._AutoListIcons.Images.SetKeyName(97, "VSObject_Object_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(98, "VSObject_Object_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(99, "VSObject_Object_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(100, "VSObject_Object_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(101, "VSObject_Object_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(102, "VSObject_Operator.bmp");
            this._AutoListIcons.Images.SetKeyName(103, "VSObject_Operator_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(104, "VSObject_Operator_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(105, "VSObject_Operator_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(106, "VSObject_Operator_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(107, "VSObject_Operator_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(108, "VSObject_Properties.bmp");
            this._AutoListIcons.Images.SetKeyName(109, "VSObject_Properties_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(110, "VSObject_Properties_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(111, "VSObject_Properties_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(112, "VSObject_Properties_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(113, "VSObject_Properties_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(114, "VSObject_Structure.bmp");
            this._AutoListIcons.Images.SetKeyName(115, "VSObject_Structure_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(116, "VSObject_Structure_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(117, "VSObject_Structure_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(118, "VSObject_Structure_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(119, "VSObject_Structure_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(120, "VSObject_Template.bmp");
            this._AutoListIcons.Images.SetKeyName(121, "VSObject_Template_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(122, "VSObject_Template_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(123, "VSObject_Template_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(124, "VSObject_Template_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(125, "VSObject_Template_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(126, "VSObject_Type.bmp");
            this._AutoListIcons.Images.SetKeyName(127, "VSObject_Type_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(128, "VSObject_Type_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(129, "VSObject_Type_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(130, "VSObject_Type_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(131, "VSObject_Type_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(132, "VSObject_TypeDef.bmp");
            this._AutoListIcons.Images.SetKeyName(133, "VSObject_TypeDef_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(134, "VSObject_TypeDef_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(135, "VSObject_TypeDef_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(136, "VSObject_TypeDef_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(137, "VSObject_TypeDef_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(138, "VSObject_Union.bmp");
            this._AutoListIcons.Images.SetKeyName(139, "VSObject_Union_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(140, "VSObject_Union_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(141, "VSObject_Union_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(142, "VSObject_Union_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(143, "VSObject_Union_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(144, "VSObject_ValueType.bmp");
            this._AutoListIcons.Images.SetKeyName(145, "VSObject_ValueType_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(146, "VSObject_ValueType_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(147, "VSObject_ValueType_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(148, "VSObject_ValueType_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(149, "VSObject_ValueType_Shortcut.bmp");
            // 
            // frmProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 459);
            this.Controls.Add(this.splitContainer3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Animation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProperties_FormClosing);
            this.Load += new System.EventHandler(this.Properties_Load);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeRoot;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.Panel MainPanel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList _AutoListIcons;
    }
}