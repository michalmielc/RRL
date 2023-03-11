using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL
{
    class currentlyCostCenter
    {

        public static int CostId = 0;
        public static string CostName = "";
        public static bool edit = false;
        public static bool add = false;
        // pobranie ustawień czy jest opcja aktywan przy pobraniach, uzupełnieniach
        public static bool activefill = false;
        public static bool activewithdraw = false;
        public static bool activeedit = false;



        public static void zerujDane()
        {


        CostId = 0;
        CostName = "";
        edit = false;
        add = false;

    }
    }
}
