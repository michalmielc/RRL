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
    public partial class oknoEditFillWithdraw : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        public oknoEditFillWithdraw()
        {
            InitializeComponent();
            label2.Text = currentlyItem.ItemId.ToString();
            label3.Text = currentlyItem.ItemName1.ToString();
            label4.Text = currentlyItem.ItemName2.ToString();
            label5.Text = currentlyItem.ItemName3.ToString();
            label7.Text = currentlyItem.Barcode.ToString();
            label8.Text = currentlyItem.amount.ToString();

            currentlyItem.PicPath = db.readItempicpath(int.Parse(currentlyItem.ItemId.ToString()));

            db.loadCostCenters_combobox(comboBox1);


            if (currentlyItem.PicPath !="" && File.Exists(currentlyItem.PicPath))
            {
                pictureBox1.Image = Image.FromFile(currentlyItem.PicPath);
            }

    

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Yellow;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Transparent;
        }

        private void oknoEditFillWithdraw_Load(object sender, EventArgs e)
        {
            numericUpDown1.Cursor = Cursors.Arrow;

            // USTAWIENIE CZY MA SIĘ POJAWIAĆ CENTRUM KOSZTÓW

            comboBox1.Visible = false;
            //ustawienie czy można edytować

            if (currentlyCostCenter.activeedit)
            { comboBox1.DropDownStyle = ComboBoxStyle.DropDownList; }

            if (!currentlyCostCenter.activeedit)
            { comboBox1.DropDownStyle = ComboBoxStyle.DropDown; }

            if ( currentlyItem.fill)
            {
                if (currentlyCostCenter.activefill)
                { comboBox1.Visible = true;               
                }

            }

            if( currentlyItem.withdraw )

            {
                if (currentlyCostCenter.activewithdraw)
                { comboBox1.Visible = true;

                }
            }


            // USTAWIENIE MAKSYMALNEJ ILOŚCI DO POBRANIA

            if (currentlyItem.withdraw==true)
            {   numericUpDown1.Maximum = int.Parse(label8.Text);
                label1.Text = "DEKLARACJA ILOŚCI DO POBRANIA";

            }

            // USTAWIENIE NOWEJ ILOŚCI PRZY INWENTARYZACJI

            else  if (currentlyItem.stocktaking==true)
            {
                numericUpDown1.Maximum = db.getMaxAmountToFill(currentlyItem.ItemId) + int.Parse(label8.Text);
                label16.Text = db.getMaxAmountToFill(currentlyItem.ItemId).ToString();
                label16.Visible = true;
                label15.Visible = true;
                label6.Text = "NOWY STAN:";
                label1.Text = "INWENTARYZACJA ARTYKUŁU";
                label1.ForeColor = Color.Red;

            }

            // USTAWIENIE MAKS ILOŚCI DO UZUPEŁNIENIA

            else if (currentlyItem.fill== true)
            {
                numericUpDown1.Maximum = db.getMaxAmountToFill(currentlyItem.ItemId);
                label16.Text= numericUpDown1.Maximum.ToString();
                label15.Visible = true;
                label16.Visible = true;
                label1.Text = "DEKLARACJA ILOŚCI DO UZUPEŁNIENIA";



            }

       
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            string costcenter="brak";

            if (comboBox1.SelectedIndex == -1 && comboBox1.Visible==true)
            {
                MessageBox.Show("WYBIERZ CENTRUM KOSZTÓW!!");
                return;
            }

            this.Close();
            numericValidation();
            if(numericUpDown1.Value==0)
            {
                return;
            }


            if (comboBox1.SelectedItem !=null)
            {
                costcenter = comboBox1.SelectedItem.ToString();

            }
            
            //uzupełnianie - przyjęcie na stan

            if (currentlyItem.fill)
            {
                db.changeItemamount(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()),true);
                db.changeItemamountStorageplace(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()), true);
                db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, currentlyItem.amount, "",costcenter, "UZUPEŁNIANIE");
            }

            //pobranie- zdjęcie ze stanu
            if (currentlyItem.withdraw)
            {
                db.changeItemamount(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()),false);
                db.changeItemamountStorageplace(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()), false);
                db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, currentlyItem.amount, "", costcenter, "POBRANIE");
            }


            //INWENTARYZACJA

            if (currentlyItem.stocktaking)
            { 
                db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, currentlyItem.amount, "", costcenter, "KOREKTA");


            }


            oknoWarehouse oknoWarehouse = new oknoWarehouse(true);
            oknoWarehouse.ShowDialog();
        }
     
        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            numericValidation();
        }

         private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            numericValidation();
        }

        private void numericUpDown1_MouseDown(object sender, MouseEventArgs e)
        {
            numericValidation();
        }
        void numericValidation()
        {
          
            if (string.IsNullOrWhiteSpace(numericUpDown1.Value.ToString()))
            {
                numericUpDown1.Value = 0;
                return;
            }

            if (currentlyItem.withdraw == true)
            {
                numericUpDown1.Maximum = int.Parse(label8.Text);

                if (numericUpDown1.Value > numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum;

                }
            }

            if (currentlyItem.fill == true)
            {
                numericUpDown1.Maximum = int.Parse(label16.Text);

                if (numericUpDown1.Value > numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum;

                }
            }

            if (currentlyItem.stocktaking == true)
            {
                numericUpDown1.Maximum = int.Parse(label8.Text) + int.Parse(label16.Text);

                if (numericUpDown1.Value > numericUpDown1.Maximum)
                {
                    numericUpDown1.Value = numericUpDown1.Maximum;

                }
            }
        }

     
        private void numericUpDown1_MouseUp(object sender, MouseEventArgs e)
        {
            numericUpDown1.Cursor = Cursors.Default;
        }
    }
}
