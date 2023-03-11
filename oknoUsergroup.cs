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
    public partial class oknoUsergroup : Form
    {

        polaczenieBazaDanych db = new polaczenieBazaDanych();
     

        public oknoUsergroup()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Yellow;
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

             
        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Yellow;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Transparent;
        }

        private void oknoUsergroup_Load(object sender, EventArgs e)
        {
            db.loadUsersgroup(dataGridView1);

        }

 
        // DODAWANIE GRUPY UŻYTKOWNIKA
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            currentlyEditUserGroup.add = true;
            currentlyEditUserGroup.edit = false;
            editUserGroup editUserGroup = new editUserGroup();
            editUserGroup.ShowDialog();
            db.loadUsersgroup(dataGridView1);
        }

        // EDYCJA GRUPY UŻYTKOWNIKA
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

            
                // PRZEKAZANIE PARAMTERÓW EDYTOWANEJ GRUPY UZYTKOWNIKÓW

                wczytajDanezDGV(dataGridView1);
                currentlyEditUserGroup.add = false;
                currentlyEditUserGroup.edit = true;
                editUserGroup editUserGroup = new editUserGroup();
                editUserGroup.ShowDialog();
            }
            db.loadUsersgroup(dataGridView1);

        
                dataGridView1.Rows[item_index].Selected = true;
     


        }
        public static void wczytajDanezDGV(DataGridView dgv)
        {

            currentlyEditUserGroup.UserGroupId= int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyEditUserGroup.UserGroupName = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();

            bool x = false;

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString());
            if(x)
            {
                currentlyEditUserGroup.x1 =1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x2 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[4].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x3 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[5].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x4 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[6].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x5 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[7].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x6 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[8].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x7 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[9].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x8 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[10].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x9 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[11].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x10 = 1;

            }

            x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[12].Value.ToString());
            if (x)
            {
                currentlyEditUserGroup.x11 = 1;

            }
        }
        //USUWANIE GRUPY UZYTKOWNIKA
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                return;

            }

            if (String.IsNullOrEmpty(label9.Text))
            {
                return;
            }

            else
            {
                DialogResult dialorgResult = MessageBox.Show("CZY USUNĄĆ GRUPĘ UZYTKOWNIKÓW?" + textBox1.Text,"USUWANIE GRUPY UZYTKOWNIKÓW",  MessageBoxButtons.YesNo);
                if (dialorgResult == DialogResult.Yes)
                {
                    db.delUsergroup(int.Parse(label9.Text));
                    db.loadUsersgroup(dataGridView1);
                }
                else
                {
                    return;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label9.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();

         }

      
    }
}
