using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace RRL
{
    public partial class oknoUsers : Form
    {
        bool first = false; // kliknięcie tabeli 
        polaczenieBazaDanych db = new polaczenieBazaDanych();
        public oknoUsers()
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

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Yellow;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Transparent;
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            pictureBox5.BackColor = Color.Yellow;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            currentlyEditUser.add = true;
            currentlyEditUser.edit = false;

            editUser editUser = new editUser();
            editUser.ShowDialog();
            db.loadUsers(dataGridView1);
        }

        private void oknoUsers_Load(object sender, EventArgs e)
        {
            db.loadUsers(dataGridView1);
            ukryjKolumnyUsers();
            wczytajdaneFirst(dataGridView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;

            if (textBox1.Text == "")
            {
                db.loadUsers_all_on_text(dataGridView1, "", "wszystkie");
                ukryjKolumnyUsers();
                return;
            }

            else
            {
                db.loadUsers_all_on_text(dataGridView1, textBox1.Text, "po_nazwach");
                ukryjKolumnyUsers();
            }
           
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count<=0)
            {
                return;
            }

               DialogResult dialorgResult = MessageBox.Show("Czy usunąć użytkownika ?" + textBox1.Text, "USUWANIE USERA",MessageBoxButtons.YesNo);
              
            if (dialorgResult == DialogResult.Yes)
               {

                   currentlyEditUser.UserId = int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
                   db.delUser(currentlyEditUser.UserId);
                   db.loadUsers(dataGridView1);
               }

               else
               {
                   return;
               }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            int item_index = 0;

            if (dataGridView1.Rows.Count <= 0 )
            {
                return;

            }

            if (!first)

            {
                wczytajdaneFirst(dataGridView1);
                currentlyEditUser.add = false;
                currentlyEditUser.edit = true;
                editUser editUser = new editUser();
                editUser.ShowDialog();
                db.loadUsers(dataGridView1);
            }

            else
            {

                // PRZEKAZANIE PARAMTERÓW EDYTOWANEGO USERA

                item_index = dataGridView1.CurrentRow.Index;

                wczytajDanezDGV(dataGridView1);
                currentlyEditUser.add = false;
                currentlyEditUser.edit = true;
                editUser editUser = new editUser();
                editUser.ShowDialog();
                db.loadUsers(dataGridView1);

                dataGridView1.Rows[item_index].Selected = true;
            }

        }

        public static void wczytajDanezDGV(DataGridView dgv)
        {

            currentlyEditUser.UserId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            currentlyEditUser.UserName = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
            currentlyEditUser.UserGroupName = dgv.Rows[dgv.CurrentRow.Index].Cells[2].Value.ToString();
            currentlyEditUser.Password = dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.ToString();
            currentlyEditUser.UserDepartment = dgv.Rows[dgv.CurrentRow.Index].Cells[4].Value.ToString();
            bool x = bool.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[5].Value.ToString());
            if(x)
            {
                currentlyEditUser.Active = 1;
            }
            
            else
            {
                currentlyEditUser.Active = 0;
            
            }
            currentlyEditUser.UserGroupId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[6].Value.ToString());
            
    
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ukryjKolumnyUsers();
            first = true;
        }

        void ukryjKolumnyUsers()
        {
        //    dataGridView1.Columns[0].Visible = false;
        //    dataGridView1.Columns[6].Visible = false;

        }

         void wczytajdaneFirst(DataGridView dgv)
        {
            if (dgv.RowCount==0)
            {
                return;
            }
            currentlyEditUser.UserId = int.Parse(dgv.Rows[0].Cells[0].Value.ToString());
            currentlyEditUser.UserName = dgv.Rows[0].Cells[1].Value.ToString();
            currentlyEditUser.UserGroupName = dgv.Rows[0].Cells[2].Value.ToString();
            currentlyEditUser.Password = dgv.Rows[0].Cells[3].Value.ToString();
            currentlyEditUser.UserDepartment = dgv.Rows[0].Cells[4].Value.ToString();
            bool x = bool.Parse(dgv.Rows[0].Cells[5].Value.ToString());
            if (x)
            {
                currentlyEditUser.Active = 1;
            }

            else
            {
                currentlyEditUser.Active = 0;

            }
            currentlyEditUser.UserGroupId = int.Parse(dgv.Rows[0].Cells[6].Value.ToString());
        }
    }
}
