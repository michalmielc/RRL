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
    public partial class editCostCenter : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public editCostCenter()
        {
            InitializeComponent();
        }

        private void editCostCenter_Load(object sender, EventArgs e)
        {
        

            if (  currentlyCostCenter.edit)
            {
            

                label6.Text = currentlyCostCenter.CostId.ToString();
                textBox1.Text = currentlyCostCenter.CostName;
         

            }

            if (currentlyCostCenter.add)
            {
                
                label6.Text = "";
                textBox1.Text = " - TU WPISZ NAZWĘ MPK  -";
                
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if ( textBox1.Text=="")
            {
                return;
            }
            
            if (currentlyCostCenter.edit)
            {

                db.editCostCenter(currentlyCostCenter.CostId, textBox1.Text);
                this.Close();
                //ustaw ostanio modyfikowany wiersz
               
            }

            if (currentlyCostCenter.add)
            {

                db.addCostcenter(textBox1.Text);
                this.Close();
                //ustaw ostanio modyfikowany wiersz
              
            }

          //  currentlyCostCenter.zerujDane();
        }

       

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            //ustaw ostanio modyfikowany wiersz
            currentlyCostCenter.zerujDane();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

  
    }
}
