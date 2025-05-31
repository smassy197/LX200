using System;
using System.Windows.Forms;

namespace ASCOM.MaxScarzellaLX200.Focuser
{
    public partial class FocuserControlForm : Form
    {
        public FocuserControlForm()
        {
            InitializeComponent();
        }

        private void btnAvanti_Click(object sender, EventArgs e)
        {
            // Esempio: muovi avanti di 100 passi
            FocuserHardware.Move(FocuserHardware.Position + 100);
        }

        private void btnIndietro_Click(object sender, EventArgs e)
        {
            // Esempio: muovi indietro di 100 passi
            FocuserHardware.Move(FocuserHardware.Position - 100);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            FocuserHardware.Halt();
        }
    }
}
