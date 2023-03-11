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
    public partial class oknoFillWithdraw : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public oknoFillWithdraw()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            currentlyItem.zerujDane();
            this.Close();
        }

        private void oknoFillWithdraw_Load(object sender, EventArgs e)
        {
            db.loadItemsToPickOrWith(dataGridView1);

            if ( !currentlyItem.withdraw && !currentlyItem.stocktaking)
            {
                label1.Text = " - PRZYJĘCIE - ";

            }

             if (!currentlyItem.withdraw && !currentlyItem.fill)
            {
                label1.Text = " - INWENTARYZACJA - ";

            }

                        
            if (!currentlyItem.stocktaking && !currentlyItem.fill)
            {
                label1.Text = " - POBRANIE - ";
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox1.Text == "")
            {
                db.loadItemsToPick_all_on_text(dataGridView1, "", "wszystkie");
                return;
            }

            else
            {
                db.loadItemsToPick_all_on_text(dataGridView1, textBox1.Text, "po_nazwach");

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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            wczytajItem(dataGridView1);
            uruchomOknoFillWithdraw();

        }


        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            wczytajItem(dataGridView1);

            if (e.KeyChar == (Char)Keys.Enter)
            {
                   uruchomOknoFillWithdraw();
             }
            
         
        }

        void uruchomOknoFillWithdraw ()
        {

              if (currentlyItem.amount == 0 && currentlyItem.withdraw == true)
            {
                return;
            }

            else

            {
                oknoEditFillWithdraw oknoEditFillWithdraw = new oknoEditFillWithdraw();
                oknoEditFillWithdraw.ShowDialog();
                db.loadItemsToPickOrWith(dataGridView1);


            }

        }



        void wczytajItem(DataGridView dgv)
        {
            currentlyItem.ItemId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyItem.ItemName1 = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
            currentlyItem.ItemName2 = dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString();
            currentlyItem.ItemName3 = dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.ToString();
            currentlyItem.Barcode = dgv.Rows[dgv.CurrentRow.Index].Cells[4].Value.ToString();
            currentlyItem.amount = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[5].Value.ToString());
            

        }
    }
}