using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RRL
{
    public partial class editStorageplace : Form
    {

        polaczenieBazaDanych db = new polaczenieBazaDanych();
        public editStorageplace()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ( dataGridView2.RowCount>0)
            {
                currentlyEditCubby.isempty = false;
            }
            this.Close();
        }

        private void editStorageplace_Load(object sender, EventArgs e)
        {
            db.loadItems(dataGridView1);
            db.loadStorageplaces(dataGridView2, currentlyEditCubby.Id);
            label5.Text = currentlyEditCubby.Id +"  "+  currentlyEditCubby.Name;
               
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

        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.RowCount!=0)
            {

                wczytajDanezDGV(dataGridView1);

                string tekst = currentlyItem.ItemName1 + " | " + currentlyItem.ItemName2 + " | " + currentlyItem.ItemName3;
                db.addStoraplace(currentlyEditCubby.Id, currentlyEditCubby.Name, textBox3.Text, currentlyItem.ItemId, tekst, int.Parse(textBox2.Text));
                db.loadItems(dataGridView1);
                db.loadStorageplaces(dataGridView2, currentlyEditCubby.Id); }
        }
        			
		public static void wczytajDanezDGV(DataGridView dgv)
        {


            currentlyItem.ItemId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyItem.ItemName1 = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
            currentlyItem.ItemName2 = dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString();
            currentlyItem.ItemName3 = dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.ToString();
           
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView2.Rows.Count==0)
            {
                return;
            }
            
            DialogResult dialorgResult = MessageBox.Show("Czy usunąć miejsce magazynowe?" + textBox1.Text, "USUWANIE MIEJSCA MAGAZYNOWEGO", MessageBoxButtons.YesNo);

            if (dialorgResult == DialogResult.Yes)
            {

                int id = int.Parse(dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[0].Value.ToString());
                db.delStorageplace(id);
                db.loadItems(dataGridView1);
                db.loadStorageplaces(dataGridView2, currentlyEditCubby.Id);
            }

            else
            {
                return;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            int k;

            if (!int.TryParse(textBox2.Text.ToString(), out k))
            {
                textBox2.Text = "0";
                    }

 
            
        }
    }
}
