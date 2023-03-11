using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RRL
{
    public partial class oknoAdministracja : Form
    {

       

        public oknoAdministracja()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

                   oknoUsers oknoUsers = new oknoUsers();
            oknoUsers.ShowDialog();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
           

            oknoItems oknoItems = new oknoItems();
            oknoItems.ShowDialog();

            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
         
            oknoUsergroup oknoUsergroup = new oknoUsergroup();
            oknoUsergroup.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

         
            oknoWarehouse oknoWarehouse = new oknoWarehouse(false);
            oknoWarehouse.ShowDialog();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
           
            oknoReports oknoReports = new oknoReports();
            oknoReports.ShowDialog();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            currentlyItem.stocktaking = true;
            oknoFillWithdraw oknoFillWithdraw = new oknoFillWithdraw();
            oknoFillWithdraw.ShowDialog();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

            oknoSupplier oknoSupplier = new oknoSupplier();
            oknoSupplier.ShowDialog();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

            oknoCostCenters oknoCostCenters = new oknoCostCenters();
            oknoCostCenters.ShowDialog();

        }



        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
          pictureBox1.BackColor = Color.Yellow;   
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
          pictureBox1.BackColor = Color.Transparent;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_1;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = Properties.Resources.user_0;
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.artykuly_1;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = Properties.Resources.artykuly_0;
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.grupy_0;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = Properties.Resources.grupy_1;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.magazyn_1;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = Properties.Resources.magazyn_0;
        }

        private void pictureBox7_MouseHover(object sender, EventArgs e)
        {
            pictureBox7.Image = Properties.Resources.raporty_1;
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox7.Image = Properties.Resources.raporty_0;
        }

        private void oknoAdministracja_Load(object sender, EventArgs e)
        {

            string tekst = File.ReadLines(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\settings\\autologoffconfig.txt").First();
        
            if(currentlyData.Autologoff)
            {
                checkBox1.Checked = true;

            }

            if (!currentlyData.Autologoff)

            {
                checkBox1.Checked = false;
            }


            double num1;
            bool res = double.TryParse(tekst, out num1);

            if (res)
            {
                textBox1.Text = (num1/1000).ToString();

            }

            else

            {
                MessageBox.Show("błąd w pliku settings. Ustawiono wartość 10 sek");
                textBox1.Text = "10";
            }
            //AKTYWACJA/DEAKTYWACJA UPRAWNIEŃ WG UPRAWNIEŃ

            if (!currentlyData.OptionEditUser)
            {
                pictureBox2.Visible = false;
            }

            if (!currentlyData.OptionEditItem)
            {
                pictureBox3.Visible = false;
            }

            if (!currentlyData.OptionUsergroup)
            {
                pictureBox4.Visible = false;
            }
            if (!currentlyData.OptionEditStorageplace)
            {
                pictureBox5.Visible = false;
            }
            if (!currentlyData.OptionReports)
            {
                pictureBox6.Visible = false;
            }
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            pictureBox8.Image = Properties.Resources.mpk_0;
        }

        private void pictureBox8_MouseHover(object sender, EventArgs e)
        {
            pictureBox8.Image = Properties.Resources.mpk_1png;
        }

        private void pictureBox10_MouseLeave(object sender, EventArgs e)
        {
            pictureBox10.Image = Properties.Resources.INWENTARYZACJA0;
        }

        private void pictureBox10_MouseHover(object sender, EventArgs e)
        {
            pictureBox10.Image = Properties.Resources.INWENTARYZACJA1;
        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {
            pictureBox9.Image = Properties.Resources.DOSTAWCY0;
        }

        private void pictureBox9_MouseHover(object sender, EventArgs e)
        {
            pictureBox9.Image = Properties.Resources.DOSTAWCY1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if( checkBox1.Checked)
            {
                currentlyData.Autologoff = true;
            }

            if (!checkBox1.Checked)
            {
                currentlyData.Autologoff = false;
            }

            double num1;
            bool res = double.TryParse(textBox1.Text.ToString(), out num1);

            
            //minimalna wartośc 10 sek
            if (!res || num1< 10)
            {
                textBox1.Text = "10";
                return;
            }

            else

            {

                double x = 1000 * num1;


                string path = @System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\settings\\autologoffconfig.txt";

                 File.WriteAllText(path, x.ToString()+ Environment.NewLine + currentlyData.Autologoff.ToString(), Encoding.ASCII);
            }

        }
    }
}
