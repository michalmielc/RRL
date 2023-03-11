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
        public Dictionary<int, balk_position> zbiorKlasBalk = new Dictionary<int, balk_position>();

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

            

            panel0.Height = Screen.PrimaryScreen.Bounds.Height- 170;
            panel0.Width= Screen.PrimaryScreen.Bounds.Width - 40;
            
            db.loadCubbies(dataGridView2);
            db.loadBalks(dataGridView3);

            readCubbies(1);

            kolorKontrolki_czypusta(0,1);

            readBalks(1);



            if (animacja == true)
            {

                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
                button8.Visible = false;
                button5.Visible = false;


                button7.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;


                db.markedItems(dataGridView1);
                markCubbies();

                dataGridView2.Visible = false;
                dataGridView3.Visible = false;

                textBox1.Visible = false;

                dataGridView1.Columns[0].Width = 1;
            }

       
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
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

          //  numer.BackColor = Color.Pink;

            panel0.Controls.Add(numer);
        }

        // ZAZNACZENIE OBIEKTU STORAGEPLACE

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

                control.BackColor = Color.Blue;

                //odczyt danych lokalizacji.

                czytaj_kontrolke();
                //pojawia sie menu

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();
                    

                    m.MenuItems.Add("BLOKUJ / ZAPISZ POZYCJĘ", new EventHandler(item_Click));
                    m.MenuItems.Add("ODBLOKUJ", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ ROZMIAR", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ NAZWĘ", new EventHandler(item_Click));
                    m.MenuItems.Add("ZMIEŃ POZYCJĘ", new EventHandler(item_Click));
                    m.MenuItems.Add("EDYTUJ", new EventHandler(item_Click));
                    m.MenuItems.Add("KASUJ OBIEKT", new EventHandler(item_Click));
                     m.Show(control, new Point(e.X, e.Y));

                    }

                if (!db.checkCubbieIfEmpty2(idLokalizacji))
                {
                    control.BackColor = Color.Transparent;
                }


                if (db.checkCubbieIfEmpty2(idLokalizacji))
                {
                    control.BackColor = Color.Yellow;
                }
                return;

            }

        }

        // ZAZNACZENIE OBIEKTU BALK

        public void zaznaczenie1(object sender, MouseEventArgs e)
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
                control.BackColor = Color.Black;

                //odczyt danych lokalizacji.

                czytaj_belke();
                //pojawia sie menu

                if (e.Button == MouseButtons.Right)
                {
                    ContextMenu m = new ContextMenu();


                    m.MenuItems.Add("BLOKUJ / ZAPISZ POZYCJĘ", new EventHandler(item_Click2));
                    m.MenuItems.Add("ODBLOKUJ", new EventHandler(item_Click2));
                    m.MenuItems.Add("ZMIEŃ ROZMIAR", new EventHandler(item_Click2));
                    m.MenuItems.Add("ZMIEŃ NAZWĘ", new EventHandler(item_Click2));
                    m.MenuItems.Add("ZMIEŃ POZYCJĘ", new EventHandler(item_Click2));
                    m.MenuItems.Add("KASUJ OBIEKT", new EventHandler(item_Click2));
                    m.Show(control, new Point(e.X, e.Y));

                }

            
                    control.BackColor = Color.Red;
                

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
        void usunKontrolke(string txt, bool storage)

        {

            if (storage)
            {
                foreach (Control ctr in this.panel0.Controls)

                {
                    if (ctr is storageplace)
                    {
                        if (ctr.Name == txt)
                        {

                            panel0.Controls.Remove(ctr);
                            ctr.Dispose();
                        }
                    }
                }
            }

            else
            {
                foreach (Control ctr in this.panel0.Controls)

                {
                    if (ctr is balk)
                    {
                        if (ctr.Name == txt)
                        {

                            panel0.Controls.Remove(ctr);
                            ctr.Dispose();
                        }
                    }
                }

            }

        }

        // ZMIEŃ KOLOR KONTROLKI
        void kolorKontrolki_czypusta(int x, byte option)

        {

            //SPRAWDZENIE WSZYSTKICH

            if (option == 1)

            {
                foreach (Control ctr in this.panel0.Controls)

                {
                    if (ctr is storageplace)
                    {
                        x = int.Parse(ctr.Name);

                            if (!db.checkCubbieIfEmpty2(x))

                            {
                                ctr.BackColor = Color.Transparent;
                            }

                            else

                        {
                            ctr.BackColor = Color.Yellow;
                        }


                        }
                    }

                }



            //SPRAWDZENIE WYBRANEJ

            if (option == 2)

            {
                foreach (Control ctr in this.panel0.Controls)

                {
                    if (ctr is storageplace)
                    {
                        if (ctr.Name == x.ToString())
                        {

                            if (!db.checkCubbieIfEmpty2(x))

                            {
                                ctr.BackColor = Color.Transparent;
                            }
                            else
                            {
                                ctr.BackColor = Color.Yellow;
                            }
                        }
                    }

                }

            }


            if (option == 3)

            {
                foreach (Control ctr in this.panel0.Controls)

                {
                    if (ctr is storageplace)
                    {
                        if (ctr.Name == x.ToString())
                        {

                          
                                ctr.BackColor = Color.Blue;
                            
                        }
                    }

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

        //ZMIANA WYMIAR BELKI
        void zmienWymiarKontrolki1(string txt)
        {

            //zmiana wymiarów w klasie

            zbiorKlasBalk[idLokalizacji].wymiarSzer = balk_position.xTemp;
            zbiorKlasBalk[idLokalizacji].wymiarWys = balk_position.yTemp;


            foreach (Control ctr in this.panel0.Controls)
            {

                if (ctr.Name == txt)
                {
                    ctr.Size = new Size(balk_position.xTemp, balk_position.yTemp);

                }

            }


        }


        //ZMIANA POZYCJĘ BELKI

        void zmienPozycjeKontrolki1(string txt, int a, int b)
        {


            zbiorKlasBalk[idLokalizacji].pozX = a;
            zbiorKlasBalk[idLokalizacji].pozY = b;


            foreach (Control ctr in this.panel0.Controls)
            {

                if (ctr.Name == txt)
                {
                    ctr.Location = new Point(a, b);

                }

            }


        }

        //ZMIANA NAZWE BELKI ( REGAŁU)

        void zmienNazweKontrolki1(string txt)
        {

            //ZMIANA NAZWY W KLASIE

            zbiorKlasBalk[idLokalizacji].nazwa = balk_position.nazwaTemp;

        }

        // CZYSĆ PLANSZĘ

        void czynscPlansze()
        {

            panel0.Controls.Clear();
            lokalizacja.nazwaTemp = "";
            balk_position.nazwaTemp = "";

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
        //ODCZYT DANYCH BELKI Z REGAŁU
        void czytaj_belke()
        {
            currentlyEditBalk.Id = int.Parse(zbiorKlasBalk[idLokalizacji].indeks.ToString());
            currentlyEditBalk.PosX = int.Parse(zbiorKlasBalk[idLokalizacji].pozX.ToString());
            currentlyEditBalk.PosY = int.Parse(zbiorKlasBalk[idLokalizacji].pozY.ToString());
            currentlyEditBalk.Height = int.Parse(zbiorKlasBalk[idLokalizacji].wymiarWys.ToString());
            currentlyEditBalk.Width = int.Parse(zbiorKlasBalk[idLokalizacji].wymiarSzer.ToString());
            currentlyEditBalk.RackName = zbiorKlasBalk[idLokalizacji].nazwa.ToString();

            textBox1.Clear();
            textBox1.AppendText("nazwa: " + zbiorKlasBalk[idLokalizacji].nazwa.ToString() + Environment.NewLine);
            textBox1.AppendText("pozX: " + zbiorKlasBalk[idLokalizacji].pozX.ToString() + Environment.NewLine);
            textBox1.AppendText("pozY: " + zbiorKlasBalk[idLokalizacji].pozY.ToString() + Environment.NewLine);
            textBox1.AppendText("ID: " + zbiorKlasBalk[idLokalizacji].indeks.ToString() + Environment.NewLine);
            textBox1.AppendText("BLOKOWANY: " + zbiorKlasBalk[idLokalizacji].blokowany.ToString() + Environment.NewLine);
            textBox1.AppendText("szer: " + zbiorKlasBalk[idLokalizacji].wymiarSzer.ToString() + Environment.NewLine);
            textBox1.AppendText("wys: " + zbiorKlasBalk[idLokalizacji].wymiarWys.ToString() + Environment.NewLine);
            textBox1.AppendText("ZAZNACZONY: " + zbiorKlasBalk[idLokalizacji].zaznaczony.ToString() + Environment.NewLine);

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

        //RUSZANIE MYSZKA I OBIEKTEM STORAGEPLACE

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


        //RUSZANIE MYSZKA I OBIEKTEM STORAGEPLACE
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

        //RUSZANIE MYSZKA I OBIEKTEM BALK

        public void MyControl_MouseDown1(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            idLokalizacji = int.Parse(control.Name);
            


            control.MouseDown += (ss, ee) =>
            {


                if (zbiorKlasBalk[idLokalizacji].blokowany)
                {
                    return;
                }

                else if (ee.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    firstPoint = Control.MousePosition;
                }

            };

        }
   
        //RUSZANIE MYSZKA I OBIEKTEM STORAGEPLACE
        public void MyControl_MouseMove1(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            idLokalizacji = int.Parse(control.Name);


            control.MouseMove += (ss, ee) =>
            {

                if (zbiorKlasBalk[idLokalizacji].blokowany)
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

                    zbiorKlasBalk[idLokalizacji].pozX = control.Location.X;
                    zbiorKlasBalk[idLokalizacji].pozY = control.Location.Y;





                }

            };




        }

        // WYBRANIE OPCJI Z PPM MENU DLA STORAGEPLACE

        void item_Click(object sender, EventArgs e)

        {

            // zaznaczenie obiektu - zmiana koloru

          

            //identyfikacja klikniętej opcji menu
            MenuItem mI = (MenuItem)sender;

            
            string str = mI.Text.ToString();

            switch (str)
            {



                case "BLOKUJ / ZAPISZ POZYCJĘ":

                    if (zbiorKlas.ContainsKey(idLokalizacji))

                    {

                        zbiorKlas[idLokalizacji].blokowany = true;
                        db.editCubby(currentlyEditCubby.Id, currentlyEditCubby.Name, currentlyEditCubby.PosX, currentlyEditCubby.PosY, currentlyEditCubby.Height, currentlyEditCubby.Width);
                        db.loadCubbies(dataGridView2);
                        MessageBox.Show("ZABLOKOWANO OBIEKT");
                     
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
                        editStorageplace editStorageplace = new editStorageplace();
                        editStorageplace.ShowDialog();
                        db.markedItems(dataGridView1);
                        kolorKontrolki_czypusta(idLokalizacji, 2);

                    }


                    break;

                case "KASUJ OBIEKT":
                    if (zbiorKlas.ContainsKey(idLokalizacji))
                    {

                        DialogResult dialog = MessageBox.Show("CZY USUNĄĆ OBIEKT?", "USUWANIE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                        if (dialog == DialogResult.OK)
                        {
                              
                  
                             if (!db.checkCubbieIfEmpty(idLokalizacji))
                                {
                                usunKontrolke(idLokalizacji.ToString(),true);
                                zbiorKlas.Remove(idLokalizacji);
                                db.delCubby(idLokalizacji);
                                db.loadCubbies(dataGridView2);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }



        }

        // WYBRANIE OPCJI Z PPM MENU DLA BELK

        void item_Click2(object sender, EventArgs e)

        {


            //identyfikacja klikniętej opcji menu
            MenuItem mI = (MenuItem)sender;


            string str = mI.Text.ToString();

            switch (str)
            {



                case "BLOKUJ / ZAPISZ POZYCJĘ":

                    if (zbiorKlasBalk.ContainsKey(idLokalizacji))

                    {

                        zbiorKlasBalk[idLokalizacji].blokowany = true;
                        db.editBalk(currentlyEditBalk.Id, currentlyEditBalk.PosX, currentlyEditBalk.PosY, currentlyEditBalk.Height, currentlyEditBalk.Width, currentlyEditBalk.RackId, currentlyEditBalk.RackName);
            
                        MessageBox.Show("ZABLOKOWANO OBIEKT");
                 
                    }
                    break;

                case "ODBLOKUJ":


                    if (zbiorKlasBalk.ContainsKey(idLokalizacji))
                    {

                        zbiorKlasBalk[idLokalizacji].blokowany = false;
                        MessageBox.Show("ODBLOKOWANO OBIEKT");
                    }

                    break;

                case "ZMIEŃ ROZMIAR":
                    {
                        zmianaRozmiaru zmR = new zmianaRozmiaru("ZMIANA ROZMIARU DLA BELKI " + idLokalizacji.ToString(), 1);
                        zmR.ShowDialog();
                        zmienWymiarKontrolki1(idLokalizacji.ToString());
                        db.editBalkSize(idLokalizacji, balk_position.xTemp, balk_position.yTemp);
                        db.loadBalks(dataGridView3);

                    }
                    break;

                case "ZMIEŃ NAZWĘ":
                    {

                        zmianaNazwy zn = new zmianaNazwy(idLokalizacji.ToString());
                        zn.ShowDialog();
                        zmienNazweKontrolki1(idLokalizacji.ToString());
                        db.editBalkName(idLokalizacji, balk_position.nazwaTemp);
                        db.loadBalks(dataGridView3);
                    }


                    break;

                case "ZMIEŃ POZYCJĘ":
                    {

                        zmianaRozmiaru zn = new zmianaRozmiaru(idLokalizacji.ToString(), 2);
                        zn.ShowDialog();
                        zmienPozycjeKontrolki1(idLokalizacji.ToString(), balk_position.xTemp, balk_position.yTemp);
                        db.editBalk(currentlyEditBalk.Id,  balk_position.xTemp, balk_position.yTemp, currentlyEditBalk.Height, currentlyEditBalk.Width, currentlyEditBalk.RackId,  currentlyEditBalk.RackName);
                        db.loadBalks(dataGridView3);

                    }


                    break;

        


                

                case "KASUJ OBIEKT":
                    if (zbiorKlasBalk.ContainsKey(idLokalizacji))
                    {

                        DialogResult dialog = MessageBox.Show("CZY USUNĄĆ OBIEKT?", "USUWANIE", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

                        if (dialog == DialogResult.OK)
                        {
                   
                     
                                usunKontrolke(idLokalizacji.ToString(),false);
                                zbiorKlasBalk.Remove(idLokalizacji);
                                db.delBalk(idLokalizacji);
                                db.loadBalks(dataGridView3);

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

        
        private void markCubbies()
        {

            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    int k = int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString());

                    foreach (Control p in panel0.Controls)
                    {

                        if (p is storageplace)
                        {
                            if (p.Name.ToString() == k.ToString())
                            {
                                 p.BackColor = Color.Blue;


                            }

                        }
                    }
                }
            }

        }

        void readCubbies(double coeff)

        {

            int indeks, pozx, pozy, szer, wys;
            string nazwa;
            int r = 0; 

            if (dataGridView2.RowCount != 0)


                
                for (int kolumna = 2; kolumna <= 5; kolumna++)
                {


                    for (int m = 0; m < dataGridView2.RowCount; m++)
                    {
                       r=    Convert.ToInt32 (dataGridView2.Rows[m].Cells[kolumna].Value.ToString());

                        //   MessageBox.Show("WIERSZ: " + m.ToString()+ "kolumna: " + kolumna.ToString()+ "WARTOŚĆ: " + dataGridView2.Rows[m].Cells[kolumna].Value.ToString());
                        // dataGridView2.Rows[m].Cells[kolumna].Value = int.Parse((d * double.Parse(dataGridView2.Rows[m].Cells[kolumna].Value.ToString())).ToString());

                        //     dataGridView2.Rows[m].Cells[kolumna].Value = int.Parse((coeff * r).ToString());

                        dataGridView2.Rows[m].Cells[kolumna].Value = Convert.ToInt32(coeff * r);


                    }

                }

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

        void readBalks( double coeff)

        {

            int indeks, pozx, pozy, szer, wys;
            string nazwa;
            int r;

            // SKALOWANIE 
            if (dataGridView3.RowCount != 0)

       


            for (int kolumna = 1; kolumna <= 4; kolumna++)
                {


                    for (int m = 0; m < dataGridView3.RowCount; m++)
                    {
                     r = Convert.ToInt32(dataGridView3.Rows[m].Cells[kolumna].Value.ToString());

                    dataGridView3.Rows[m].Cells[kolumna].Value = Convert.ToInt32(coeff * r);

                }

                }


            {
                for (int k = 0; k < dataGridView3.RowCount; k++)
                {
                    indeks = int.Parse(dataGridView3.Rows[k].Cells[0].Value.ToString());
                    nazwa = dataGridView3.Rows[k].Cells[6].Value.ToString();
                    pozx = int.Parse(dataGridView3.Rows[k].Cells[1].Value.ToString());
                    pozy = int.Parse(dataGridView3.Rows[k].Cells[2].Value.ToString());
                    wys = int.Parse(dataGridView3.Rows[k].Cells[3].Value.ToString());
                    szer = int.Parse(dataGridView3.Rows[k].Cells[4].Value.ToString());

                    // tworzenie kontrolki belki

                    balk balk = new balk();

                    // tworzenie obiektu balk o własciwościach klasy balk
                    // balk_position podaje właściwości konkretnergo balk

                    balk_position balk_position = new balk_position();

                    //utworzenie słownika klas

                    zbiorKlasBalk.Add(indeks, balk_position);

                    //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

                    balk.Name = indeks.ToString();
                    balk_position.nazwa = nazwa;
                    balk_position.indeks = indeks;

                    //przypisanie metod zaznaczenie, przesunięcie
                    balk.MouseClick += zaznaczenie1;

                    balk.MouseDown += MyControl_MouseDown1;

                    balk.MouseMove += MyControl_MouseMove1;


                    // punkt początkowy kontrolki
                    balk.Location = new Point(pozx, pozy);
                    balk_position.pozX = pozx;
                    balk_position.pozY = pozy;

                    // rozmiary kontrolki

                    balk.Size = new Size(szer, wys);
                    balk_position.wymiarSzer = szer;
                    balk_position.wymiarWys = wys;


                    //TEST NAZWY KONTROLKI
 
                    panel0.Controls.Add(balk);





                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // currentlyEditUser.UserId = int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            // currentlyEditUser.UserName = dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString();
        }

        
        private void pokazpuste(object sender, EventArgs e)
        {
       

        }


        //DODAJ LOKALIZACJĘ

        private void button1_Click_1(object sender, EventArgs e)
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
            lok1.Location = new Point(10, 10);

            // rozmiary kontrolki

            lok1.Size = new Size(lokalizacja.x, lokalizacja.y);

            // TEST FUNKCJI ETYKIETA
            //etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(lok1);

            //wpis do bazy danych
            db.addCubby();

            //odświeżenie tabeli
            db.loadCubbies(dataGridView2);

        }
        
        // DODAJ BELKĘ

        private void button5_Click_1(object sender, EventArgs e)
        {
            int indeks = db.nextfreeBalkid();   

            balk balk = new balk();


            // tworzenie obiektu lokalizacja  o własciwościach klasy lokalizacja
            // lok podaje właściwości konkretnergo storageplace

            balk_position balk_position = new balk_position();

            //utworzenie słownika klas

            zbiorKlasBalk.Add(indeks, balk_position);

            //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

            balk.Name = indeks.ToString();
            balk_position.indeks = indeks;

            //przypisanie metod zaznaczenie, przesunięcie
            balk.MouseClick += zaznaczenie1;

            balk.MouseDown += MyControl_MouseDown1;

            balk.MouseMove += MyControl_MouseMove1;


            // punkt początkowy kontrolki
            balk.Location = new Point(10, 10);

            // rozmiary kontrolki

            balk.Size = new Size(balk_position.x, balk_position.y);

            //TEST NAZWY KONTROLKI
            //etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(balk);

            //wpis do bazy danych
            db.addBalk();

        }

        //WYJŚCIE

        private void button4_Click(object sender, EventArgs e)
        {


            if (animacja == true && currentlyData.Autologoff && currentlyData.UserName != "admin")
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

        // USUŃ WSZYSTKO
        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("CZY WYCZYŚCIĆ PLANSZĘ? WYMAZANE ZOSTANĄ MIEJSCA MAGAZYNOWE! "
                + "\n" + "STANY MAGAZYNOWE NIE ZOSTANĄ USUNIĘTE!", "USUWANIE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);


            if (dialog == DialogResult.OK)
            {
                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();
                db.delAllStorageplace();
                
                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
            }
        }

        //POKAŻ UKRYJ LISTĘ STORAGEPLACES
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            { dataGridView2.Visible = false; }

            else
            {
                dataGridView2.Visible = true;
            }

        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!checkBox2.Checked)
            {
           
                 dataGridView1.Visible = false;

            }

                else
                {
                    dataGridView1.Visible = true;
                }
            
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            string screenWidth = Screen.PrimaryScreen.Bounds.Width.ToString();
            string screenHeight = Screen.PrimaryScreen.Bounds.Height.ToString();

            MessageBox.Show("AKTUALNY WYMIAR EKRANU: \n"+ screenHeight + " " + screenWidth);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
            {

                dataGridView3.Visible = false;

            }

            else
            {
                dataGridView3.Visible = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
        
            rackCreator rc = new rackCreator();

            rc.ShowDialog();

 
                kreator();

            db.loadCubbies(dataGridView2);
            db.loadBalks(dataGridView3);
       
            }

        private void button2_Click_1(object sender, EventArgs e)
        {
            zmianaRozmiaru ust = new zmianaRozmiaru("zmiana ustawień globalnych dla nowych elementów", 0);
            ust.ShowDialog();
        }

        //KREATOR MAGAZYNU

        private void kreator( )
        {
            int k = 0;

            for ( int a =1; a<=currentlyBuildingRacks.RackAmount;a++)
            {

            // TWORZENIE NOGI REGAŁU ( POCZATEK )
                createVertikalBalk(0+k);

            
                k = k + currentlyBuildingRacks.BalkWidth;

                // TWORZENIE NOGI REGAŁU ( KONIEC)
                createVertikalBalk(currentlyBuildingRacks.RackWidth+k);

             

                if (currentlyBuildingRacks.RackLevels >1)
                {
                    for (int b = 1; b < currentlyBuildingRacks.RackLevels; b++)
                    
                        // TWORZENIE POZIOMÓW REGAŁU
                    {
                          if (a == 1)
                        {
                            createHorizontalBalk(b * currentlyBuildingRacks.VectorOfDividing + (b - 1) * currentlyBuildingRacks.BalkWidth, k - currentlyBuildingRacks.BalkWidth);
       
                        }
                          else
                        {
                            createHorizontalBalk(b * currentlyBuildingRacks.VectorOfDividing + (b - 1) * currentlyBuildingRacks.BalkWidth, k- currentlyBuildingRacks.BalkWidth);
           
                        }

                    }
                }



                k = k + currentlyBuildingRacks.RackWidth + currentlyBuildingRacks.BalkWidth;
            }


            //KREATOR MIEJSC MAGAZYNOWYCH

           for (int s = 1; s <= currentlyBuildingRacks.RackAmount; s++)
           {

                for (int l = 1; l < currentlyBuildingRacks.RackLevels; l++)

                {
                    for (int j = 1; j <= currentlyBuildingRacks.storageAmount; j++)

                    {
                        createStorage((s-1)*(currentlyBuildingRacks.RackWidth+2*currentlyBuildingRacks.BalkWidth)+ currentlyBuildingRacks.startPointX + 5 + currentlyBuildingRacks.BalkWidth + (j - 1) * (currentlyBuildingRacks.wektor_pion + 5), currentlyBuildingRacks.startPointY + l * currentlyBuildingRacks.VectorOfDividing- currentlyBuildingRacks.storageY + (l - 1) * currentlyBuildingRacks.BalkWidth);

                     
                    }

                }

                //OSTATNI POZIOM  -KREATOR MIEJSC MAGAZYNOWYCH NAJNIŻSZEGO POZIOMU

                if (currentlyBuildingRacks.storageAmount != 0)
                {
                    for (int j = 1; j <= currentlyBuildingRacks.storageAmount; j++)

                    {
                        createStorage((s - 1) * (currentlyBuildingRacks.RackWidth + currentlyBuildingRacks.BalkWidth) + (s - 1) * currentlyBuildingRacks.BalkWidth + currentlyBuildingRacks.startPointX + 5 + currentlyBuildingRacks.BalkWidth + (j - 1) * (currentlyBuildingRacks.wektor_pion + 5), currentlyBuildingRacks.RackHeight + currentlyBuildingRacks.startPointY - (int)((currentlyBuildingRacks.wsp_wysokosci_storage * currentlyBuildingRacks.RackHeight )/ currentlyBuildingRacks.RackLevels));
                    }
                }

           }

 
        }
        // RYSOWANIE BELKI POZIOMEJ

        private void createHorizontalBalk( int vector, int vectorx)
        {


            int indeks = db.nextfreeBalkid();

            balk balk = new balk();


            // tworzenie obiektu lokalizacja  o własciwościach klasy lokalizacja
            // lok podaje właściwości konkretnergo storageplace

            balk_position balk_position = new balk_position();

            //dodanie do słownika klas

            zbiorKlasBalk.Add(indeks, balk_position);

            //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

            balk.Name = indeks.ToString();
            balk_position.indeks = indeks;

            //przypisanie metod zaznaczenie, przesunięcie
            balk.MouseClick += zaznaczenie1;

            balk.MouseDown += MyControl_MouseDown1;

            balk.MouseMove += MyControl_MouseMove1;


            // punkt początkowy kontrolki
            balk.Location = new Point(currentlyBuildingRacks.startPointX +currentlyBuildingRacks.BalkWidth + vectorx , currentlyBuildingRacks.startPointY + vector);

            // rozmiary kontrolki

            balk.Size = new Size(currentlyBuildingRacks.RackWidth, currentlyBuildingRacks.balk_width_horizontal);

            //TEST NAZWY KONTROLKI
            //etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(balk);

            currentlyEditBalk.Id = indeks;
            currentlyEditBalk.PosX = currentlyBuildingRacks.startPointX + currentlyBuildingRacks.BalkWidth + vectorx;
            currentlyEditBalk.PosY = currentlyBuildingRacks.startPointY + vector;
            currentlyEditBalk.Height = currentlyBuildingRacks.RackWidth;
            currentlyEditBalk.Width = currentlyBuildingRacks.balk_width_horizontal;


            currentlyEditBalk.RackName = "regał";

            db.addBalk_byCreator(currentlyEditBalk.Id, currentlyEditBalk.PosX, currentlyEditBalk.PosY,  currentlyEditBalk.Width, currentlyEditBalk.Height, currentlyEditBalk.RackId, currentlyEditBalk.RackName);

     
        }

        // RYSOWANIE BELKI PIONOWEJ

        private void createVertikalBalk(int vector)
        {

            

            int indeks = db.nextfreeBalkid();

            balk balk = new balk();


            // tworzenie obiektu lokalizacja  o własciwościach klasy lokalizacja
            // lok podaje właściwości konkretnergo storageplace

            balk_position balk_position = new balk_position();

            //dodanie do słownika klas

            zbiorKlasBalk.Add(indeks, balk_position);

            //nazwa kontrolki jest nazwą klasy!!! Klucz obcy!!!!

            balk.Name = indeks.ToString();
            balk_position.indeks = indeks;

            //przypisanie metod zaznaczenie, przesunięcie
            balk.MouseClick += zaznaczenie1;

            balk.MouseDown += MyControl_MouseDown1;

            balk.MouseMove += MyControl_MouseMove1;


            // punkt początkowy kontrolki
            balk.Location = new Point(currentlyBuildingRacks.startPointX+vector, currentlyBuildingRacks.startPointY);

            // rozmiary kontrolki

            balk.Size = new Size(currentlyBuildingRacks.BalkWidth, currentlyBuildingRacks.RackHeight);

            //TEST NAZWY KONTROLKI
            //etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(balk);

            currentlyEditBalk.Id = indeks;
            currentlyEditBalk.PosX = currentlyBuildingRacks.startPointX + vector;
            currentlyEditBalk.PosY = currentlyBuildingRacks.startPointY ;
            currentlyEditBalk.Height = currentlyBuildingRacks.BalkWidth;
            currentlyEditBalk.Width = currentlyBuildingRacks.RackHeight;
            currentlyEditBalk.RackName = "nowy_regal";

            db.addBalk_byCreator(currentlyEditBalk.Id, currentlyEditBalk.PosX, currentlyEditBalk.PosY,   currentlyEditBalk.Width, currentlyEditBalk.Height, currentlyEditBalk.RackId, currentlyEditBalk.RackName);


             

        }

        // RYSOWANIE STORAGEPLACE

        private void createStorage(int a, int b)
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
            lok1.Location = new Point(a, b);


            // rozmiary kontrolki

            lok1.Size = new Size(currentlyBuildingRacks.wektor_pion, currentlyBuildingRacks.storageY);

            // TEST FUNKCJI ETYKIETA
            //etykieta();

            // dodanie na ekranie
            panel0.Controls.Add(lok1);



            db.addcubbie_byCreator(indeks, a,b,currentlyBuildingRacks.storageY,currentlyBuildingRacks.wektor_pion,"nowy");



        
    }


        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {

               textBox1.Visible = false;

            }

            else
            {
                textBox1.Visible = true;
            }
        }

        // ZMIANA SKALI

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox1.SelectedItem.ToString();

            if (curItem == "x 0.8")

            {

                MessageBox.Show("x 0.8");

                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();

                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
                readBalks(0.8);
                readCubbies(0.8);
                kolorKontrolki_czypusta(0, 1);
            }

            if (curItem=="x 1.0")

            {

                MessageBox.Show("x 1.0") ;

                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();
           
                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
                readBalks(1);
                readCubbies(1);
                kolorKontrolki_czypusta(0, 1);
            }

            if (curItem == "x 1.2")

            {
                MessageBox.Show("x 1.2");

                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();
       
                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
                readCubbies(1.2);
                readBalks(1.2);
                kolorKontrolki_czypusta(0, 1);
            }

            if (curItem == "x 1.5")

            {
                MessageBox.Show("x 1.5");
         
                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();
        
                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
                readCubbies(1.5);
                readBalks(1.5);
                kolorKontrolki_czypusta(0, 1);
            }


            if (curItem == "x 2.0")

            {
                MessageBox.Show("x 2.0");
                
                czynscPlansze();
                zbiorKlas.Clear();
                zbiorKlasBalk.Clear();
          
                db.loadCubbies(dataGridView2);
                db.loadBalks(dataGridView3);
                readCubbies(2);
                readBalks(2);
                kolorKontrolki_czypusta(0, 1);

            }
        }

      
    }

    }

