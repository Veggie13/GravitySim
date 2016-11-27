namespace GravitySim
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
            this._panel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this._txtParticleCount = new System.Windows.Forms.Label();
            this._txtEnergy = new System.Windows.Forms.Label();
            this._txtAM = new System.Windows.Forms.Label();
            this._txtMomentum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.Location = new System.Drawing.Point(13, 13);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(1000, 1000);
            this._panel.TabIndex = 0;
            this._panel.Paint += new System.Windows.Forms.PaintEventHandler(this._panel_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1028, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // _txtParticleCount
            // 
            this._txtParticleCount.AutoSize = true;
            this._txtParticleCount.Location = new System.Drawing.Point(1025, 52);
            this._txtParticleCount.Name = "_txtParticleCount";
            this._txtParticleCount.Size = new System.Drawing.Size(35, 13);
            this._txtParticleCount.TabIndex = 2;
            this._txtParticleCount.Text = "label1";
            // 
            // _txtEnergy
            // 
            this._txtEnergy.AutoSize = true;
            this._txtEnergy.Location = new System.Drawing.Point(1025, 83);
            this._txtEnergy.Name = "_txtEnergy";
            this._txtEnergy.Size = new System.Drawing.Size(35, 13);
            this._txtEnergy.TabIndex = 3;
            this._txtEnergy.Text = "label1";
            // 
            // _txtAM
            // 
            this._txtAM.AutoSize = true;
            this._txtAM.Location = new System.Drawing.Point(1025, 115);
            this._txtAM.Name = "_txtAM";
            this._txtAM.Size = new System.Drawing.Size(35, 13);
            this._txtAM.TabIndex = 4;
            this._txtAM.Text = "label1";
            // 
            // _txtMomentum
            // 
            this._txtMomentum.AutoSize = true;
            this._txtMomentum.Location = new System.Drawing.Point(1025, 150);
            this._txtMomentum.Name = "_txtMomentum";
            this._txtMomentum.Size = new System.Drawing.Size(35, 13);
            this._txtMomentum.TabIndex = 5;
            this._txtMomentum.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 1019);
            this.Controls.Add(this._txtMomentum);
            this.Controls.Add(this._txtAM);
            this.Controls.Add(this._txtEnergy);
            this.Controls.Add(this._txtParticleCount);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._panel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label _txtParticleCount;
        private System.Windows.Forms.Label _txtEnergy;
        private System.Windows.Forms.Label _txtAM;
        private System.Windows.Forms.Label _txtMomentum;
    }
}

