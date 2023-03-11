using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using System.IO;

namespace RRL
{
    public partial class oknoMenu : Form
    {
        public currentlySession cs = new currentlySession();

        public oknoMenu()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
            this.Close();
            oknoLogowanie oknoLogowanie = new oknoLogowanie();
            oknoLogowanie.Show();

            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currentlyItem.withdraw = true;
            currentlyItem.fill= false;
            oknoFillWithdraw oknoFillWithdraw = new oknoFillWithdraw();
            oknoFillWithdraw.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {


            currentlyItem.withdraw = false;
            currentlyItem.fill = true;
            oknoFillWithdraw oknoFillWithdraw = new oknoFillWithdraw();
            oknoFillWithdraw.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            oknoAdministracja oknoAdministracja = new oknoAdministracja();
            oknoAdministracja.ShowDialog();

        }

        private void oknoMenu_Load(object sender, EventArgs e)
        {
            //AKTYWACJA/DEAKTYWACJA UPRAWNIEŃ WG UPRAWNIEŃ

            if (!currentlyData.OptionAdministrator)
            {
                pictureBox4.Visible = false;
            }

            if (!currentlyData.OptionReceipt)
            {
                pictureBox1.Visible = false;
            }

            if (!currentlyData.OptionReceive)
            {
                pictureBox2.Visible = false;
            }


            label1.Text = "UŻYTKOWNIK: " + currentlyData.UserName;

            //uruchomienie opcji autolowylogowania

            string tekst1 = File.ReadLines(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\settings\\autologoffconfig.txt").First();
            string SecondLine = File.ReadLines(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\settings\\autologoffconfig.txt").Skip(1).First();

           


            Double czasDoWylogowania = double.Parse(tekst1);

            currentlySession cs = new currentlySession();
            cs.start(czasDoWylogowania);
            cs.zeit = czasDoWylogowania;


            bool g = bool.TryParse(SecondLine, out g);

            if (g)
            {
               currentlyData.Autologoff= true;

            }

            else

            {
                currentlyData.Autologoff = true;
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.pobranie_1;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.pobranie_0;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.przyjecie_1;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.przyjecie_0;
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.administracja_1;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.administracja_0;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.wyloguj_11;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.wyloguj_01;
        }
    }
}
