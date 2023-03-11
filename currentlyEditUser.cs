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
    public class currentlyEditUser
    {
        public static int UserId = 0;
        public static string UserName = "";
        public static string UserDepartment = "";
        public static string Password = "";
        public static int UserGroupId = 0;
        public static string UserGroupName ="";
        public static byte Active= 0;
        public static bool edit = false;
        public static bool add = false;
      

        public static void zerujDane ()
        {
            UserId = 0;
            UserName = "";
            UserDepartment = "";
            Password = "";
            UserGroupId = 0;
            UserGroupName ="";
            Active= 0;
            edit = false;
            add = false;
      
        }
      
    }
}
