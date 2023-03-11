using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Windows.Forms;
using System.IO;
namespace RRL
{
    public partial class oknoLogowanie : Form
    {

        DateTime dataakt = DateTime.Now;

        
        polaczenieBazaDanych db = new polaczenieBazaDanych();
    
        public oknoLogowanie()
        {
            InitializeComponent();
        }

        //funkcja zmieniająca klucz
        void zmienklucz()
        {
       
            string path = @System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\license\\licencja.txt";
            File.AppendAllText(path, "X",Encoding.ASCII);

        }

         private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


            if (db.checkUserPassword(textBox1.Text) == true)
            {
                db.loadDetailsLoggeduser(textBox1.Text);

                //this.Hide();
                oknoMenu oknoMenu = new oknoMenu();
                oknoMenu.ShowDialog();

            }

            else
            {
                MessageBox.Show("UZYTKOWNIK ZABLOKOWANY LUB NIEPRAWIDŁWE HASŁO", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "";
            }

      
            
        }

        private void oknoLogowanie_Load(object sender, EventArgs e)
        {

            label1.Text = DateTime.Now.ToLongDateString();

            //licencja sprawdzenie klucza oraz daty użytkowania//

            if (File.Exists(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\license\\licencja.txt"))
             {   
                string tekst = System.IO.File.ReadAllText(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\license\\licencja.txt");
                string tekst1 = System.IO.File.ReadAllText(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\license\\date.txt");

                string rok = tekst1.Substring(0,4);
                string mies = tekst1.Substring(4, 2);

            
                if (tekst != "KHUASNASA213217AB67H86")
                {
                    label1.Text = "NIEPRAWIDŁOWY KLUCZ LICENCJI";
                    label1.BackColor = Color.Yellow;
                    textBox1.ReadOnly = true;

                }

            

                else if (dataakt.Year > int.Parse(rok.ToString()))
                {
                    label1.Text = "LICENCJA WYGASŁA";
                    label1.BackColor = Color.Yellow;
                    textBox1.ReadOnly = true;
                    zmienklucz();

                }

                else if (dataakt.Year == int.Parse(rok.ToString()) && dataakt.Month > int.Parse(mies.ToString()))
                {
                    label1.Text = "LICENCJA WYGASŁA";
                    label1.BackColor = Color.Yellow;
                    textBox1.ReadOnly = true;
                    zmienklucz();

                }
                else
                {
                    label1.Text = DateTime.Now.ToLongDateString();
                }
            }


            else
            {
                label1.Text = "BRAK AKTYWNEJ LICENCJI";
                textBox1.ReadOnly = true;

            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (db.checkUserPassword(textBox1.Text) == true)
                {
                    db.loadDetailsLoggeduser(textBox1.Text);

                    this.Hide();
                    oknoMenu oknoMenu = new oknoMenu();
                    oknoMenu.ShowDialog();

                }

                else
                {
                    MessageBox.Show("UZYTKOWNIK ZABLOKOWANY LUB NIEPRAWIDŁOWE HASŁO", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                }
            }
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Transparent;
        }



        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            Color cl = new Color();

            bool a = db.CheckDbConnection(db.db_path);

        
            if (a)
            {
                cl = Color.FromArgb(61,253,17);
            }

            else
            {
                cl = Color.Red;
            }

            float x = float.Parse(pictureBox4.Size.Width.ToString());
            float y = float.Parse(pictureBox4.Size.Height.ToString());
            double x1 = x * 0.5;
            double y1 = y * 0.5;
            x = float.Parse(x1.ToString());
            y = float.Parse(y1.ToString());


            Pen pen = new Pen(cl, 2);
            SolidBrush brush = new SolidBrush(cl);

            e.Graphics.DrawEllipse(pen, x/4, y/4, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);
             e.Graphics.FillEllipse(brush, x/4, y/4, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);



        }

  

        private void label2_MouseHover(object sender, EventArgs e)
        {
            label2.BackColor = Color.White;

        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.BackColor = SystemColors.ActiveCaption;

        }


        void animacja ()

        {
            pictureBox4.Image = null;
            pictureBox4.Invalidate();


            Color cl = new Color();

            bool a = db.CheckDbConnection(db.db_path);

            System.Drawing.Graphics formGraphics ;
            formGraphics = this.CreateGraphics();
            if (a)
            {
                cl = Color.FromArgb(61, 253, 17);
            }

            else
            {
                cl = Color.Red;
            }

            float x = float.Parse(pictureBox4.Size.Width.ToString());
            float y = float.Parse(pictureBox4.Size.Height.ToString());
            double x1 = x * 0.5;
            double y1 = y * 0.5;
            x = float.Parse(x1.ToString());
            y = float.Parse(y1.ToString());


            Pen pen = new Pen(Color.Black, 2);
            SolidBrush brush = new SolidBrush(Color.Black);

            formGraphics.DrawEllipse(pen, pictureBox4.Location.X + 10, pictureBox4.Location.Y, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);
            formGraphics.FillEllipse(brush, pictureBox4.Location.X + 10, pictureBox4.Location.Y, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);

            // formGraphics.DrawEllipse(pen, x / 4, y / 4, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);
            //  formGraphics.FillEllipse(brush, x / 4, y / 4, pictureBox4.Size.Width / 4, pictureBox4.Size.Height / 4);

        }

        private void label2_Click(object sender, EventArgs e)
        {

            animacja();

        }

   
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();

            oknoWarehouse oknoWarehouse = new oknoWarehouse(false);
            oknoWarehouse.ShowDialog();
         

        }

   




    }
}
