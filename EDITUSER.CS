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
    public partial class editUser : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public editUser()
        {
            InitializeComponent();
        }

        private void editUser_Load(object sender, EventArgs e)
        {
   

            if (currentlyEditUser.edit)
            {
            

                label6.Text = currentlyEditUser.UserId.ToString();
                textBox1.Text = currentlyEditUser.UserName;
                textBox2.Text = currentlyEditUser.UserDepartment;
                textBox3.Text = currentlyEditUser.Password;
                db.loadUsergroups_combobox(comboBox1);
                comboBox1.Text = currentlyEditUser.UserGroupName;

                if (currentlyEditUser.Active==1)
                {
                    checkBox1.Checked = true;
                }


            }

            if (currentlyEditUser.add)
            {
                label1.Text = "";
                label6.Text = "";

                db.loadUsergroups_combobox(comboBox1);
                
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // OBSŁUGA BŁĘDÓW USERGROUPS

            if ( comboBox1.Items.Count==0)
            {
                MessageBox.Show("brak grup użytkowników.Nie można załozyć użytkownika!");
                return;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("wybierz grupę uzytkownika!");
                return;

            }

            // SPRAWDZENIE POLA HASŁO
            
            if (textBox3.Text == null)
            {
                return;

            }


            string haslo = textBox3.Text.ToString();


            int dlug = int.Parse(haslo.Length.ToString());


            if (dlug<=3)
            {
                MessageBox.Show("Zbyt krótkie hasło!");
                return;
            }

            if (currentlyEditUser.add)
            {
                if (db.checkUserPassword2(textBox3.Text))
                {
                    textBox3.Text = "";
                    MessageBox.Show("Hasło Istnieje");
                    return;
                }
            }

            if (currentlyEditUser.edit)
            {
                if (db.checkUserPassword3(textBox3.Text, currentlyEditUser.UserId))
                {
                    textBox3.Text = "";
                    MessageBox.Show("Hasło Istnieje");

                    return;
                }
            }

            if (currentlyEditUser.edit)
                {

                    db.editUser(currentlyEditUser.UserId, textBox1.Text, textBox2.Text, currentlyEditUser.UserGroupId, textBox3.Text, currentlyEditUser.Active);
                    this.Close();
                    //ustaw ostanio modyfikowany wiersz
                    currentlyEditUser.zerujDane();
                }

                if (currentlyEditUser.add)
                {
                    db.addUser(textBox1.Text, textBox2.Text, currentlyEditUser.UserGroupId, textBox3.Text, currentlyEditUser.Active);
                    this.Close();
                    //ustaw ostanio modyfikowany wiersz
                    currentlyEditUser.zerujDane();
                }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

       

                if (checkBox1.Checked)
                {
                    currentlyEditUser.Active = 1;
                }

                else
                {

                    currentlyEditUser.Active = 0;
                }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            //ustaw ostanio modyfikowany wiersz
            currentlyEditUser.zerujDane();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentlyEditUser.UserGroupId = db.checkUsergroupId(comboBox1.SelectedItem.ToString());
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if(db.checkUserPassword2(textBox3.Text))
            {
                textBox3.Text = "";
                return;
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

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            if (currentlyEditUser.add)
            {
                if (db.checkUserPassword2(textBox3.Text))
                {
                    textBox3.Text = "";
                    return;
                }
            }
        }
    }
}
