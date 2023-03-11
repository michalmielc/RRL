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
    public class currentlyEditSupplier
    {
        public static int SupplierId = 0;
        public static string SupplierName = "";
        public static string SupplierEmail = "";
        public static bool add = false;
        public static bool edit = false;



        public static void zerujDane ()
        {
             SupplierId = 0;
             SupplierName = "";
             SupplierEmail = "";
            add = false;
            edit = false;
    }
      
    }
}
