namespace WindowsFormsApp1
{
    partial class Form3
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
            this.colorPicker1 = new LaMarvin.Windows.Forms.ColorPicker();
            this.symFact3 = new vGraphic.SymFact();
            this.symFact4 = new vGraphic.SymFact();
            this.symFact2 = new vGraphic.SymFact();
            this.symFact1 = new vGraphic.SymFact();
            this.SuspendLayout();
            // 
            // colorPicker1
            // 
            this.colorPicker1.Location = new System.Drawing.Point(498, 175);
            this.colorPicker1.Name = "colorPicker1";
            this.colorPicker1.Size = new System.Drawing.Size(75, 23);
            this.colorPicker1.TabIndex = 0;
            // 
            // symFact3
            // 
            this.symFact3.BackColor = System.Drawing.Color.Transparent;
            this.symFact3.Code = "";
            this.symFact3.FillColor = System.Drawing.Color.Lime;
            this.symFact3.FillMode = XamlHelper.FillMode.Solid;
            this.symFact3.Flip = XamlHelper.FlipType.None;
            this.symFact3.Location = new System.Drawing.Point(442, 61);
            this.symFact3.Name = "symFact3";
            this.symFact3.ObjectSelect = "Graphics___Conect";
            this.symFact3.Rotate = XamlHelper.Rotate.None;
            this.symFact3.Size = new System.Drawing.Size(442, 326);
            this.symFact3.TabIndex = 3;
            this.symFact3.TagName = "";
            // 
            // symFact4
            // 
            this.symFact4.BackColor = System.Drawing.Color.Transparent;
            this.symFact4.Code = "";
            this.symFact4.FillColor = System.Drawing.Color.Red;
            this.symFact4.FillMode = XamlHelper.FillMode.Shaded;
            this.symFact4.Flip = XamlHelper.FlipType.Horizontal;
            this.symFact4.Location = new System.Drawing.Point(680, 112);
            this.symFact4.Name = "symFact4";
            this.symFact4.ObjectSelect = "Graphics___Cyclone_n";
            this.symFact4.Rotate = XamlHelper.Rotate.Rot_180;
            this.symFact4.Size = new System.Drawing.Size(191, 326);
            this.symFact4.TabIndex = 4;
            this.symFact4.TagName = "";
            // 
            // symFact2
            // 
            this.symFact2.BackColor = System.Drawing.Color.Transparent;
            this.symFact2.Code = "";
            this.symFact2.FillColor = System.Drawing.Color.Red;
            this.symFact2.FillMode = XamlHelper.FillMode.Solid;
            this.symFact2.Flip = XamlHelper.FlipType.None;
            this.symFact2.Location = new System.Drawing.Point(281, 73);
            this.symFact2.Name = "symFact2";
            this.symFact2.ObjectSelect = "Graphics___Cyclone_n";
            this.symFact2.Rotate = XamlHelper.Rotate.None;
            this.symFact2.Size = new System.Drawing.Size(191, 326);
            this.symFact2.TabIndex = 2;
            this.symFact2.TagName = "";
            // 
            // symFact1
            // 
            this.symFact1.BackColor = System.Drawing.Color.Transparent;
            this.symFact1.Code = "";
            this.symFact1.FillColor = System.Drawing.Color.Red;
            this.symFact1.FillMode = XamlHelper.FillMode.Shaded;
            this.symFact1.Flip = XamlHelper.FlipType.Horizontal;
            this.symFact1.Location = new System.Drawing.Point(168, 90);
            this.symFact1.Name = "symFact1";
            this.symFact1.ObjectSelect = "Graphics___Cyclone_n";
            this.symFact1.Rotate = XamlHelper.Rotate.None;
            this.symFact1.Size = new System.Drawing.Size(191, 326);
            this.symFact1.TabIndex = 1;
            this.symFact1.TagName = "";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 450);
            this.Controls.Add(this.symFact3);
            this.Controls.Add(this.symFact4);
            this.Controls.Add(this.symFact2);
            this.Controls.Add(this.symFact1);
            this.Controls.Add(this.colorPicker1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }

        #endregion

        private LaMarvin.Windows.Forms.ColorPicker colorPicker1;
        private vGraphic.SymFact symFact1;
        private vGraphic.SymFact symFact2;
        private vGraphic.SymFact symFact3;
        private vGraphic.SymFact symFact4;
    }
}