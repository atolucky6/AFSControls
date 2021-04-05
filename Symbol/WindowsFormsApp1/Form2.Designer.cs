namespace WindowsFormsApp1
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.symFact1 = new Hten.Controls.SymFact();
            this.symFact2 = new Hten.Controls.SymFact();
            this.symFact3 = new Hten.Controls.SymFact();
            this.SuspendLayout();
            // 
            // symFact1
            // 
          //  this.symFact1.Authorization = "";
            this.symFact1.BackColor = System.Drawing.Color.Transparent;
            this.symFact1.Code = resources.GetString("symFact1.Code");
            this.symFact1.FillColor = System.Drawing.Color.Green;
            this.symFact1.FillMode = XamlHelper.FillMode.Original;
            this.symFact1.Flip = XamlHelper.FlipType.None;
            this.symFact1.Location = new System.Drawing.Point(444, 198);
            this.symFact1.Name = "symFact1";
            this.symFact1.ObjectSelect = "Pumps___Cool_pump";
            this.symFact1.Rotate = XamlHelper.Rotate.None;
            this.symFact1.Size = new System.Drawing.Size(150, 150);
            this.symFact1.TabIndex = 0;
            this.symFact1.TagName = "";
            // 
            // symFact2
            // 
           // this.symFact2.Authorization = "";
            this.symFact2.BackColor = System.Drawing.Color.Transparent;
            this.symFact2.Code = resources.GetString("symFact2.Code");
            this.symFact2.FillColor = System.Drawing.Color.Green;
            this.symFact2.FillMode = XamlHelper.FillMode.Original;
            this.symFact2.Flip = XamlHelper.FlipType.None;
            this.symFact2.Location = new System.Drawing.Point(129, 141);
            this.symFact2.Name = "symFact2";
            this.symFact2.ObjectSelect = "Pumps___Cool_pump";
            this.symFact2.Rotate = XamlHelper.Rotate.None;
            this.symFact2.Size = new System.Drawing.Size(150, 150);
            this.symFact2.TabIndex = 1;
            this.symFact2.TagName = "";
            // 
            // symFact3
            // 
         //   this.symFact3.Authorization = "";
            this.symFact3.BackColor = System.Drawing.Color.Transparent;
            this.symFact3.Code = resources.GetString("symFact3.Code");
            this.symFact3.FillColor = System.Drawing.Color.Green;
            this.symFact3.FillMode = XamlHelper.FillMode.Original;
            this.symFact3.Flip = XamlHelper.FlipType.Vertical;
            this.symFact3.Location = new System.Drawing.Point(241, 187);
            this.symFact3.Name = "symFact3";
            this.symFact3.ObjectSelect = "Pumps___Cool_pump";
            this.symFact3.Rotate = XamlHelper.Rotate.Rot_180;
            this.symFact3.Size = new System.Drawing.Size(150, 150);
            this.symFact3.TabIndex = 2;
            this.symFact3.TagName = "";
            this.symFact3.Load += new System.EventHandler(this.symFact3_Load);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.symFact3);
            this.Controls.Add(this.symFact2);
            this.Controls.Add(this.symFact1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private Hten.Controls.SymFact symFact1;
        private Hten.Controls.SymFact symFact2;
        private Hten.Controls.SymFact symFact3;
    }
}