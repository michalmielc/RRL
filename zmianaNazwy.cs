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
    public partial class zmianaNazwy : Form
    {

        string nazwa;
        public zmianaNazwy(string txt)
        {
            InitializeComponent();
            nazwa = txt;
        }


        private void zmianaNazwy_Load(object sender, EventArgs e)
        {

        }

 

        private void button1_Click(object sender, EventArgs e)
        {

            balk_position.nazwaTemp = textBox1.Text;
            lokalizacja.nazwaTemp = textBox1.Text;
           
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
