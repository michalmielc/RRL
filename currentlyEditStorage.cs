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
    public class currentlyEditStorage
    {
        public static int StorageId = 0;
        public static string StorageName = "";
        public static int CurrentIventory = 0;
        public static int MaxIventory = 0;
        public static int DiffIventory = 0;
        public static int Id= 0;
        public static string ItemName = "";

        public static void zerujDane ()
        {
        StorageId = 0;
        StorageName = "";
        CurrentIventory = 0;
        MaxIventory = 0;
        DiffIventory = 0;
        Id = 0;
        ItemName = "";
        }
      
    }
}
