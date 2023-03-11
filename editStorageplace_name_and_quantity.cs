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
    public partial class editStorageplace_name_and_quantity : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public editStorageplace_name_and_quantity()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int m = 0;
            int n = 0;
   


            bool x = int.TryParse(textBox2.Text.ToString(), out m);
            bool y = int.TryParse(textBox3.Text.ToString(), out n);

            if(!x )
            {
                MessageBox.Show("NIEPRAWIDŁOWY FORMAT LICZBOWY!: " +textBox2.Text, " UWAGA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = currentlyEditStorage.CurrentIventory.ToString();

                return;
            }


            if (!y)
            {
                MessageBox.Show("NIEPRAWIDŁOWY FORMAT LICZBOWY!: "+textBox3.Text , " UWAGA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Text = currentlyEditStorage.MaxIventory.ToString(); 
       
                return;
            }

            if (m>n)

            {
                MessageBox.Show("STAN AKTUALNY NIE MOŻE BYĆ WIĘKSZY OD MAKSYMALKNEGO! ", " UWAGA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = currentlyEditStorage.CurrentIventory.ToString();
                textBox3.Text = currentlyEditStorage.MaxIventory.ToString();
                return;
            }
            int curr = int.Parse(textBox2.Text.ToString());

            // o ile zmieniana ilość szt w Storageplace
            currentlyEditStorage.DiffIventory = curr - currentlyEditStorage.CurrentIventory;

            int max = int.Parse(textBox3.Text.ToString());


            if (max < curr)
                {
                    MessageBox.Show("ILOŚĆ MAKSYMALNA NIE MOŻE BYĆ MNIEJSZA NIŻ AKTUALNA!", "BŁĄD!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    max = curr;

                }
                else
                {
                    db.editStorageplace(currentlyEditStorage.StorageId, textBox1.Text, curr, currentlyEditStorage.DiffIventory, max, currentlyEditStorage.Id);
                  db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyEditStorage.Id.ToString(), currentlyEditStorage.ItemName.ToString(),  curr- currentlyEditStorage.CurrentIventory,currentlyEditStorage.StorageName, "", "KOREKTA");

            }


  
            this.Close();
            
            currentlyEditStorage.zerujDane();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void editStorageplace_name_and_quantity_Load(object sender, EventArgs e)
        {
            label5.Text =  currentlyEditStorage.StorageId.ToString();
            textBox1.Text =  currentlyEditStorage.StorageName;
            textBox2.Text =  currentlyEditStorage.CurrentIventory.ToString();
            textBox3.Text = currentlyEditStorage.MaxIventory.ToString();
            label6.Text= currentlyEditStorage.Id.ToString();
            label8.Text = currentlyEditStorage.ItemName.ToString();

        }
    }
}
