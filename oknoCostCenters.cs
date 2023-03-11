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
    public partial class oknoCostCenters : Form
    {

        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public oknoCostCenters()
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

        private void oknoCostCenters_Load(object sender, EventArgs e)
        {
            db.loadCostCenter(dataGridView1);

            if (currentlyCostCenter.activewithdraw)
            {
                checkBox1.Checked = true;
            }

            if (currentlyCostCenter.activefill)
            {
                checkBox2.Checked = true;
            }

            if (currentlyCostCenter.activeedit)
            {
                checkBox3.Checked = true;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox1.Text == "")
            {
                db.loadCostCenter_all_on_text(dataGridView1, "", "wszystkie");
              
                return;
            }

            else
            {
                db.loadCostCenter_all_on_text(dataGridView1, textBox1.Text, "po_nazwach");
            
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
        
            if (dataGridView1.Rows.Count<=0)
            {
                return;

            }

            DialogResult dialorgResult = MessageBox.Show("Czy usunąć MPK ?" + textBox1.Text, "USUWANIE MPK", MessageBoxButtons.YesNo);

            if (dialorgResult == DialogResult.Yes)
            {

                currentlyCostCenter.CostId = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
                db.delCostCenter(currentlyCostCenter.CostId);
                db.loadCostCenter(dataGridView1);
                
            }

            else
            {
                return;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            currentlyCostCenter.add = true;
            currentlyCostCenter.edit = false;
            editCostCenter editCostCenter = new editCostCenter();
            editCostCenter.ShowDialog();
            db.loadCostCenter(dataGridView1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

            int item_index = 0;

            if (dataGridView1.Rows.Count <= 0)
            {
                return;

            }

            else
            {


                item_index = dataGridView1.CurrentRow.Index;



                wczytajDanezDGV(dataGridView1);
                currentlyCostCenter.add = false;
                currentlyCostCenter.edit = true;

                editCostCenter editCostCenter = new editCostCenter();
                editCostCenter.ShowDialog();
             }

            db.loadCostCenter(dataGridView1);
            dataGridView1.Rows[item_index].Selected = true;
        }

        public static void wczytajDanezDGV(DataGridView dgv)
        {

           currentlyCostCenter.CostId= int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyCostCenter.CostName= dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.editCostCentersOptions(currentlyCostCenter.activefill,currentlyCostCenter.activewithdraw,currentlyCostCenter.activeedit);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                currentlyCostCenter.activewithdraw = true;
            }

            else
            {

                currentlyCostCenter.activewithdraw = false;
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                currentlyCostCenter.activefill = true;
            }

            else
            {

                currentlyCostCenter.activefill = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                currentlyCostCenter.activeedit = true;
            }

            else
            {

                currentlyCostCenter.activeedit = false;
            }
        }
    }
}
