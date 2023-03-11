using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;



namespace RRL
{
    public partial class oknoWarehouse : Form
    {
        polaczenieBazaDanych db = new polaczenieBazaDanych();

        // zbior klas
        public Dictionary<int, lokalizacja> zbiorKlas = new Dictionary<int, lokalizacja>();
        // punkt
        public Point firstPoint = new Point();
        //aktualnie zaznaczony obiekt
        public int idLokalizacji;

        public bool animacja;

        public oknoWarehouse(bool an)
        {
            InitializeComponent();
            animacja = an;
        }

        private void oknoWarehouse_Load(object sender, EventArgs e)
        {


            db.loadCubbies(dataGridView2);

            readCubbies();

            if (animacja == true)
            {

                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button5.Visible = false;

                button7.Visible = false;
                checkBox1.Visible = false;

                db.markedItems(dataGridView1);
                markCubbies();

                dataGridView2.Visible = false;
                textBox1.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int indeks = db.nextfreeCubbieid();

            storageplace lok1 = new storageplace();
            

            // tworzenie obiektu lokalizacja  o własciwościach klasy lokalizacja
            // lok podaje właściwości konkretnergo storageplace

            lokalizacja lok = new lokalizacja();

            //utworzenie słownika klas

            zbiorKlas.Add(indeks, lok);

            //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

            lok1.Name = indeks.ToString();
            lok.indeks = indeks;

            //przypisanie metod zaznaczenie, przesunięcie
            lok1.MouseClick += zaznaczenie;

            lok1.MouseDown += MyControl_MouseDown;

            lok1.MouseMove += MyControl_MouseMove;


            // punkt początkowy kontrolki
            lok1.Location = new Point(5, 5);

            // rozmiary kontrolki

            lok1.Size = new Size(lokalizacja.x, lokalizacja.y);

            etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(lok1);

            //wpis do bazy danych
            db.addCubby();

            //odświeżenie tabeli
            db.loadCubbies(dataGridView2);

         
        }

        // testowa funkcja etykieta na miejscu magazynowym

            void etykieta()
        {
            Label numer = new Label();
            numer.Text = "4";
            numer.Font = new Font("Arial", 12);
            numer.Location = new Point(5 + (Location.X) / 2, 5 + (Location.Y) / 2);
            //numer.Location = new Point(170, 170);
            numer.AutoSize = true;

            numer.BackColor = Color.Pink;

            panel0.Controls.Add(numer);
        }

        // ZAZNACZENIE OBIEKTU

        public void zaznaczenie(object sender, MouseEventArgs e)
        {

            if (animacja == true)
            {

                return;
            }

            else
            {
                //identyfikacja kliknietej kontrolki

                Control control = (Control)sender;
                idLokalizacji = int.Parse(control.Name);
                control.BackColor = Color.Yellow;

                //odczyt danych lokalizacji.

                czytaj_kontrolke();
                //pojawia sie menu

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();
                    

                    m.MenuItems.Add("BLOKUJ", new EventHandler(item_Click));
                    m.MenuItems.Add("ODBLOKUJ", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ ROZMIAR", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ NAZWĘ", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ POZYCJĘ", new EventHandler(item_Click));
                    m.MenuItems.Add("EDYTUJ", new EventHandler(item_Click));
                    m.MenuItems.Add("KASUJ OBIEKT", new EventHandler(item_Click));
                     m.Show(control, new Point(e.X, e.Y));

                    }

                if (!currentlyEditCubby.isempty)
                {
                    control.BackColor = Color.Cyan;
                    return;
                }

             
                    control.BackColor = Color.Pink;
                
            }
        }
        // ZMIANA KOLORU MENU TEST
        void Cm_DrawItem(object sender, DrawItemEventArgs e)
        {
            var item = (MenuItem)sender;
            var g = e.Graphics;
            var font = new Font("Arial", 10, FontStyle.Italic);
            var brush = new SolidBrush(System.Drawing.Color.Beige);
            g.DrawString(item.Text, font, brush, e.Bounds.X, e.Bounds.Y);
        }

        // KASOWANIE POJEDYNCZEJ KONTROLKI
        void usunKontrolke(string txt)

        {

            foreach (Control ctr in this.panel0.Controls)

            {

                if (ctr.Name == txt)
                {

                    panel0.Controls.Remove(ctr);
                    ctr.Dispose();
                }

            }

        }

        // ZMIEŃ KOLOR KONTROLKI
        void kolorKontrolki(string txt)

        {
            foreach (Control ctr in this.panel0.Controls)

            {

                if (ctr.Name == txt)
                {

                  //    panel0.Controls[2].BackColor = Color.Pink;

                }

            }

        }

        //ZMIANA WYMIARÓW KONTROLKI

        void zmienWymiarKontrolki(string txt)
        {

            //zmiana wymiarów w klasie

            zbiorKlas[idLokalizacji].wymiarSzer = lokalizacja.xTemp;
            zbiorKlas[idLokalizacji].wymiarWys = lokalizacja.yTemp;


            foreach (Control ctr in this.panel0.Controls)
            {

                if (ctr.Name == txt)
                {
                    ctr.Size = new Size(lokalizacja.xTemp, lokalizacja.yTemp);

                }

            }


        }


        //ZMIANA POZYCJI KONTROLKI

        void zmienPozycjeKontrolki(string txt, int a, int b)
        {


            zbiorKlas[idLokalizacji].pozX = a;
            zbiorKlas[idLokalizacji].pozY = b;


            foreach (Control ctr in this.panel0.Controls)
            {

                if (ctr.Name == txt)
                {
                    ctr.Location = new Point(a, b);

                }

            }


        }

        //ZMIANA NAZWY KONTROLKI

        void zmienNazweKontrolki(string txt)
        {

            //ZMIANA NAZWY W KLASIE

            zbiorKlas[idLokalizacji].nazwa = lokalizacja.nazwaTemp;

        }

        // CZYSĆ PLANSZĘ

        void czynscPlansze()
        {

            panel0.Controls.Clear();
            lokalizacja.nazwaTemp = "";


        }

        //ODCZYT DANYCH KONTROLKI
        void czytaj_kontrolke()
        {
            currentlyEditCubby.Id = int.Parse(zbiorKlas[idLokalizacji].indeks.ToString());
            currentlyEditCubby.Name = zbiorKlas[idLokalizacji].nazwa.ToString();
            currentlyEditCubby.PosX = int.Parse(zbiorKlas[idLokalizacji].pozX.ToString());
            currentlyEditCubby.PosY = int.Parse(zbiorKlas[idLokalizacji].pozY.ToString());
            currentlyEditCubby.Height = int.Parse(zbiorKlas[idLokalizacji].wymiarWys.ToString());
            currentlyEditCubby.Width = int.Parse(zbiorKlas[idLokalizacji].wymiarSzer.ToString());

            textBox1.Clear();
            textBox1.AppendText("nazwa: " + zbiorKlas[idLokalizacji].nazwa.ToString() + Environment.NewLine);
            textBox1.AppendText("pozX: " + zbiorKlas[idLokalizacji].pozX.ToString() + Environment.NewLine);
            textBox1.AppendText("pozY: " + zbiorKlas[idLokalizacji].pozY.ToString() + Environment.NewLine);
            textBox1.AppendText("ID: " + zbiorKlas[idLokalizacji].indeks.ToString() + Environment.NewLine);
            textBox1.AppendText("BLOKOWANY: " + zbiorKlas[idLokalizacji].blokowany.ToString() + Environment.NewLine);
            textBox1.AppendText("szer: " + zbiorKlas[idLokalizacji].wymiarSzer.ToString() + Environment.NewLine);
            textBox1.AppendText("wys: " + zbiorKlas[idLokalizacji].wymiarWys.ToString() + Environment.NewLine);
            textBox1.AppendText("ZAZNACZONY: " + zbiorKlas[idLokalizacji].zaznaczony.ToString() + Environment.NewLine);

            db.loadStorageplaces2(dataGridView1, currentlyEditCubby.Id);
        }

        //PRZESUNIĘCIE KONTROLIKI

        public void przesuniecie(object sender, MouseEventArgs e)

        {
            Control nazwa = (Control)sender;
            idLokalizacji = int.Parse(nazwa.Name);



            // JEŚLI OBIEKT JEST BLOKOWANY, TO NIE DA SIĘ PRZESUNĄĆ

            if (zbiorKlas[idLokalizacji].blokowany)
            {
                return;

            }

          
        }

        public void MyControl_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            idLokalizacji = int.Parse(control.Name);



            control.MouseDown += (ss, ee) =>
            {


                if (zbiorKlas[idLokalizacji].blokowany)
                {
                    return;
                }

                else if (ee.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    firstPoint = Control.MousePosition;
                }

            };

        }

        public void MyControl_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            idLokalizacji = int.Parse(control.Name);


            control.MouseMove += (ss, ee) =>
            {

                if (zbiorKlas[idLokalizacji].blokowany)
                {
                    return;
                }

                else if (ee.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point temp = Control.MousePosition;
                    Point res = new Point(firstPoint.X - temp.X, firstPoint.Y - temp.Y);

                    control.Location = new Point(control.Location.X - res.X, control.Location.Y - res.Y);
                    firstPoint = temp;

                    // warunki brzegowe żeby kontrolka nie wypadła poza ekran

                    if (control.Location.X < 0 && control.Location.Y >= 0)
                    {

                        control.Location = new Point(0, control.Location.Y);


                    }




                    else if (control.Location.X >= 0 && control.Location.Y < 0)
                    {
                        control.Location = new Point(control.Location.X, 0);


                    }


                    else if (control.Location.X < 0 && control.Location.Y < 0)
                    {
                        control.Location = new Point(0, 0);


                    }

                    zbiorKlas[idLokalizacji].pozX = control.Location.X;
                    zbiorKlas[idLokalizacji].pozY = control.Location.Y;





                }

            };




        }


        void item_Click(object sender, EventArgs e)

        {


            //identyfikacja klikniętej opcji menu
            MenuItem mI = (MenuItem)sender;


            string str = mI.Text.ToString();

            switch (str)
            {



                case "BLOKUJ":

                    if (zbiorKlas.ContainsKey(idLokalizacji))

                    {

                        zbiorKlas[idLokalizacji].blokowany = true;
                        db.editCubby(currentlyEditCubby.Id, currentlyEditCubby.Name, currentlyEditCubby.PosX, currentlyEditCubby.PosY, currentlyEditCubby.Height, currentlyEditCubby.Width);
                        db.loadCubbies(dataGridView2);
                        MessageBox.Show("ZABLOKOWANO OBIEKT");
                        //ZMIANA KOLORU
                    }
                    break;

                case "ODBLOKUJ":


                    if (zbiorKlas.ContainsKey(idLokalizacji))
                    {

                        zbiorKlas[idLokalizacji].blokowany = false;
                        MessageBox.Show("ODBLOKOWANO OBIEKT");
                    }

                    break;

                case "ZMIEŃ ROZMIAR":
                    {
                        zmianaRozmiaru zmR = new zmianaRozmiaru("ZMIANA ROZMIARU DLA KONTROLKI " + idLokalizacji.ToString(), 1);
                        zmR.ShowDialog();
                        zmienWymiarKontrolki(idLokalizacji.ToString());
                        db.editCubbySize(idLokalizacji, lokalizacja.xTemp, lokalizacja.yTemp);
                        db.loadCubbies(dataGridView2);

                    }
                    break;

                case "ZMIEŃ NAZWĘ":
                    {

                        zmianaNazwy zn = new zmianaNazwy(idLokalizacji.ToString());
                        zn.ShowDialog();
                        zmienNazweKontrolki(idLokalizacji.ToString());
                        db.editCubbyName(idLokalizacji, lokalizacja.nazwaTemp);
                        db.loadCubbies(dataGridView2);
                    }


                    break;

                case "ZMIEŃ POZYCJĘ":
                    {

                        zmianaRozmiaru zn = new zmianaRozmiaru(idLokalizacji.ToString(), 2);
                        zn.ShowDialog();
                        zmienPozycjeKontrolki(idLokalizacji.ToString(), lokalizacja.xTemp, lokalizacja.yTemp);
                        db.editCubby(currentlyEditCubby.Id, currentlyEditCubby.Name, lokalizacja.xTemp, lokalizacja.yTemp, currentlyEditCubby.Height, currentlyEditCubby.Width);

                        db.loadCubbies(dataGridView2);
                    }


                    break;

                case "EDYTUJ":
                    {

                        zbiorKlas[idLokalizacji].pusty = false;
                        kolorKontrolki(idLokalizacji.ToString());
                        editStorageplace editStorageplace = new editStorageplace();
                        editStorageplace.ShowDialog();
                    }


                    break;

                case "KASUJ OBIEKT":
                    if (zbiorKlas.ContainsKey(idLokalizacji))
                    {

                        DialogResult dialog = MessageBox.Show("CZY USUNĄĆ OBIEKT?", "USUWANIE", MessageBoxButtons.OKCancel);

                        if (dialog == DialogResult.OK)
                        {
                            usunKontrolke(idLokalizacji.ToString());
                            zbiorKlas.Remove(idLokalizacji);
                            db.delCubby(idLokalizacji);
                            db.loadCubbies(dataGridView2);
                        }
                    }
                    break;

                default:
                    break;
            }



        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zmianaRozmiaru ust = new zmianaRozmiaru("zmiana ustawień globalnych dla nowych elementów", 0);
            ust.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("CZY WYCZYŚCIĆ PLANSZĘ??", "USUWANIE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);


            if (dialog == DialogResult.OK)
            {
                czynscPlansze();
                zbiorKlas.Clear();
                db.delAllStorageplace();
            }
        }

        private void markCubbies()
        {

            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int k = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());

                    foreach (Control p in panel0.Controls)
                    {

                        if (p is UserControl)
                        {
                            if (p.Name.ToString() == k.ToString())
                            {
                                p.BackColor = Color.Yellow;
                                p.BackColor = Color.Blue;


                            }

                        }
                    }
                }
            }

        }

        void readCubbies()

        {

            int indeks, pozx, pozy, szer, wys;
            string nazwa;
            if (dataGridView2.RowCount != 0)


            {
                for (int k = 0; k < dataGridView2.RowCount; k++)
                {
                    indeks = int.Parse(dataGridView2.Rows[k].Cells[0].Value.ToString());
                    nazwa = dataGridView2.Rows[k].Cells[1].Value.ToString();
                    pozx = int.Parse(dataGridView2.Rows[k].Cells[2].Value.ToString());
                    pozy = int.Parse(dataGridView2.Rows[k].Cells[3].Value.ToString());
                    wys = int.Parse(dataGridView2.Rows[k].Cells[4].Value.ToString());
                    szer = int.Parse(dataGridView2.Rows[k].Cells[5].Value.ToString());

                    // tworzenie kontrolki storageplace

                    storageplace lok1 = new storageplace();

                    // tworzenie obiektu lokalizacja  o własciwościach klasy lokalizacja
                    // lok podaje właściwości konkretnergo storageplace

                    lokalizacja lok = new lokalizacja();

                    //utworzenie słownika klas

                    zbiorKlas.Add(indeks, lok);

                    //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

                    lok1.Name = indeks.ToString();
                    lok.nazwa = nazwa;
                    lok.indeks = indeks;

                    //przypisanie metod zaznaczenie, przesunięcie
                    lok1.MouseClick += zaznaczenie;

                    lok1.MouseDown += MyControl_MouseDown;

                    lok1.MouseMove += MyControl_MouseMove;


                    // punkt początkowy kontrolki
                    lok1.Location = new Point(pozx, pozy);
                    lok.pozX = pozx;
                    lok.pozY = pozy;

                    // rozmiary kontrolki
                    lok1.Size = new Size(szer, wys);
                    lok.wymiarSzer = szer;
                    lok.wymiarWys = wys;

                    // dodanie na ekranie
                    panel0.Controls.Add(lok1);


                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();

            MessageBox.Show(screenHeight + " " + screenWidth);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            if (animacja == true && currentlyData.Autologoff && currentlyData.UserName!="admin")
            {
                this.Close();
                Application.Exit();
                Application.Restart();
            }

            else
            {
                this.Close();

            }


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            { dataGridView2.Visible = false; }

            else
            {
                dataGridView2.Visible = true;
            }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // currentlyEditUser.UserId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            // currentlyEditUser.UserName = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ZAPISUJE WSZYSTKIE ELEMENTY

            DialogResult dialog = MessageBox.Show("CZY ZAPISAĆ NOWY UKŁAD?", "ZAPISYWANIE", MessageBoxButtons.OKCancel);

            if (dialog == DialogResult.OK)
            {

                foreach (Control ctr in this.panel0.Controls)

                {

                    idLokalizacji = int.Parse(ctr.Name);
                    czytaj_kontrolke();
                    db.editCubby(currentlyEditCubby.Id, currentlyEditCubby.Name, currentlyEditCubby.PosX, currentlyEditCubby.PosY, currentlyEditCubby.Height, currentlyEditCubby.Width);

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            kolorKontrolki("4142");
        }
    }
}
