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
    public partial class oknoSupplier : Form
    {

   
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public oknoSupplier()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
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

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Yellow;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Transparent;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Transparent;
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Yellow;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Yellow;
        }

        private void oknoSupplier_Load(object sender, EventArgs e)
        {
            db.loadSuppliers(dataGridView1);
         
            wczytajDanezDGV(dataGridView1);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox1.Text == "")
            {

                db.loadSuppliers_all_on_text(dataGridView1,"", "wszystkie");
             
                return;
            }

            else
            {
                db.loadSuppliers_all_on_text(dataGridView1, textBox1.Text, "po_nazwach");
           
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
    

            DialogResult dialorgResult = MessageBox.Show("Czy usunąć dostawcę ?" + textBox1.Text, "USUWANIE dostawcy", MessageBoxButtons.YesNo);

            if (dialorgResult == DialogResult.Yes)
            {

                currentlyEditSupplier.SupplierId = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
               
                db.delSupplier(currentlyEditSupplier.SupplierId);
                db.loadSuppliers(dataGridView1);
            }

            else
            {
                return;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            currentlyEditSupplier.add = true;
            currentlyEditSupplier.edit = false;

            editSupplier editSupplier = new editSupplier();
            editSupplier.ShowDialog();
            db.loadSuppliers(dataGridView1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //EDYCJA DOSTAWCY

            int item_index = 0;

            if (dataGridView1.Rows.Count <= 0)
            {
                return;

            }

            else
            {


                item_index = dataGridView1.CurrentRow.Index;

                wczytajDanezDGV(dataGridView1);
                currentlyEditSupplier.add = false;
                currentlyEditSupplier.edit = true;

                editSupplier editSupplier = new editSupplier();
                editSupplier.ShowDialog();
             
            }

            db.loadSuppliers(dataGridView1);
            dataGridView1.Rows[item_index].Selected = true;

        }

        public static void wczytajDanezDGV(DataGridView dgv)
        {
            if (dgv.Rows.Count<=0)
            {
                return;
            }

            else
            {
                currentlyEditSupplier.SupplierId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
                currentlyEditSupplier.SupplierName = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
                currentlyEditSupplier.SupplierEmail = dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString();
              }
         }


   
    }
}
