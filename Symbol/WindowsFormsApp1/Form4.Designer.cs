namespace PropertyCode
{
    partial class Form4
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
            this.easyDriverConnector1 = new EasyScada.Winforms.Controls.EasyDriverConnector(this.components);
            this.symFact1 = new vGraphic.SymFact();
            ((System.ComponentModel.ISupportInitialize)(this.easyDriverConnector1)).BeginInit();
            this.SuspendLayout();
            // 
            // easyDriverConnector1
            // 
            this.easyDriverConnector1.CommunicationMode = EasyScada.Core.CommunicationMode.ReceiveFromServer;
            this.easyDriverConnector1.Port = ((ushort)(8800));
            this.easyDriverConnector1.RefreshRate = 1000;
            this.easyDriverConnector1.ServerAddress = "127.0.0.1";
            // 
            // symFact1
            // 
            this.symFact1.BackColor = System.Drawing.Color.Transparent;
            this.symFact1.FillColor = System.Drawing.Color.Green;
            this.symFact1.FillMode = XamlHelper.FillMode.Original;
            this.symFact1.Flip = XamlHelper.FlipType.None;
            this.symFact1.Location = new System.Drawing.Point(527, 199);
            this.symFact1.Name = "symFact1";
            this.symFact1.ObjectSelect = "Pumps___Classic_pump_3";
            this.symFact1.Rotate = XamlHelper.Rotate.None;
            this.symFact1.ScadaAutoCode = "";
            this.symFact1.Size = new System.Drawing.Size(150, 150);
            this.symFact1.TabIndex = 0;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 621);
            this.Controls.Add(this.symFact1);
            this.Name = "Form4";
            this.Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)(this.easyDriverConnector1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private EasyScada.Winforms.Controls.EasyDriverConnector easyDriverConnector1;
        private vGraphic.SymFact symFact1;
    }
}