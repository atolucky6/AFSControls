namespace vControl
{
    partial class UserControl1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorPicker1 = new LaMarvin.Windows.Forms.ColorPicker();
            this.SuspendLayout();
            // 
            // colorPicker1
            // 
            this.colorPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorPicker1.Location = new System.Drawing.Point(0, 0);
            this.colorPicker1.Name = "colorPicker1";
            this.colorPicker1.Size = new System.Drawing.Size(150, 114);
            this.colorPicker1.TabIndex = 0;
            this.colorPicker1.ColorChanged += new System.EventHandler(this.colorPicker1_ColorChanged);
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colorPicker1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(150, 114);
            this.ResumeLayout(false);

        }

        #endregion

        private LaMarvin.Windows.Forms.ColorPicker colorPicker1;
    }
}
