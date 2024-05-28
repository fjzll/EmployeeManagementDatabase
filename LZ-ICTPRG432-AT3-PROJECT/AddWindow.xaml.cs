using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LZ_ICTPRG432_AT3_PROJECT
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        //MySQL connection
        string dbConnectionString = "server=localhost; user=root; database=LZ_ictprg431; port=3306; password=''";

        public void AddEmployee()
        {
            MySqlConnection conn = new MySqlConnection(dbConnectionString);
            string sqlQuery = "Insert into LZ_ictprg431.employees (ID, given_name, family_name, date_of_birth, gender_identity, gross_salary, supervisor_id, branch_id)" +
                "values ('" + this.IDTextBox.Text + "','" + this.GivenNameTextBox.Text + "','" + this.FamilyNameTextBox.Text + "','" + this.DoBTextBox.Text + "','" + this.GenderTextBox.Text + "','" + this.GrossSalaryTextBox.Text + "','" + this.SupervisorIDTextBox.Text + "','" + this.BranchIDTextBox.Text + "')";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("New Employee Added");
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        public void ClearData()
        {
            IDTextBox.Clear();
            GivenNameTextBox.Clear();
            FamilyNameTextBox.Clear();
            DoBTextBox.Clear();
            GenderTextBox.Clear();
            GrossSalaryTextBox.Clear();
            SupervisorIDTextBox.Clear();
            BranchIDTextBox.Clear();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
 
    }
}
