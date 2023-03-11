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
    public partial class editItem : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public editItem()
        {
            InitializeComponent();
        }

        private void editItem_Load(object sender, EventArgs e)
        {
        

            if (currentlyItem.edit)
            {
            

                label6.Text = currentlyItem.ItemId.ToString();
                textBox1.Text = currentlyItem.ItemName1;
                textBox2.Text = currentlyItem.ItemName2;
                textBox3.Text = currentlyItem.ItemName3;
                textBox4.Text = currentlyItem.Barcode;
                label7.Text = currentlyItem.PicPath;
                textBox5.Text = currentlyItem.Price.ToString();
                textBox6.Text = currentlyItem.MinInventory.ToString();
                pictureBox5.ImageLocation=currentlyItem.PicPath;
                label13.Text = currentlyItem.amount.ToString();
                db.loadSupplier_combobox(comboBox1);
                comboBox1.Text = currentlyItem.Supplier;

                if (currentlyItem.Active == 1)
                {
                    checkBox1.Checked=true;
                }

                else
                {
                    checkBox1.Checked = false;
                }

            }

            if (currentlyItem.add)
            {
                label1.Text = "";
                label6.Text = "";
                currentlyItem.Active = 1;
                textBox1.Text = " - TU WPISZ NAZWĘ ARTYKUŁU  -";
                db.loadSupplier_combobox(comboBox1);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ( textBox1.Text=="")
            {
                return;
            }

            decimal cena = 0;
            int stan_min = 0;
            bool x = decimal.TryParse(textBox5.Text.ToString(),out cena);
            bool y = int.TryParse(textBox6.Text.ToString(), out stan_min);

            if(!x||!y)
            {
              
                if(!x)
                {
                    textBox5.BackColor = Color.Red;

                }

                if (!y)
                {
                    textBox6.BackColor = Color.Red;

                }
                MessageBox.Show("NIEPRAWIDŁOWE WARTOŚCI CENA LUB STAN MIN.!");
                return;
            }


            if (cena<0 || stan_min<0)
            {
                textBox5.BackColor = Color.Red;
                
                textBox6.BackColor = Color.Red;

                MessageBox.Show("NIEPRAWIDŁOWE WARTOŚCI CENA LUB STAN MIN.!");
                return;
            }

            string cena1 = cena.ToString("n2");
            cena = decimal.Parse(cena1);

            if (currentlyItem.edit)
            {
  
                 
                db.editItem(currentlyItem.ItemId,textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, label7.Text, currentlyItem.Active, cena, stan_min, comboBox1.GetItemText(comboBox1.SelectedItem));
                this.Close();
                //ustaw ostanio modyfikowany wiersz
               
            }

            if (currentlyItem.add)
            {
                db.addItem(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, label7.Text, currentlyItem.Active, cena, stan_min, comboBox1.GetItemText(comboBox1.SelectedItem));
                this.Close();
                //ustaw ostanio modyfikowany wiersz
              
            }

            currentlyItem.zerujDane();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBox1.Checked)
            {
                currentlyItem.Active = 1;
            }

            else
            {

                currentlyItem.Active = 0;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
       
            //ustaw ostanio modyfikowany wiersz
            currentlyEditUser.zerujDane();
            this.Close();
        }

    
        private void textBox3_Leave(object sender, EventArgs e)
        {
            if(db.checkUserPassword2(textBox3.Text))
            {
                textBox3.Text = "";
            }


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
            pictureBox2.BackColor = Color.Yellow;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|"
+ "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
  
                currentlyItem.PicPath = ofd.InitialDirectory + ofd.FileName;
                label7.Text = currentlyItem.PicPath;
           
                pictureBox5.Image = Image.FromFile(label7.Text);
                pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox5.Refresh();

            }

            ofd.Dispose();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            
         
        }
    }
}
