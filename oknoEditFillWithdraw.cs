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
                pictureBox5.Visible = false;

                if (currentlyCostCenter.activefill)
                { comboBox1.Visible = true;
                       
                }

            }

            if( currentlyItem.withdraw )

            {
                pictureBox5.Visible = false;

                if (currentlyCostCenter.activewithdraw)
                {
                    comboBox1.Visible = true;

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
                //było:
                // numericUpDown1.Maximum = db.getMaxAmountToFill(currentlyItem.ItemId) + int.Parse(label8.Text);
                //label16.Text = db.getMaxAmountToFill(currentlyItem.ItemId).ToString();
                numericUpDown1.Maximum = 0;
        
                label16.Visible = false;
                label15.Visible = false;
                label17.Visible = true;
                label17.Text= "ABY ZMIENIĆ NA INNY STAN NIŻ 0 WEJDŹ DO MAGAZYNU->";

                label6.Text = "WYZEROWANIE STANU:";
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

                if (numericUpDown1.Maximum == 0)
                {
                    label17.Visible = true;
                }
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
            if(numericUpDown1.Value==0 && !currentlyItem.stocktaking)
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
                db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, int.Parse(numericUpDown1.Value.ToString()), "",costcenter, "UZUPEŁNIANIE");
            }

            //pobranie- zdjęcie ze stanu
            if (currentlyItem.withdraw)
            {
                db.changeItemamount(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()),false);
                db.changeItemamountStorageplace(currentlyItem.ItemId, int.Parse(numericUpDown1.Value.ToString()), false);
                db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, (-1)*int.Parse(numericUpDown1.Value.ToString()), "", costcenter, "POBRANIE");
            }


            //INWENTARYZACJA

            if (currentlyItem.stocktaking)
            {
                DialogResult dialogResult = MessageBox.Show("OPERACJA WYZEROWANIA STANU ARTYKUŁU" + currentlyItem.ItemId + " " + currentlyItem.ItemName1, "Wykasowanie stanów", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //zapis operacji wyzerowania
                    db.changeItemamounttozero(currentlyItem.ItemId);
                    db.changeItemamountStorageplacetozero(currentlyItem.ItemId);
                    db.addwarehouseoperations(currentlyData.UserName, currentlyData.UserDepartment, currentlyItem.ItemId.ToString(), currentlyItem.ItemName1, (-1)*currentlyItem.amount, "", costcenter, "KOREKTA");
                    return;
                }

                if (dialogResult == DialogResult.No)
                {
                    return;
                }

  
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


        }

     
        private void numericUpDown1_MouseUp(object sender, MouseEventArgs e)
        {
            numericUpDown1.Cursor = Cursors.Default;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();

            oknoWarehouse oknoWarehouse = new oknoWarehouse(false);
            oknoWarehouse.ShowDialog();
        }
    }
}
