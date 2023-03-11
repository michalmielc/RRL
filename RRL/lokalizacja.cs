using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL
    {
    
        interface Ilokalizacja
        {
            //  void zaznaczenie();
            //  void odznaczenie();
            //  void blokowanie();
            //  void odblokowanie();

        }

        public class lokalizacja : Ilokalizacja
        {
            // tworzy przycisk/lokalizację
            // dodaje przycisk/lokalizację
            // usuwa przycisk/lokalizację

            // zaznaczony TAK/NIE
            public bool zaznaczony { get; set; }

            // blokowany TAK/NIE
            public bool blokowany { get; set; }

            // pozycja x i y

            public int pozX { get; set; }
            public int pozY { get; set; }

            // wymiary szer i dlugosc
            public int wymiarSzer { get; set; }
            public int wymiarWys { get; set; }


            //nazwa obiektu
            public string nazwa { get; set; }


            //wstepne rozmiary kontrolki
            public static int x = 100;
            public static int y = 30;

            //TYMCZASOWE rozmiary kontrolki PRZY pojedyńczych zmianach
            public static int xTemp = 100;
            public static int yTemp = 30;
            public static string nazwaTemp = "";

            //PUSTY PEŁNY
            public bool pusty { get; set; }
    
            //INDEKS WG PANEL0
            public int indeks { get; set; }


        //konstruktor

        public lokalizacja()
            {
               // instanceId = ++instanceCounter;// nowy numer id
                zaznaczony = true;
                blokowany = true;
                pozX = 20;
                pozY = 20; 
                wymiarSzer = x;
                wymiarWys = y;
                 nazwa = "PODAJ NAZWĘ OBIEKTU NR ";
                pusty = true;
                indeks = 0;

            }

            // zmienna przesuwajaca 
            // public static int wsp = 30;

            //licznik klasy

      //      private static int instanceCounter = 100;

        //    private readonly int instanceId;


            //ZWRACA ID KLASY
           // public int UniqueId
          //  {
        //        get { return this.instanceId; }
          //  }



         }
    }


