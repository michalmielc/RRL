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
    public partial class editSupplier : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public editSupplier()
        {
            InitializeComponent();
        }

        private void editSupplier_Load(object sender, EventArgs e)
        {
        

            if (currentlyEditSupplier.edit)
            {
            

                label6.Text =  currentlyEditSupplier.SupplierId.ToString();
                textBox1.Text = currentlyEditSupplier.SupplierName.ToString();
                textBox2.Text = currentlyEditSupplier.SupplierEmail.ToString();
       


            }

            if (currentlyEditSupplier.add)
            {
             
                label6.Text = "";
                textBox1.Text = " - TU WPISZ NAZWĘ DOSTAWCY  -";
                textBox2.Text = "";
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ( textBox1.Text=="")
            {
                return;
            }

      
            if (currentlyEditSupplier.edit)
            {
              
                db.editSupplier(currentlyEditSupplier.SupplierId,textBox1.Text, textBox2.Text );
                this.Close();
                //ustaw ostanio modyfikowany wiersz
               
            }

            if (currentlyEditSupplier.add)
            {
                db.addSupplier(textBox1.Text, textBox2.Text);
                this.Close();
                //ustaw ostanio modyfikowany wiersz
              
            }

            currentlyEditSupplier.zerujDane();
            
        }

      

        private void pictureBox2_Click(object sender, EventArgs e)
        {
       
            //ustaw ostanio modyfikowany wiersz
            currentlyEditUser.zerujDane();
            this.Close();
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

  


    }
}
