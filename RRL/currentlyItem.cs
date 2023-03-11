using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRL
{
    class currentlyItem
    {

        public static int ItemId = 0;
        public static string ItemName1 = "";
        public static string ItemName2 = "";
        public static string ItemName3 = "";
        public static string Barcode = "";
        public static string PicPath= "";
        public static byte Active = 0;
        public static Decimal Price = 0;
        public static int MinInventory= 0;
        public static bool edit = false;
        public static bool add = false;
        public static int amount = 0;
        public static bool withdraw = false;
        public static bool fill= false;
        public static bool stocktaking = false;
        public static string Supplier = "";



        public static void zerujDane()
        {


        ItemId = 0;
        ItemName1 = "";
        ItemName2 = "";
        ItemName3 = "";
        Barcode = "";
        PicPath = "";
        Active = 0;
        Price = 0;
        MinInventory = 0;
        edit = false;
        add = false;
        amount = 0;
        withdraw = false;
        fill = false;
        stocktaking = false;
        Supplier = "";
    }
    }
}
