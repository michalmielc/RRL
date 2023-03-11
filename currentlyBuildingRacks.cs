using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Drawing;
using System.ComponentModel;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Windows.Forms;

namespace RRL
{
    public class currentlyBuildingRacks
    {

        public static int BalkWidth = 0;
        public static int RackWidth = 0;
        public static int RackHeight = 0;
        public static int RackLevels= 0;
        public static int RackAmount = 0;
        public static int VectorOfDividing = 0;
        public static int startPointX = 0;
        public static int startPointY = 0;
        public static int storageAmount = 0;
        public static int wektor_pion = 0;
        public static double wsp_wysokosci_storage= 0;
        public static int balk_width_horizontal = 0;
        public static int storageY = 0;
        public static int wektor_poziom = 0;


        public static void zerujDane ()
        {


        BalkWidth = 0;
        RackWidth = 0;
        RackHeight = 0;
        RackLevels = 0;
        RackAmount = 0;
        VectorOfDividing = 0;
        startPointX = 0;
        startPointY = 0;
        storageAmount = 0;
        wektor_pion = 0;
        wsp_wysokosci_storage = 0;
        balk_width_horizontal = 0;
        storageY = 0;
        wektor_poziom = 0;
        }
      
    }
}
