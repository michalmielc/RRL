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
    public partial class editUserGroup : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();
        byte x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11;



        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            if ( radioButton18.Checked)
            {
                groupBox1.Visible = true;

            }

            else
            {
                groupBox1.Visible = false;

            }
        }

        public editUserGroup()
        {
            InitializeComponent();
        }

        private void editUserGroup_Load(object sender, EventArgs e)
        {
            if (currentlyEditUserGroup.edit)
            {
                label6.Text = currentlyEditUserGroup.UserGroupId.ToString();
                textBox1.Text = currentlyEditUserGroup.UserGroupName.ToString();

                 
                if (currentlyEditUserGroup.x1==1)
                {
                    radioButton18.Checked=true;
                }

                else
                {
                    radioButton18.Checked = false;
                    radioButton17.Checked = true;
                    groupBox1.Visible = false;
                }

                if (currentlyEditUserGroup.x2==1)
                {
                    radioButton16.Checked = true;
                }
                else
                {
                    radioButton16.Checked = false;
                    radioButton15.Checked = true;
                }

                if (currentlyEditUserGroup.x3==1)
                {
                    radioButton14.Checked = true;
                }
                else
                {
                    radioButton14.Checked = false;
                    radioButton13.Checked = true;
                }


                if (currentlyEditUserGroup.x4==1)
                {
                    radioButton1.Checked = true;
                }

                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }

                if (currentlyEditUserGroup.x5==1)
                {
                    radioButton4.Checked = true;
                }

                else
                {
                    radioButton4.Checked = false;
                    radioButton3.Checked = true;
                }

                if (currentlyEditUserGroup.x6==1)
                {
                    radioButton8.Checked = true;
                }

                else
                {
                    radioButton8.Checked = false;
                    radioButton7.Checked = true;
                }
                if (currentlyEditUserGroup.x7==1)
                {
                    radioButton10.Checked = true;
                }

                else
                {
                    radioButton10.Checked = false;
                    radioButton9.Checked = true;
                }


                if (currentlyEditUserGroup.x8==1)
                {
                    radioButton6.Checked = true;
                }
                else
                {
                    radioButton6.Checked = false;
                    radioButton5.Checked = true;
                }

                if (currentlyEditUserGroup.x9 == 1)
                {
                    radioButton11.Checked = true;
                }
                else
                {
                    radioButton11.Checked = false;
                    radioButton12.Checked = true;
                }

                if (currentlyEditUserGroup.x10 == 1)
                {
                    radioButton19.Checked = true;
                }
                else
                {
                    radioButton19.Checked = false;
                    radioButton20.Checked = true;
                }
                if (currentlyEditUserGroup.x11 == 1)
                {
                    radioButton21.Checked = true;
                }
                else
                {
                    radioButton21.Checked = false;
                    radioButton22.Checked = true;
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            x1 = statusRights(radioButton18);
            x2 = statusRights(radioButton16);
            x3 = statusRights(radioButton14);
            x4 = statusRights(radioButton1);
            x5 = statusRights(radioButton4);
            x6 = statusRights(radioButton8);
            x7 = statusRights(radioButton10);
            x8 = statusRights(radioButton6);
            x9 = statusRights(radioButton11);
            x10 = statusRights(radioButton19);
            x11 = statusRights(radioButton21);

            if (currentlyEditUserGroup.add)
            {

                // DODAWANIE GRUPY UŻYTKOWNIKA 

    
                db.addUsergroup(textBox1.Text, x1, x2, x3, x4, x5, x6, x7, x8,x9,x10,x11);
                this.Close();

            }

            // EDYCJA GRUPY UŻYTKOWNIKA 

            if (currentlyEditUserGroup.edit)
            {
   
                db.editUsergroup(int.Parse(label6.Text), textBox1.Text, x1, x2, x3, x4, x5, x6, x7, x8,x9, x10, x11);
                this.Close();
            }

            currentlyEditUserGroup.zerujDane();
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
            //ustaw ostanio modyfikowany wiersz
             currentlyEditUserGroup.zerujDane();
        }


        //SPRAWDŹ STATUS UPRAWNIEŃ 

        byte statusRights(RadioButton rb)
        {
            byte x = 0;

            if (rb.Checked)
            {
                x = 1;
            }

            else
            {
                x = 0;
            }


            return x;
        }

    }
}
