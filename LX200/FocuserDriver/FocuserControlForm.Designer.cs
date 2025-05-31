namespace ASCOM.MaxScarzellaLX200.Focuser
{
    partial class FocuserControlForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAvanti;
        private System.Windows.Forms.Button btnIndietro;
        private System.Windows.Forms.Button btnStop;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnAvanti = new System.Windows.Forms.Button();
            this.btnIndietro = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAvanti
            // 
            this.btnAvanti.Location = new System.Drawing.Point(30, 30);
            this.btnAvanti.Name = "btnAvanti";
            this.btnAvanti.Size = new System.Drawing.Size(75, 23);
            this.btnAvanti.TabIndex = 0;
            this.btnAvanti.Text = "Avanti";
            this.btnAvanti.UseVisualStyleBackColor = true;
            this.btnAvanti.Click += new System.EventHandler(this.btnAvanti_Click);
            // 
            // btnIndietro
            // 
            this.btnIndietro.Location = new System.Drawing.Point(120, 30);
            this.btnIndietro.Name = "btnIndietro";
            this.btnIndietro.Size = new System.Drawing.Size(75, 23);
            this.btnIndietro.TabIndex = 1;
            this.btnIndietro.Text = "Indietro";
            this.btnIndietro.UseVisualStyleBackColor = true;
            this.btnIndietro.Click += new System.EventHandler(this.btnIndietro_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(75, 70);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FocuserControlForm
            // 
            this.ClientSize = new System.Drawing.Size(230, 120);
            this.Controls.Add(this.btnAvanti);
            this.Controls.Add(this.btnIndietro);
            this.Controls.Add(this.btnStop);
            this.Name = "FocuserControlForm";
            this.Text = "Controllo Focuser";
            this.ResumeLayout(false);
        }
    }
}