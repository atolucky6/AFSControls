﻿namespace Hten.SymFact
{
    partial class FormTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTest));
            this.symFact1 = new Hten.Controls.SymFact();
            this.SuspendLayout();
            // 
            // symFact1
            // 
         //   this.symFact1.Authorization = "";
            this.symFact1.BackColor = System.Drawing.Color.Transparent;
            this.symFact1.Code = resources.GetString("symFact1.Code");
            this.symFact1.FillColor = System.Drawing.Color.Green;
            this.symFact1.FillMode = XamlHelper.FillMode.Original;
            this.symFact1.Flip = XamlHelper.FlipType.Vertical;
            this.symFact1.Location = new System.Drawing.Point(215, 167);
            this.symFact1.Name = "symFact1";
            this.symFact1.ObjectSelect = "Pumps___Cool_pump";
            this.symFact1.Rotate = XamlHelper.Rotate.None;
            this.symFact1.Size = new System.Drawing.Size(150, 150);
            this.symFact1.TabIndex = 0;
            this.symFact1.TagName = "";
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.symFact1);
            this.Name = "FormTest";
            this.Text = "FormTest";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SymFact symFact1;
    }
}