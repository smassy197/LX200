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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FocuserControlForm));
            this.btnAvanti = new System.Windows.Forms.Button();
            this.btnIndietro = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAvanti
            // 
            this.btnAvanti.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAvanti.Location = new System.Drawing.Point(15, 31);
            this.btnAvanti.Name = "btnAvanti";
            this.btnAvanti.Size = new System.Drawing.Size(84, 71);
            this.btnAvanti.TabIndex = 0;
            this.btnAvanti.Text = "Avanti";
            this.btnAvanti.UseVisualStyleBackColor = true;
            this.btnAvanti.Click += new System.EventHandler(this.btnAvanti_Click);
            // 
            // btnIndietro
            // 
            this.btnIndietro.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIndietro.Location = new System.Drawing.Point(195, 31);
            this.btnIndietro.Name = "btnIndietro";
            this.btnIndietro.Size = new System.Drawing.Size(84, 71);
            this.btnIndietro.TabIndex = 1;
            this.btnIndietro.Text = "Indietro";
            this.btnIndietro.UseVisualStyleBackColor = true;
            this.btnIndietro.Click += new System.EventHandler(this.btnIndietro_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(105, 31);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 71);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(108, 141);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(75, 23);
            this.btnSetup.TabIndex = 3;
            this.btnSetup.Text = "Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click_1);
            // 
            // FocuserControlForm
            // 
            this.ClientSize = new System.Drawing.Size(287, 191);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.btnAvanti);
            this.Controls.Add(this.btnIndietro);
            this.Controls.Add(this.btnStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FocuserControlForm";
            this.Text = "Controllo Focuser";
            this.Load += new System.EventHandler(this.FocuserControlForm_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnSetup;
    }
}