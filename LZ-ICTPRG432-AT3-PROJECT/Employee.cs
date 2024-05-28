using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LZ_ICTPRG432_AT3_PROJECT
{
    public class Employee
    {
        //Define Connection Details
        private string dbName = "LZ_ictprg431";
        private string dbUser = "root";
        private string dbPassword = " ";
        private int dbPort = 3306;
        private string dbServer = "localhost";
        // Connection String and MySQL Connection
        private string dbConnectionString = "";
        private MySqlConnection conn;

        public string SearchEmployeeName {  get; set; }
        public int ID { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public int GrossSalary { get; set; }
        public Employee()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
            try
            {
                conn.Open();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("No DB Connection");
            }
            finally
            {
                conn.Close();
            }
        }

        public void SearchEmployee(string searchdata)
        {
            try
            {

                string sqlToRun = "Select id, given_name, family_name, gross_salary from employees where CONCAT(given_name, ' ', family_name) like '%" + searchdata + "%';";
                SearchEmployeeName = searchdata;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlToRun, conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    ID = Convert.ToInt32(rdr["id"].ToString());
                    GivenName = rdr["given_name"].ToString();
                    FamilyName = rdr["family_name"].ToString();
                    GrossSalary = int.Parse(rdr["gross_salary"].ToString());
                }
                conn.Close();
            }
            catch
            {
                MessageBox.Show("No record found");
            }
        }

        public void UpdateSalary()
        {
            try
            {

                string sqlToRun = "Update employees set gross_salary = " + GrossSalary + " where CONCAT(given_name, ' ', family_name) like '%" + SearchEmployeeName + "%';";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlToRun, conn);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("No record found");
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
