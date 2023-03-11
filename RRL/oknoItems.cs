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
    public partial class oknoItems : Form
    {
        bool first = false;

    

        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public oknoItems()
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

        private void oknoItems_Load(object sender, EventArgs e)
        {
            db.loadItems(dataGridView1);
        

            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }

            wczytajDanezDGV_start(dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox1.Text == "")
            {
                db.loadItems_all_on_text(dataGridView1, "", "wszystkie");
             
                return;
            }

            else
            {
                db.loadItems_all_on_text(dataGridView1, textBox1.Text, "po_nazwach");
          
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {


          if (dataGridView1.RowCount <= 0)
            {
                return;
            }

            if (!first)
            {
                wczytajDanezDGV_start(dataGridView1);

                if (currentlyItem.amount == 0)
                {

                    DialogResult dialorgResult = MessageBox.Show("Czy usunąć artykuł ?" + textBox1.Text, "USUWANIE artykułu", MessageBoxButtons.YesNo);

                    if (dialorgResult == DialogResult.Yes)
                    {

                        currentlyItem.ItemId = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
                        db.delItem(currentlyItem.ItemId);
                        db.loadItems(dataGridView1);
                    }

                }
            }

            else
            {
                wczytajDanezDGV(dataGridView1);

                if (currentlyItem.amount == 0)
                {

                    DialogResult dialorgResult = MessageBox.Show("Czy usunąć artykuł ?" + textBox1.Text, "USUWANIE artykułu", MessageBoxButtons.YesNo);

                    if (dialorgResult == DialogResult.Yes)
                    {

                        currentlyItem.ItemId = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
                        db.delItem(currentlyItem.ItemId);
                        db.loadItems(dataGridView1);
                    }

                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            currentlyItem.add = true;
            currentlyItem.edit = false;

            editItem editItem = new editItem();
            editItem.ShowDialog();
            db.loadItems(dataGridView1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            int item_index=0;
            if (dataGridView1.RowCount > 0)
            {
            
                if (!first)
                {
                    dataGridView1.Rows[0].Selected = true;
                }

                 item_index = dataGridView1.CurrentRow.Index;

            }

            else
            {
                return;
            }

            if (!first)
            {
                wczytajDanezDGV_start(dataGridView1);
                currentlyItem.add = false;
                currentlyItem.edit = true;

                editItem editItem = new editItem();
                editItem.ShowDialog();
                db.loadItems(dataGridView1);

            }

                else
            {
                wczytajDanezDGV(dataGridView1);
                currentlyItem.add = false;
                currentlyItem.edit = true;

                editItem editItem = new editItem();
                editItem.ShowDialog();
                db.loadItems(dataGridView1);
            }

            //zaznaczenie ostatnio edytowanego wiersza

            if (dataGridView1.RowCount > 0)
            {
                dataGridView1.Rows[item_index].Selected = true;
            }
        }

        public static void wczytajDanezDGV(DataGridView dgv)
        {


            currentlyItem.ItemId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyItem.ItemName1 = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
            currentlyItem.ItemName2 = dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString();
            currentlyItem.ItemName3 = dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.ToString();
            currentlyItem.Barcode= dgv.Rows[dgv.CurrentRow.Index].Cells[4].Value.ToString();
            currentlyItem.PicPath = dgv.Rows[dgv.CurrentRow.Index].Cells[5].Value.ToString();
           
   


            bool x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[6].Value.ToString());
            if (x)
            {
                currentlyItem.Active = 1;
            }

            else
            {
                currentlyItem.Active = 0;

            }

            currentlyItem.Price = decimal.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[7].Value.ToString());
            currentlyItem.MinInventory = int.Parse( dgv.Rows[dgv.CurrentRow.Index].Cells[8].Value.ToString());
            currentlyItem.amount = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[9].Value.ToString());
            currentlyItem.Supplier = dgv.Rows[dgv.CurrentRow.Index].Cells[10].Value.ToString();

        }

        public static void wczytajDanezDGV_start(DataGridView dgv)
        {

            if ( dgv.RowCount==0 )
            {
                return;
            }
            dgv.Rows[0].Selected = true;

            currentlyItem.ItemId = int.Parse(dgv.Rows[0].Cells[0].Value.ToString());
            currentlyItem.ItemName1 = dgv.Rows[0].Cells[1].Value.ToString();
            currentlyItem.ItemName2 = dgv.Rows[0].Cells[2].Value.ToString();
            currentlyItem.ItemName3 = dgv.Rows[0].Cells[3].Value.ToString();
            currentlyItem.Barcode = dgv.Rows[0].Cells[4].Value.ToString();
            currentlyItem.PicPath = dgv.Rows[0].Cells[5].Value.ToString();




            bool x = bool.Parse(dgv.Rows[0].Cells[6].Value.ToString());
            if (x)
            {
                currentlyItem.Active = 1;
            }

            else
            {
                currentlyItem.Active = 0;

            }

            currentlyItem.Price = decimal.Parse(dgv.Rows[0].Cells[7].Value.ToString());
            currentlyItem.MinInventory = int.Parse(dgv.Rows[0].Cells[8].Value.ToString());

            currentlyItem.amount = int.Parse(dgv.Rows[0].Cells[9].Value.ToString());

         
            currentlyItem.Supplier = dgv.Rows[0].Cells[10].Value.ToString();

        }
   

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

      
            first = true;

        }
    }
}
