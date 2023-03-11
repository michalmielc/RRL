using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.ComponentModel;
using System.Data.Sql;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Windows.Forms;
using System.IO;

namespace RRL
{
    public class polaczenieBazaDanych
    {

        // ŁAŃCUCH POŁĄCZENIA
        //public string db_path = "Data Source=PCS03;Initial Catalog=rrl;Integrated Security=True";
        //public string db_path = System.IO.File.ReadAllText(@System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\..\\..\\settings\\sql.txt");

        public string db_path = "Data Source=DESKTOP-ER50C2B\\SQLEXPRESS;Initial Catalog = rrl; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public string loggedUser = "";
        public string loggedUserdepartment = "";
        // TEST POŁĄCZENIA

        public bool CheckDbConnection(string connectionString)
        {
            MessageBox.Show(db_path);

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false; // any error is considered as db connection error for now
            }
        }


        // LOGOWANIE: SPRAWDZENIE CZY HASŁO ISTNIEJE I CZY USER JEST AKTYWNY

        public bool checkUserPassword(string haslo)
        {
            bool istnieje = false;

            using (SqlConnection conn = new SqlConnection(db_path))
            {
             
                    SqlCommand cmd = new SqlCommand("select dbo.checkuserpassword_and_status(@haslo)", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@haslo", haslo));
           
                try
                {
                    conn.Open();
                    var objekt = cmd.ExecuteScalar();

                    if ((bool)objekt)
                    { istnieje = true; }

                    else
                    {
                        istnieje = false;
                    }


                    conn.Close();
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return istnieje;
        }

        // LOGOWANIE: SPRAWDZENIE CZY HASŁO ISTNIEJE
        public bool checkUserPassword2(string haslo)
        {
            bool istnieje = false;

            using (SqlConnection conn = new SqlConnection(db_path))
            {

                SqlCommand cmd = new SqlCommand("select dbo.checkuserpassword(@haslo)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@haslo", haslo));

                try
                {
                    conn.Open();
                    var objekt = cmd.ExecuteScalar();

                    if ((bool)objekt)
                    { istnieje = true;
                        MessageBox.Show("PODANE HASŁO ISTNIEJE");
                    }

                    else
                    {
                        istnieje = false;
                    }


                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());                   
                }
            }
            return istnieje;
        }



        // LOGOWANIE: POBRANIE INFORMACJI O UŻYTKOWNIKU
        public void loadDetailsLoggeduser(string haslo)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
        
            try
            {


                conn = new
                    SqlConnection(db_path);
                conn.Open();



                SqlCommand cmd1 = new SqlCommand("select dbo.getuserid(@haslo)", conn);
                cmd1.Parameters.Add(new SqlParameter("@haslo", haslo));
                cmd1.CommandType = CommandType.Text;

                var objekt1 = cmd1.ExecuteScalar();

                SqlCommand cmd2 = new SqlCommand("select dbo.getusername(@haslo)", conn);
                cmd2.Parameters.Add(new SqlParameter("@haslo", haslo));
                cmd2.CommandType = CommandType.Text;

                var objekt2 = cmd2.ExecuteScalar();

                SqlCommand cmd3 = new SqlCommand("select dbo.getuserdepartment(@haslo)", conn);
                cmd3.Parameters.Add(new SqlParameter("@haslo", haslo));
                cmd3.CommandType = CommandType.Text;

                var objekt3 = cmd3.ExecuteScalar();


                SqlCommand cmd4 = new SqlCommand("select dbo.getuserrights_on_password(@haslo)", conn);
                cmd4.Parameters.Add(new SqlParameter("@haslo", haslo));
                cmd4.CommandType = CommandType.Text;

                var objekt4 = cmd4.ExecuteScalar();
                int upr = int.Parse(objekt4.ToString());

                //ŁADOWANIE UPRAWNIEŃ

                string sqlQuery = "SELECT *FROM ViewUSERGROUPS WHERE UserGroupId =" + upr;
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                       currentlyData.OptionAdministrator  = bool.Parse( rdr["OptionAdministrator"].ToString());
                       currentlyData.OptionReceive = bool.Parse(rdr["OptionReceive"].ToString());
                       currentlyData.OptionReceipt = bool.Parse(rdr["OptionReceipt"].ToString());
                       currentlyData.OptionEditUser = bool.Parse(rdr["OptionEditUser"].ToString());
                       currentlyData.OptionEditItem = bool.Parse(rdr["OptionEditItem"].ToString());
                       currentlyData.OptionEditStorageplace = bool.Parse(rdr["OptionEditStorageplace"].ToString());
                       currentlyData.OptionReports = bool.Parse(rdr["OptionReports"].ToString());
                       currentlyData.OptionUsergroup = bool.Parse(rdr["OptionGroupUser"].ToString());
                }

                rdr.Close();
                //ŁADOWANIE OPCJI MPK

                            
                string sqlQuery1 = "SELECT *FROM ViewCOSTCENTERSOPTIONS";
                SqlCommand command1 = new SqlCommand(sqlQuery1, conn);
                
                rdr = command1.ExecuteReader();

                while (rdr.Read())
                {
                    currentlyCostCenter.activefill= bool.Parse(rdr["ActiveFill"].ToString());
                    currentlyCostCenter.activewithdraw= bool.Parse(rdr["ActiveWithdraw"].ToString());
                    currentlyCostCenter.activeedit = bool.Parse(rdr["ActiveEdit"].ToString());
      
                }

                //USTAWIENIE ZMIENNYCH GLOBALNYCH


                currentlyData.UserId = int.Parse(objekt1.ToString());
                currentlyData.UserName = objekt2.ToString();
                currentlyData.UserDepartment = objekt3.ToString();

                DateTime data = DateTime.Now;

                int month = int.Parse(data.Month.ToString());
                int day = int.Parse(data.Day.ToString());
                int hour = int.Parse(data.Hour.ToString());
                int minute = int.Parse(data.Minute.ToString());
                int second = int.Parse(data.Second.ToString());

                string mo ="";
                string da="";
                string ho = "";
                string mi = "";
                string se = "";

                if (month<10)
                { mo = "0" + month.ToString();
                }

                else
                {
                    mo = month.ToString();
                }

                if (day < 10)
                {
                    da = "0" + day.ToString();
                }
                else
                {
                    da =  day.ToString();
                }


                if (hour < 10)
                {
                    ho = "0" + hour.ToString();
                }
                else
                {
                    ho = hour.ToString();
                }


                if (minute < 10)
                {
                    mi = "0" + minute.ToString();
                }
                else
                {
                    mi = minute.ToString();
                }


                if (second < 10)
                {
                    se = "0" + second.ToString();
                }
                else
                {
                    se = second.ToString();
                }

                string czas = data.Year.ToString() + "-" + mo + "-" + da + "  " + ho+ ":" + mi + ":" + se;


              //  ZAPIS LOGOWANIA DO DZIENNIKA

                SqlCommand cmd5 = new SqlCommand("addlog", conn);
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.Add(new SqlParameter("@Name", objekt2.ToString()));
                cmd5.Parameters.Add(new SqlParameter("@Department", objekt3.ToString()));
                cmd5.Parameters.Add(new SqlParameter("@Date", czas));

                rdr.Close();

                 rdr = cmd5.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }


        }

        // GRUPY UZYTKOWNIKÓW: ŁADOWANIE LISTY

       public void loadUsersgroup(DataGridView dgv)
       {
           string sql ="select*from dbo.ViewUSERGROUPS where UserGroupId <>1";
           SqlConnection connection = new SqlConnection(db_path);
           SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
           DataSet ds = new DataSet();

           connection.Open();
           dataadapter.Fill(ds, "viewUSERSGROUP");
           connection.Close();
           dgv.DataSource = ds;
           dgv.DataMember = "viewUSERSGROUP";

       }

       // GRUPY UZYTKOWNIKÓW: DODAWANIE GRUPY UŻYTKOWNIKÓW
       public void addUsergroup(string name, byte x1, byte x2, byte x3, byte x4, byte x5, byte x6, byte x7, byte x8, byte x9, byte x10, byte x11)
       {


           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "addusersgroup", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@UserGroupName", name));
               cmd.Parameters.Add(new SqlParameter("@OptionAdministrator", x1));
               cmd.Parameters.Add(new SqlParameter("@OptionReceive", x2));
               cmd.Parameters.Add(new SqlParameter("@OptionReceipt", x3));
               cmd.Parameters.Add(new SqlParameter("@OptionEditUser", x4));
               cmd.Parameters.Add(new SqlParameter("@OptionEditItem", x5));
               cmd.Parameters.Add(new SqlParameter("@OptionEditStorageplace", x6));
               cmd.Parameters.Add(new SqlParameter("@OptionReports", x7));
               cmd.Parameters.Add(new SqlParameter("@OptionGroupUser", x8));
               cmd.Parameters.Add(new SqlParameter("@OptionSupplier", x9));
               cmd.Parameters.Add(new SqlParameter("@OptionCostcenter", x10));
               cmd.Parameters.Add(new SqlParameter("@OptionStocktaking", x11));

                // execute the command
                rdr = cmd.ExecuteReader();

               MessageBox.Show("Dodano nową grupę uzytkownika: " + name);

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ;
       }

       // GRUPY UZYTKOWNIKÓW: EDYCJA GRUPY UŻYTKOWNIKÓW
       public void editUsergroup(int id, string name, byte x1, byte x2, byte x3, byte x4, byte x5, byte x6, byte x7, byte x8, byte x9, byte x10, byte x11) 
       {


           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "editusersgroup", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@UserGroupId", id));
               cmd.Parameters.Add(new SqlParameter("@UserGroupName", name));
               cmd.Parameters.Add(new SqlParameter("@OptionAdministrator", x1));
               cmd.Parameters.Add(new SqlParameter("@OptionReceive", x2));
               cmd.Parameters.Add(new SqlParameter("@OptionReceipt", x3));
               cmd.Parameters.Add(new SqlParameter("@OptionEditUser", x4));
               cmd.Parameters.Add(new SqlParameter("@OptionEditItem", x5));
               cmd.Parameters.Add(new SqlParameter("@OptionEditStorageplace", x6));
               cmd.Parameters.Add(new SqlParameter("@OptionReports", x7));
               cmd.Parameters.Add(new SqlParameter("@OptionGroupUser", x8));
                cmd.Parameters.Add(new SqlParameter("@OptionSupplier", x9));
                cmd.Parameters.Add(new SqlParameter("@OptionCostcenter", x10));
                cmd.Parameters.Add(new SqlParameter("@OptionStocktaking", x11));


                // execute the command
                rdr = cmd.ExecuteReader();

               MessageBox.Show("ZMIENIONO DANE! Grupa użytkowników: " + name);

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ; ;
       }

       // GRUPY UZYTKOWNIKÓW: USUWANIE GRUPY UŻYTKOWNIKÓW
       public void delUsergroup(int id)
       {

           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "delusergroup", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@id", id));



               // execute the command
               rdr = cmd.ExecuteReader();

           }


           catch
           {
               MessageBox.Show("Istnieje przynajmnie jeden user, który ma przypisaną grupę", "BŁĄD", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ;
       }

        // GRUPY UZYTKOWNIKÓW: SPRAWDZENIE CZY NAZWA ISTNIEJE
        public bool checkUserGroupName(string name)
        {
            bool istnieje = false;

            using (SqlConnection conn = new SqlConnection(db_path))
            {

                SqlCommand cmd = new SqlCommand("select dbo.checkusergroupname(@name)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@name", name));

                try
                {
                    conn.Open();
                    var objekt = cmd.ExecuteScalar();

                    if ((bool)objekt)
                    {
                        istnieje = true;
                        MessageBox.Show("PODANA NAZWA ISTNIEJE. WPISZ INNĄ!");
                    }

                    else
                    {
                        istnieje = false;
                    }


                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return istnieje;
        }

        // UŻYTKOWNICY: WYŚWIETLANIE WSZYTSKICH

        public void loadUsers(DataGridView dgv)
       {
           string sql = "SELECT * from viewUSERSwithUsergroups WHERE UserName NOT like'admin';";


           SqlConnection connection = new SqlConnection(db_path);
           SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
           DataSet ds = new DataSet();

           connection.Open();
           dataadapter.Fill(ds, "users");
           connection.Close();
           dgv.DataSource = ds;
           dgv.DataMember = "users";

       }

        // UŻYTKOWNICY: WYŚWIETLANIE UŻYTKOWNIKÓW PO PODANYCH PARAMETRACH
       public void loadUsers_all_on_text(DataGridView dgv, string parametr, string atrybut)
       {
           if (atrybut == "wszystkie")
           {
               string sql = "SELECT * from viewUSERSwithUsergroups WHERE UserName NOT like'admin'";
               SqlConnection connection = new SqlConnection(db_path);
               SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
               DataSet ds = new DataSet();

               connection.Open();
               dataadapter.Fill(ds, "users");
               connection.Close();
               dgv.DataSource = ds;
               dgv.DataMember = "users";
           }

           if (atrybut == "po_nazwach")
           {
               string sql = "SELECT * from viewUSERSwithUsergroups  WHERE UserName LIKE " + "'%" + parametr + "%' and UserName NOT like'admin'";
               SqlConnection connection = new SqlConnection(db_path);
               SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
               DataSet ds = new DataSet();

               connection.Open();
               dataadapter.Fill(ds, "users_");
               connection.Close();
               dgv.DataSource = ds;
               dgv.DataMember = "users_";
           }

       }

       // UŻYTKOWNICY: DODAJ UŻYTKOWNIKA
       public void addUser(string name, string department, int ugid, string password, byte active)
       {

           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "adduser", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@UserName", name));
               cmd.Parameters.Add(new SqlParameter("@Department", department));
               cmd.Parameters.Add(new SqlParameter("@Rights", ugid));
               cmd.Parameters.Add(new SqlParameter("@Password", password));
               cmd.Parameters.Add(new SqlParameter("@Active", active));

       
               // execute the command
               rdr = cmd.ExecuteReader();

               MessageBox.Show("Dodano nowego uzytkownika: " + name);

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ;
       }

       // UŻYTKOWNICY: EDYTUJ UŻYTKOWNIKA
       public void editUser(int id, string name, string department, int ugid, string password, byte active )
       {


           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "edituser", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;


               cmd.Parameters.Add(new SqlParameter("@UserId", id));
               cmd.Parameters.Add(new SqlParameter("@UserName", name));
               cmd.Parameters.Add(new SqlParameter("@Department", department));
               cmd.Parameters.Add(new SqlParameter("@Rights", ugid));
               cmd.Parameters.Add(new SqlParameter("@Password", password));
               cmd.Parameters.Add(new SqlParameter("@Active", active));
               
               // execute the command
               try
               {
                   rdr = cmd.ExecuteReader();
               }

               catch (Exception ex)
               {
                   MessageBox.Show("PODANE HASŁO ISTNIEJE!!!" + ex.ToString(), "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return;

               }
               MessageBox.Show("ZMIENIONO DANE! UŻYTKOWNIK: " + name);

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ; ;
       }

        // UŻYTKOWNICY: SPRAWDZENIE CZY HASŁO ISTNIEJE I CZY NALEŻY DO KOGOŚ INNEGO 

        public bool checkUserPassword3(string haslo, int id)
        {
            bool istnieje = false;

            using (SqlConnection conn = new SqlConnection(db_path))
            {

                SqlCommand cmd = new SqlCommand("select dbo.checkuserpassword_at_edit(@haslo,@id)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@haslo", haslo));
                cmd.Parameters.Add(new SqlParameter("@id", id));
                try
                {
                    conn.Open();
                    var objekt = cmd.ExecuteScalar();

                    if ((bool)objekt)
                    { istnieje = true; }

                    else
                    {
                        istnieje = false;
                    }


                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return istnieje;
        }

        // UŻYTKOWNICY: SPRAWDZENIE ID GRUPY UŻYTKOWNIKA PO NAZWIE GRUPY
        public int checkUsergroupId(string nazwa)
       {
             SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {


                conn = new
                    SqlConnection(db_path);
                conn.Open();



                SqlCommand cmd1 = new SqlCommand("select dbo.getuserrights_on_name(@name)", conn);
                cmd1.Parameters.Add(new SqlParameter("@name", nazwa));
                cmd1.CommandType = CommandType.Text;

                var objekt1 = cmd1.ExecuteScalar();

                return int.Parse(objekt1.ToString());
            }

        finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                };
            }
            
       
    
            
       }

       // UŻYTKOWNICY: USUŃ UŻYTKOWNIKA
        public void delUser(int id)
       {



           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {


               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "deluser", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@UserId", id));

               // execute the command
               rdr = cmd.ExecuteReader();

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ;
       }

       //UŻYTKOWNICY: ŁADOWANIE NAZW GRUP UŻYTKOWNIKÓW
       public void loadUsergroups_combobox(ComboBox cbx)
       {


           string Sql = "select UserGroupName from ViewUsergroups where UserGroupId<>1";
           SqlConnection conn = new SqlConnection(db_path);
           conn.Open();
           SqlCommand cmd = new SqlCommand(Sql, conn);
           SqlDataReader DR = cmd.ExecuteReader();

           while (DR.Read())
           {
               cbx.Items.Add(DR[0]);

           }
       }
        
       // ARTYKUŁY: WYŚWIETLANIE WSZYTSKICH

       public void loadItems(DataGridView dgv)
       {
           string sql = "SELECT * from ViewItems_Amounts_adv;";


           SqlConnection connection = new SqlConnection(db_path);
           SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
           DataSet ds = new DataSet();

           connection.Open();
           dataadapter.Fill(ds, "items");
           connection.Close();
           dgv.DataSource = ds;
           dgv.DataMember = "items";



        }

        // ARTYKUŁY: WYŚWIETLANIE WSZYTSKICH AKTYWNYCH DO POBRANIA

        public void loadItemsToPickOrWith(DataGridView dgv)
        {
            string sql = "SELECT * from viewITEMS_AMOUNTS;";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "items");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "items";




        }

        //   // ARTYKUŁY: WYŚWIETLANIE WSZYTSKICH AKTYWNYCH DO POBRANIA PO PODANYCH PARAMETRACH
        public void loadItemsToPick_all_on_text(DataGridView dgv, string parametr, string atrybut)
        {
            if (atrybut == "wszystkie")
            {
                string sql = "SELECT * FROM ViewITEMS_AMOUNTS";
                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "items");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "items";
            }

            if (atrybut == "po_nazwach")
            {
                string sql = "SELECT *FROM ViewITEMS_AMOUNTS WHERE ItemName1 LIKE " + "'%" + parametr + "%' OR ItemName2 LIKE " + "'%" + parametr + "%' OR ItemName3 LIKE " + "'%" + parametr + "%'                   OR Barcode LIKE " + "'%" + parametr + "%'";

                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "users_");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "users_";
            }

        }

        // ARTYKUŁY: WYŚWIETLANIE ARTYKUŁÓW PO PODANYCH PARAMETRACH
        public void loadItems_all_on_text(DataGridView dgv, string parametr, string atrybut)
       {
           if (atrybut == "wszystkie")
           {
               string sql = "SELECT * FROM ViewItems_Amounts_adv";
               SqlConnection connection = new SqlConnection(db_path);
               SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
               DataSet ds = new DataSet();

               connection.Open();
               dataadapter.Fill(ds, "items");
               connection.Close();
               dgv.DataSource = ds;
               dgv.DataMember = "items";
           }

           if (atrybut == "po_nazwach")
           {
               string sql = "SELECT *from ViewItems_Amounts_adv WHERE ItemName1 LIKE " + "'%" + parametr + "%' OR ItemName2 LIKE " + "'%" + parametr + "%' OR ItemName3 LIKE " + "'%" + parametr + "%'  OR Barcode LIKE " + "'%" + parametr + "%'" ;

               SqlConnection connection = new SqlConnection(db_path);
               SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
               DataSet ds = new DataSet();

               connection.Open();
               dataadapter.Fill(ds, "items_");
               connection.Close();
               dgv.DataSource = ds;
               dgv.DataMember = "items_";
           }

       }

        // ARTYKUŁY: DODAJ ARTYKUŁ
        public void addItem(string name1, string name2, string name3, string barcode, string picpath, byte active, decimal price, int min,string supplier)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "additem", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Name1", name1));
                cmd.Parameters.Add(new SqlParameter("@Name2", name2));
                cmd.Parameters.Add(new SqlParameter("@Name3", name3));
                cmd.Parameters.Add(new SqlParameter("@Barcode", barcode));
                cmd.Parameters.Add(new SqlParameter("@Picpath", picpath));
                cmd.Parameters.Add(new SqlParameter("@Active", active));
                cmd.Parameters.Add(new SqlParameter("@Price", price));
                cmd.Parameters.Add(new SqlParameter("@MinInv", min));
                cmd.Parameters.Add(new SqlParameter("@Supplier", supplier));
                // execute the command
                rdr = cmd.ExecuteReader();

                MessageBox.Show("DODANO NOWY ARTYKUŁ: " + name1);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        // ARTYKUŁY: EDYTUJ ARTYKUŁ
        public void editItem(int id, string name1, string name2, string name3, string barcode, string picpath, byte active, decimal price, int min, string supplier)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "edititem", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ItemId", id));
                cmd.Parameters.Add(new SqlParameter("@Name1", name1));
                cmd.Parameters.Add(new SqlParameter("@Name2", name2));
                cmd.Parameters.Add(new SqlParameter("@Name3", name3));
                cmd.Parameters.Add(new SqlParameter("@Barcode", barcode));
                cmd.Parameters.Add(new SqlParameter("@Picpath", picpath));
                cmd.Parameters.Add(new SqlParameter("@Active", active));
                cmd.Parameters.Add(new SqlParameter("@Price", price));
                cmd.Parameters.Add(new SqlParameter("@MinInv", min));
                cmd.Parameters.Add(new SqlParameter("@Supplier", supplier));



                // execute the command
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                        return;

                }
                MessageBox.Show("ZMIENIONO ARTYKUŁ: " + name1);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        // ARTYKUŁY: USUŃ ARTYKUŁ
        public void delItem(int id)
       {
            

           SqlConnection conn = null;
           SqlDataReader rdr = null;
           try
           {
                
               // create and open a connection object
               conn = new
                   SqlConnection(db_path);
               conn.Open();

               // 1. create a command object identifying
               // the stored procedure
               SqlCommand cmd = new SqlCommand(
                   "delitem", conn);

               // 2. set the command object so it knows
               // to execute a stored procedure
               cmd.CommandType = CommandType.StoredProcedure;

               cmd.Parameters.Add(new SqlParameter("@ItemId", id));

               // execute the command
               rdr = cmd.ExecuteReader();

           }

           finally
           {
               if (conn != null)
               {
                   conn.Close();
               }
               if (rdr != null)
               {
                   rdr.Close();
               }
           }

           ;
       }

        //ARTYKUŁY: ŁADOWANIE DOSTAWCÓW
        public void loadSupplier_combobox(ComboBox cbx)
        {


            string Sql = "select SupplierName from ViewSuppliers";
            SqlConnection conn = new SqlConnection(db_path);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Sql, conn);
            SqlDataReader DR = cmd.ExecuteReader();

            while (DR.Read())
            {
                cbx.Items.Add(DR[0]);

            }
        }

        //  ARTYKUŁY: POBIERZ/PRZYJMIJ  - WCZYTAJ ŚCIEŻKĘ ZDJĘCIA
        public string readItempicpath(int id)
        {



            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {

                

                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand("select dbo.getitempicpath(@ItemID)", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add(new SqlParameter("@ItemId", id));

                // execute the command
               // rdr = cmd.ExecuteReader();


                var objekt1 = cmd.ExecuteScalar();

                return (objekt1.ToString());

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

           ;
        }

        //  ARTYKUŁY: POBIERZ/PRZYJMIJ  - ZMIEŃ STAN
        public void changeItemamount(int id, int amount, bool fill)
        {
            
            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {
                
                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "changeitemamount", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ItemId", id));
                cmd.Parameters.Add(new SqlParameter("@Amount", amount));
                cmd.Parameters.Add(new SqlParameter("@Fill", fill));
                // execute the command
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

           ;
        }

        //  ARTYKUŁY: POBIERZ/PRZYJMIJ  - ZMIEŃ STAN W STORAGEPLACE
        public void changeItemamountStorageplace(int id, int amount, bool fill)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {

                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "changeitemamount_storageplace", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ItemId", id));
                cmd.Parameters.Add(new SqlParameter("@Amount", amount));
                cmd.Parameters.Add(new SqlParameter("@Fill", fill));
                // execute the command
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

           ;
        }

        //RAPORTY: LISTA LOGOWAŃ
        public void loadLoggedusers(DataGridView dgv)
        {
            string sql = "SELECT * FROM dbo.ViewLOGS_TABLE";
            //string sql = "SELECT * FROM dbo.LOGS_TABLE";

            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds,"logowania");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "logowania";

        }

        // RAPORTY: LISTA UŻYTKOWNIKÓW
        public void loadListofusers(DataGridView dgv)
        {
            string sql = "SELECT * FROM users";
            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "users");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "users";

        }

        // DOSTAWCY: DODAJ DOSTAWCĘ
        public void addSupplier(string name, string email)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "addsupplier", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Name", name));
                cmd.Parameters.Add(new SqlParameter("@Email", email));

                // execute the command
                rdr = cmd.ExecuteReader();

                MessageBox.Show("Dodano nowego dostawcę: " + name);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        // DOSTAWCY: EDYTUJ DOSTAWCĘ
        public void editSupplier(int id, string name, string email)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editsupplier", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@SupplierId", id));
                cmd.Parameters.Add(new SqlParameter("@Name", name));
                cmd.Parameters.Add(new SqlParameter("@Email", email));

         
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch

                {

                }
                MessageBox.Show("ZMIENIONO DANE! DOSTAWCA: " + name);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        // DOSTAWCĘ: USUŃ DOSTAWCĘ
        public void delSupplier(int id)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {

                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "delsupplier", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@SupplierId", id));

                // execute the command
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

           ;
        }

        // DOSTAWCY: WYŚWIETLANIE DOSTAWCÓW PO PODANYCH PARAMETRACH
        public void loadSuppliers_all_on_text(DataGridView dgv, string parametr, string atrybut)
        {
            if (atrybut == "wszystkie")
            {
                string sql = "SELECT * FROM ViewSUPPLIERS";
                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "suppliers");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "suppliers";
            }

            if (atrybut == "po_nazwach")
            {
                string sql = "SELECT *from ViewSUPPLIERS WHERE SupplierName LIKE " + "'%" + parametr + "%' OR SupplierEmail LIKE " + "'%" + parametr + "%'";

                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "suppliers_");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "suppliers_";
            }

        }

        // DOSTAWCY: WYŚWIETLANIE WSZYTSKICH

        public void loadSuppliers(DataGridView dgv)
        {
            string sql = "SELECT * from ViewSuppliers;";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "suppliers");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "suppliers";

            
        }

        // MPK: DODAJ MPK
        public void addCostcenter(string name)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "addcostcenter", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CostName", name));
        
                // execute the command
                rdr = cmd.ExecuteReader();

                MessageBox.Show("Dodano nowe centrum kosztów: " + name);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        // MPK: EDYTUJ MPK
        public void editCostCenter(int id, string name)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editcostcenter", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@CostId", id));
                cmd.Parameters.Add(new SqlParameter("@CostName", name));
     

                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch

                {

                }
                MessageBox.Show("ZMIENIONO DANE! DOSTAWCA: " + name);

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        //  MPK: USUŃ MPK
        public void delCostCenter(int id)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {

                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "delcostcenter", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CostId", id));

                // execute the command
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

           ;
        }

        //MPK: WYŚWIETLANIE MPK PO PODANYCH PARAMETRACH
        public void loadCostCenter_all_on_text(DataGridView dgv, string parametr, string atrybut)
        {
            if (atrybut == "wszystkie")
            {
                string sql = "SELECT * FROM ViewCOSTCENTERS";
                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "costcenters");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "costcenters";
            }

            if (atrybut == "po_nazwach")
            {
                string sql = "SELECT *from ViewCOSTCENTERS WHERE CostName LIKE " + "'%" + parametr + "%'";

                SqlConnection connection = new SqlConnection(db_path);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();

                connection.Open();
                dataadapter.Fill(ds, "costcenters_");
                connection.Close();
                dgv.DataSource = ds;
                dgv.DataMember = "costcenters_";
            }

        }

        // MPK: WYŚWIETLANIE WSZYTSKICH

        public void loadCostCenter(DataGridView dgv)
        {
            string sql = "SELECT * from ViewCOSTCENTERS;";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "costcenters");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "costcenters";


        }


        // CUBBIES: DODAJ CUBBY
        public void addCubby()
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "addcubby", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                
                rdr = cmd.ExecuteReader();

                MessageBox.Show("DODANO NOWY KONTENER " );

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        // CUBBIES: USUŃ CUBBY
        public void delCubby( int id)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "delcubby", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CubbyId", id));
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        // CUBBIES: ZMIANA POZYCJI CUBBY
        public void editCubby(int id, string name, int pozx, int pozy, int wys, int szer)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editcubby", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@CubbyId", id));
                cmd.Parameters.Add(new SqlParameter("@CubbyName", name));
                cmd.Parameters.Add(new SqlParameter("@PositionX", pozx));
                cmd.Parameters.Add(new SqlParameter("@PositionY", pozy));
                cmd.Parameters.Add(new SqlParameter("@Height", wys));
                cmd.Parameters.Add(new SqlParameter("@Width", szer));

                // execute the command
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
        

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        // CUBBIES: ZMIANA POZYCJI CUBBY
        public void editCubbyName(int id, string name)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editcubbyname", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@CubbyId", id));
                cmd.Parameters.Add(new SqlParameter("@CubbyName", name));
       

                // execute the command
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
              
            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        // CUBBIES: ZMIANA POZYCJI CUBBY
        public void editCubbySize(int id, int x, int y)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editcubbysize", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@CubbyId", id));
                cmd.Parameters.Add(new SqlParameter("@Height", x));
                cmd.Parameters.Add(new SqlParameter("@Width", y));

                // execute the command
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        //ŁADUJ CUBBIES
        public void loadCubbies(DataGridView dgv)
        {
            string sql = "select*from dbo.ViewCUBBIES";
            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "viewCUBBIES");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "viewCUBBIES";

        }

        //CUBBIES: NASTĘPNY WOLNY ID - POTRZEBNY PRZY TWORZENIU
        public int nextfreeCubbieid()

        {
            int wynik =0;

            using (SqlConnection conn = new SqlConnection(db_path))
            {

                SqlCommand cmd = new SqlCommand("select dbo.getnextidCubbies()", conn);
                cmd.CommandType = CommandType.Text;
              
                try
                {
                    conn.Open();
                    var objekt = cmd.ExecuteScalar();
                    wynik = int.Parse(objekt.ToString());
                    conn.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            return wynik;
        }

        //STORAGEPLACES ITEMS: ŁADOWANIE ITEMS DO PRZYPORZĄDKOWNAIA DO CUBBY
        public void loadItems_cubbies(DataGridView dgv)
        {
            

            string sql = "select *from ViewITEMS_AMOUNTS_for_cubbies";
        
            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "ViewITEMS_AMOUNTS_for_cubbies");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "ViewITEMS_AMOUNTS_for_cubbies";
        }

        // STORAGEPLACE: DODAJ STORAGEPLACE
        public void addStoraplace(int cubbyid, string cubbyname, string name1, int itemid, string itemname, int max)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "addstorageplace", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@CubbyId", cubbyid));
                cmd.Parameters.Add(new SqlParameter("@CubbyName", cubbyname));
                cmd.Parameters.Add(new SqlParameter("@StorageName", name1));
                cmd.Parameters.Add(new SqlParameter("@ItemId", itemid));
                cmd.Parameters.Add(new SqlParameter("@ItemName", itemname));
              //  cmd.Parameters.Add(new SqlParameter("@CurrentIventory", 0));
                cmd.Parameters.Add(new SqlParameter("@MaxInventory", max));
                // execute the command
                rdr = cmd.ExecuteReader();


            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

        //STORAGEPLACES ITEMS: ŁADOWANIE STORAGEPLACES WG ZAZNACZONEGO CUBBY 
        public void loadStorageplaces(DataGridView dgv, int x)
        {


            string sql = "select*from ViewSTORAGEPLACES where CubbyId = "+x;

            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "ViewSTORAGEPLACES");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "ViewSTORAGEPLACES";
        }
        //STORAGEPLACES ITEMS: ŁADOWANIE STORAGEPLACES SKRÓCONY WG ZAZNACZONEGO CUBBY 
        public void loadStorageplaces2(DataGridView dgv, int x)
        {


            string sql = "select*from ViewSTORAGEPLACES where CubbyId = " + x;

            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "ViewSTORAGEPLACES");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "ViewSTORAGEPLACES";
        }

        //STORAGEPLACES ITEMS: ŁADOWANIE STORAGEPLACES all
        public void loadStorageplaces_all(DataGridView dgv)
        {


            string sql = "select*from ViewSTORAGEPLACES";

            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "ViewSTORAGEPLACES");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "ViewSTORAGEPLACES";
        }

        // STORAGEPLACES : USUŃ STORAGEPLACE
        public void delStorageplace(int id)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "delstorageplace", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StorageId", id));
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }


        // STORAGEPLACES : USUŃ STORAGEPLACE
        public void delAllStorageplace()
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "delallstorageplace", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
             
                rdr = cmd.ExecuteReader();

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }


        //WAREHOUSE: LISTA ZAZNACZONYCH ITEMÓW DO POBRANIA
        public void markedItems(DataGridView dgv)
        {
            string sql = "select*from dbo.ViewSTORAGEPLACES_TEMP";
            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "ViewSTORAGEPLACES_TEMP");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "ViewSTORAGEPLACES_TEMP";

        }

        // LOGOWANIE: POBRANIE INFORMACJI O UZYTKOWNIKU
        public int getMaxAmountToFill(int ItemId)
        {

            SqlConnection conn = null;
            SqlDataReader rdr = null;

            try
            {


                conn = new
                    SqlConnection(db_path);
                conn.Open();



                SqlCommand cmd1 = new SqlCommand("select dbo.getmaxamounttofill(@ItemId)", conn);
                cmd1.Parameters.Add(new SqlParameter("@ItemId", ItemId));
                cmd1.CommandType = CommandType.Text;

                var objekt1 = cmd1.ExecuteScalar();

                return int.Parse(objekt1.ToString());
            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }


        }

        // COST CENTERS: ZMIANA USTAWIEŃ
        public void editCostCentersOptions(bool activefill, bool activewithdraw, bool activeedit)
        {


            SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "editcostcentersoptions", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ActiveFill", activefill));
                cmd.Parameters.Add(new SqlParameter("@ActiveWithdraw", activewithdraw));
                cmd.Parameters.Add(new SqlParameter("@Activeedit", activeedit));


                // execute the command
                try
                {
                    rdr = cmd.ExecuteReader();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("BŁĄD" + ex.ToString(), "BŁAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
            

            }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ; ;
        }

        //FILLWITHDRAW: ŁADOWANIE NAZW MPK - ÓW
        public void loadCostCenters_combobox(ComboBox cbx)
        {


            string Sql = "select CostName from ViewCOSTCENTERS";
            SqlConnection conn = new SqlConnection(db_path);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Sql, conn);
            SqlDataReader DR = cmd.ExecuteReader();

            while (DR.Read())
            {
                cbx.Items.Add(DR[0]);

            }
        }

        // RAPORTY: WYŚWIETLANIE POBRAŃ

        public void loadWithdraws(DataGridView dgv)
        {
            string sql = "SELECT * from ViewWAREHOUSEOPERATIONS_all WHERE Operation like 'POBRANIE';";
          

            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "withdraws");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "withdraws";



        }

        // RAPORTY: WYŚWIETLANIE PRZYJĘĆ

        public void loadFillings(DataGridView dgv)
        {
            string sql = "SELECT * from ViewWAREHOUSEOPERATIONS_all WHERE Operation like 'UZUPEŁNIANIE';";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "fillings");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "fillings";



        }

        // RAPORTY: WYŚWIETLANIE KOREKTY STANÓW

        public void loadStockTaking(DataGridView dgv)
        {
            string sql = "SELECT * from ViewWAREHOUSEOPERATIONS_all WHERE Operation like 'KOREKTA';";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "stocktaking");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "stocktaking";



        }

        // RAPORTY: WYŚWIETLANIE WSZYTSKIE RUCHY MAGAZYNOWE

        public void loadWarehouseOperations(DataGridView dgv)
        {
            string sql = "SELECT * from ViewWAREHOUSEOPERATIONS_all;";


            SqlConnection connection = new SqlConnection(db_path);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();

            connection.Open();
            dataadapter.Fill(ds, "warehouseoperations");
            connection.Close();
            dgv.DataSource = ds;
            dgv.DataMember = "warehouseoperations";



        }

        // RAPORT KSIĘGOWAŃ: UZUEPŁNIANIE WPISÓW WSZYSTKICH OPERACJI
        public void addwarehouseoperations(string name, string department, string item_number, string item_name, int amount, string storage, string cost, string operation )
        {
             
           string date = DateTime.Now.ToShortDateString();
           string hour = DateTime.Now.ToString("HH:mm:ss");
             SqlConnection conn = null;
            SqlDataReader rdr = null;
            try
            {


                // create and open a connection object
                conn = new
                    SqlConnection(db_path);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand(
                    "addwarehouseoperation", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Name", name));
                cmd.Parameters.Add(new SqlParameter("@Department", department));
                cmd.Parameters.Add(new SqlParameter("@Date", date));
                cmd.Parameters.Add(new SqlParameter("@Time", hour));
                cmd.Parameters.Add(new SqlParameter("@ItemNo", item_number));
                cmd.Parameters.Add(new SqlParameter("@ItemName", item_name));
                cmd.Parameters.Add(new SqlParameter("@Amount", amount));
                cmd.Parameters.Add(new SqlParameter("@Storage", storage));
                cmd.Parameters.Add(new SqlParameter("@CostCenter", cost));
                cmd.Parameters.Add(new SqlParameter("@Operation", operation));



                // execute the command
                rdr = cmd.ExecuteReader();

              }

            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            ;
        }

    }
}