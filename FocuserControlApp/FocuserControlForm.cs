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
            FocuserHardwareMove(GetCurrentPosition() + 100);
        }

        private void btnIndietro_Click(object sender, EventArgs e)
        {
            // Esempio: muovi indietro di 100 passi
            FocuserHardwareMove(GetCurrentPosition() - 100);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Utilizzo di reflection per accedere al metodo privato Halt
            typeof(FocuserHardware)
                .GetMethod("Halt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, null);
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            
        }

        private int GetCurrentPosition()
        {
            // Metodo per accedere alla posizione del focuser
            return (int)typeof(FocuserHardware)
                .GetProperty("Position", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(null);
        }

        private void FocuserHardwareMove(int position)
        {
            // Metodo wrapper per accedere a FocuserHardware.Move
            typeof(FocuserHardware)
                .GetMethod("Move", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { position });
        }

        private void FocuserControlForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSetup_Click_1(object sender, EventArgs e)
        {
            typeof(FocuserHardware)
                .GetMethod("SetupDialog", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Invoke(null, null);
        }
    }
}
