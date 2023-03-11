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
    public partial class oknoReports : Form
    {


        polaczenieBazaDanych db = new polaczenieBazaDanych();
        public oknoReports()
        {
            InitializeComponent();
            treeView1.ExpandAll();
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //RAPORT LOGOWAŃ
            if (treeView1.SelectedNode.Name == "LOGOWANIA")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadLoggedusers(dataGridView1);

            }

            //LISTA UZYTKOWNIKÓW
            if (treeView1.SelectedNode.Name == "LISTA UŻYTKOWNIKÓW")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadUsers(dataGridView1);

            }

            //LISTA ARTYKUŁÓW
            if (treeView1.SelectedNode.Name == "LISTA ARTYKUŁÓW")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadItems(dataGridView1);

            }
            // STANY MAGAZYNOWE
            if (treeView1.SelectedNode.Name == "STANY MAGAZYNOWE")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadItemsToPickOrWith(dataGridView1);

            }

            // MIEJSCA MAGAZYNOWE
            if (treeView1.SelectedNode.Name == "MIEJSCA_MAGAZYNOWE")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadStorageplaces_all(dataGridView1);

            }
            // OPERACJE MAGAZYNOWE
            //POBRANIA
            if (treeView1.SelectedNode.Name == "POBRANIA")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadWithdraws(dataGridView1);

            }

            // PRZYJĘCIA
            if (treeView1.SelectedNode.Name == "PRZYJĘCIA")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadFillings(dataGridView1);

            }

            // KOREKTY STANU
            if (treeView1.SelectedNode.Name == "KOREKTY_STANU")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadStockTaking(dataGridView1);

            }

            // WSZYSTKIE
            if (treeView1.SelectedNode.Name == "WSZYSTKIE")
            {

                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                db.loadWarehouseOperations(dataGridView1);

            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SaveToCSV(dataGridView1);
        }

        private void SaveToCSV(DataGridView DGV)
        {
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Output.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Dane zostaną wyeksportowane");
                if (File.Exists(filename))
                {
                    try
                    {
                        File.Delete(filename);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Zapis danych nie jest możliwy!" + ex.Message);
                    }
                }
                int columnCount = DGV.ColumnCount;
                string columnNames = "";
                string[] output = new string[DGV.RowCount + 1];

                for (int i = 0; i < columnCount; i++)
                {
                    columnNames += DGV.Columns[i].Name.ToString() + ";";
                }
                output[0] += columnNames;
                for (int i = 1; (i - 1) < DGV.RowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        output[i] += DGV.Rows[i - 1].Cells[j].Value.ToString() + ";";
                    }
                }
                System.IO.File.WriteAllLines(sfd.FileName, output, System.Text.Encoding.UTF8);
                MessageBox.Show("PLIK ZOSTAŁ WYEKSPORTOWANY");
            }
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Yellow;

        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Transparent;
        }
    }
}
