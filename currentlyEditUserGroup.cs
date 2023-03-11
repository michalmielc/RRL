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
    public class currentlyEditUserGroup
    {
        public static int UserGroupId = 0;
        public static string UserGroupName = "";
        public static byte x1 = 0;
        public static byte x2 = 0;
        public static byte x3 = 0;
        public static byte x4 = 0;
        public static byte x5 = 0;
        public static byte x6 = 0;
        public static byte x7 = 0;
        public static byte x8 = 0;
        public static byte x9 = 0;
        public static byte x10 = 0;
        public static byte x11 = 0;
        
        public static bool edit = false;
        public static bool add = false;
      

        public static void zerujDane ()
        {
            UserGroupId = 0;
            UserGroupName = "";
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            x5 = 0;
            x6 = 0;
            x7 = 0;
            x8 = 0;
            x9 = 0;
            x10 = 0;
            x11 = 0;
            edit = false;
            add = false;
      
        }
      
    }
}
