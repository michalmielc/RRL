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
    public partial class zmianaRozmiaru : Form
    {


        public byte opcja;

        public zmianaRozmiaru(string txt, byte op)
        {
            InitializeComponent();
            label4.Text = txt;
            opcja = op;
        }


             

        private void button1_Click(object sender, EventArgs e)
        {

            // odczyt dwóch wartości
            //USTAWIENIA GLOBALNE
            int x1, y1;


            bool res1 = int.TryParse(textBox1.Text, out x1);
            bool res2 = int.TryParse(textBox2.Text, out y1);

            if (res1 == false || res2 == false)
            {
                return;

            }

            else
            {
                // USTAWIENIA DLA NOWYCH KONTROLEK

                if (opcja == 0)
                {
                    lokalizacja.x = int.Parse(textBox1.Text);
                    lokalizacja.y = int.Parse(textBox2.Text);
                }

                // USTAWIENIA TYMCZASOWE DLA ZMIENIANEJ KONTROLKI

                if (opcja == 1|| opcja==2)
                {

                    lokalizacja.xTemp = int.Parse(textBox1.Text);
                    lokalizacja.yTemp = int.Parse(textBox2.Text);
                }
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lokalizacja.xTemp = lokalizacja.x;
            lokalizacja.yTemp = lokalizacja.y;

            this.Close();
        }

        private void zmianaRozmiaru_Load(object sender, EventArgs e)
        {
            if (opcja ==2 )
            {
                label1.Text= "ZMIANA POZYCJI KONTROLKI";
                label2.Text = "POZYCJA X";
                label3.Text = "POZYCJA Y";

            }

        }
    }
   
}
