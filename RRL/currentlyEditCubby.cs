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
    public class currentlyEditCubby
    {
        public static int Id = 0;
        public static string Name = "";
        public static int PosX= 0;
        public static int PosY = 0;
        public static int Height = 0;
        public static int Width= 0;
        public static bool isempty= true;

        public static void zerujDane ()
        {
        
        Id = 0;
        Name = "";
        PosX = 0;
        PosY = 0;
        Height = 0;
        Width = 0;
        isempty = true;

    }
      
    }
}
