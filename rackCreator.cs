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
    public partial class rackCreator : Form
    {

        int wysokosc_pion=1;
        int grubosc_pion=1;
        int grubosc_poziom=1;
        int szerokosc_poziom=1;
        int x=1;
        int y=1;
        int ilosc_poziomow=1;
        int ilosc_miejsc=1;
        //wektor przesunięcia poziomych belek
        int wektor_poziom = 1;
        //wektor przesunięcia pion - miejsca magazynowe
        int wektor_pion = 1;
        //wymiar sorageplaces
        int wymiar_miejsca_x = 1;
        int wymiar_miejsca_y = 1;
        double wspolczynnik_wys_storage = 0.75;

        public rackCreator()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {


                if (!dane_wejsciowe())

                {
                    MessageBox.Show("Wprowadzono wartości niedodatnie!", "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                else
                {
                    currentlyBuildingRacks.BalkWidth = int.Parse(textBox3.Text);
                    currentlyBuildingRacks.RackWidth = int.Parse(textBox1.Text);
                    currentlyBuildingRacks.RackHeight = int.Parse(textBox2.Text);
                    currentlyBuildingRacks.RackLevels = int.Parse(numericUpDown1.Value.ToString());
                    currentlyBuildingRacks.RackAmount = int.Parse(numericUpDown2.Value.ToString());
                    currentlyBuildingRacks.startPointX = int.Parse(textBox5.Text);
                    currentlyBuildingRacks.startPointY = int.Parse(textBox4.Text);
                    currentlyBuildingRacks.storageAmount = int.Parse(numericUpDown3.Value.ToString());
                    currentlyBuildingRacks.wektor_pion= wektor_pion;
                    currentlyBuildingRacks.wsp_wysokosci_storage = wspolczynnik_wys_storage;
                    currentlyBuildingRacks.balk_width_horizontal = int.Parse(textBox6.Text);
                    currentlyBuildingRacks.storageY= wymiar_miejsca_y;
                    currentlyBuildingRacks.wektor_poziom= wektor_poziom;


                  int h = currentlyBuildingRacks.RackHeight;
                    int w = currentlyBuildingRacks.BalkWidth;
                    int n = currentlyBuildingRacks.RackLevels;



                    currentlyBuildingRacks.VectorOfDividing = Math.Abs(int.Parse(((h - (n - 1) * w) / n).ToString()));

                       DialogResult dg = MessageBox.Show("Czy wprowadzone dane są poprawne?", "PRZED ZATWIERDZENIEM", MessageBoxButtons.YesNo);

                    if (dg == DialogResult.No)
                    {
                        return;

                    }

                    else

                    {
                        this.Close();
                    }
                }

            

          

        }

        private void rackCreator_Load(object sender, EventArgs e)
        {


            start(x,y, szerokosc_poziom,grubosc_pion);

        }

         //kreator przykładu

        void kreator_()
        {
            panel1.Controls.Clear();


            start(x, y, szerokosc_poziom, grubosc_pion);

            wektor_poziom = (int)(wysokosc_pion / ilosc_poziomow);

         

            wektor_pion = 5;

            if (ilosc_miejsc != 0)
            {
                wektor_pion = (int)((szerokosc_poziom - 10 - (ilosc_miejsc - 1) * 5) / ilosc_miejsc);
            }



            for (int i = 1; i < ilosc_poziomow; i++)
            {

                kreator_poziomow(i * wektor_poziom);

            }


            // wymiar storageplace
            wymiar_miejsca_x = wektor_pion;
            wymiar_miejsca_y = (int)(wspolczynnik_wys_storage * wysokosc_pion / ilosc_poziomow);

            for (int l = 1; l < ilosc_poziomow; l++)

            {
                for (int j = 1; j <= ilosc_miejsc; j++)

                {

                    kreator_miejsc(x+5 +grubosc_pion+(j-1)*(wektor_pion+5),  y+ l * wektor_poziom-wymiar_miejsca_y);

                }

           }

            if (ilosc_miejsc != 0)
            {
                for (int j = 1; j <= ilosc_miejsc; j++)

                {
                    kreator_miejsc(x + 5 + grubosc_pion + (j - 1) * (wektor_pion + 5), wysokosc_pion + y - wymiar_miejsca_y);
                }
            }



        }

        void kreator_poziomow(int k)

        {


            storageplace poziom = new storageplace();


            poziom.BackColor = Color.Red;

            poziom.Location = new Point(x+grubosc_pion, y+k);


            poziom.Size = new Size(szerokosc_poziom, grubosc_poziom);

            panel1.Controls.Add(poziom);



        }

        void kreator_miejsc(int m, int n)

        {

            // wymiar pudełka 0.8 wysokości poziomu oraz +10 od lewej  i -10 od prawej nogi regału

           
            storageplace poziom = new storageplace();


            poziom.BackColor = Color.Yellow;

            poziom.Location = new Point(m,n);

           
            poziom.Size = new Size(wymiar_miejsca_x, wymiar_miejsca_y);

            panel1.Controls.Add(poziom);



        }

        //początek lewa i prawa noga

        void start( int x1 , int y1, int x_dl, int y_gr)

        {

            storageplace lewanoga = new storageplace();
            storageplace prawanoga = new storageplace();

            lewanoga.BackColor = Color.Red;
            prawanoga.BackColor = Color.Red;

            lewanoga.Location = new Point(x, y);
            prawanoga.Location = new Point(x+szerokosc_poziom+grubosc_pion, y);


            // rozmiary kontrolki

            lewanoga.Size = new Size(grubosc_pion, wysokosc_pion);
            prawanoga.Size = new Size(grubosc_pion, wysokosc_pion);


            // dodanie na ekranie
            panel1.Controls.Add(lewanoga);
            panel1.Controls.Add(prawanoga);



        }

       
        //FUNCJA SPRAWDZAJĄCA DANE WEJŚCIOWE

        bool dane_wejsciowe()
        {
            bool wynik = true;
            int number;

           bool success = int.TryParse(textBox3.Text.ToString(), out number) && number>0;

            
            if (!success)
            {
                wynik = false;
                textBox3.BackColor = Color.Red;
                return wynik;
            }

            textBox3.BackColor = Color.White;

            grubosc_pion = int.Parse(textBox3.Text.ToString());


            success = int.TryParse(textBox6.Text.ToString(), out number) && number > 0;

            if (!success)
            {
                wynik = false;
                textBox6.BackColor = Color.Red;
                return wynik;
            }

            textBox6.BackColor = Color.White;

            grubosc_poziom= int.Parse(textBox6.Text.ToString());


            success = int.TryParse(textBox1.Text.ToString(), out number) && number > 0;

            if (!success)
            {
                wynik = false;
                textBox1.BackColor = Color.Red;
                return wynik;
            }


            textBox1.BackColor = Color.White;

            szerokosc_poziom = int.Parse(textBox1.Text.ToString());

            success = int.TryParse(textBox2.Text.ToString(), out number) && number > 0;

            if (!success)
            {
                wynik = false;
                textBox2.BackColor = Color.Red;
                return wynik;
            }


            textBox2.BackColor = Color.White;

            wysokosc_pion = int.Parse(textBox2.Text.ToString());


            success = int.TryParse(textBox5.Text.ToString(), out number) && number > 0;

            if (!success)
            {
                wynik = false;
                textBox5.BackColor = Color.Red;
                return wynik;
            }


            textBox5.BackColor = Color.White;

            x = int.Parse(textBox5.Text.ToString());


            success = int.TryParse(textBox4.Text.ToString(), out number) && number > 0;

            if (!success)
            {
                wynik = false;
                textBox4.BackColor = Color.Red;
                return wynik;
            }


            textBox4.BackColor = Color.White;

            y = int.Parse(textBox4.Text.ToString());

            
            success = int.TryParse(numericUpDown1.Value.ToString(), out number);

            if (!success)
            {
                wynik = false;
                numericUpDown1.BackColor = Color.Red;
                return wynik;
            }

          

            numericUpDown1.BackColor = Color.White;

            ilosc_poziomow = int.Parse(numericUpDown1.Value.ToString());
            

            success = int.TryParse(numericUpDown3.Value.ToString(), out number);

            if (!success)
            {
                wynik = false;
                numericUpDown3.BackColor = Color.Red;
                return wynik;
            }


            numericUpDown3.BackColor = Color.White;


            ilosc_miejsc= int.Parse(numericUpDown3.Value.ToString());

            return wynik;

        }


           private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = true;

            if (dane_wejsciowe())
            {
                kreator_();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = 25.ToString();
            textBox6.Text = 15.ToString();
            textBox1.Text = 300.ToString();
            textBox2.Text = 350.ToString();
            textBox5.Text = 50.ToString();
            textBox4.Text = 175.ToString();

            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 1;

        }
    }
}
