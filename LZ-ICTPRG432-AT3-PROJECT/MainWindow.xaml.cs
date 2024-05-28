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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;


namespace LZ_ICTPRG432_AT3_PROJECT
{
    /// <summary>
    /// SQL Project 3
    /// Author: Lili Zheng
    /// Email: 20113384@tafe.wa.edu.au
    /// </summary>
    public partial class MainWindow : Window
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

        public MainWindow()
        {
            InitializeComponent();
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = "SELECT * FROM EMPLOYEES;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            conn.Close();
        }

        private void SearchEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextbox.Text)) 
            {
                MessageBox.Show("Please enter a name in the search box");
                return;
            };
            string sqlQuery = "Select * from EMPLOYEES where CONCAT(given_name, ' ', family_name) like '%" + SearchTextbox.Text + "%';";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void SearchEmployeebySalaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(int.TryParse(SearchTextbox.Text, out int grossSalary)))
            {
                MessageBox.Show("Please enter the minimum salary you like to search.");
                return;
            }
            string sqlQuery = "Select * from EMPLOYEES where gross_salary >" + SearchTextbox.Text;
            try
            {

                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void SearchTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchTextbox.Clear();
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addEmployee = new AddWindow();
            addEmployee.ShowDialog();
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextbox.Text)) 
            {
                MessageBox.Show("Please enter the employee's name to delete");
                return;
            }
            string sqlQuery = "Delete from employees where CONCAT(given_name, ' ', family_name) like '%" + SearchTextbox.Text + "%';";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
                MessageBox.Show("Employee Deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void UpdateEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextbox.Text))
            {
                MessageBox.Show("Enter Employee Name in Search box ");
            }
            else
            {

                UpdateEmployee ob = new UpdateEmployee(SearchTextbox.Text);
                ob.ShowDialog();
            }
        }

        private void ShowSalesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextbox.Text))
            {
                MessageBox.Show("Please enter employee name.");
                return;
            }
            List<Sales> list = new List<Sales>();
            string sqlQuery = "Select employees.id, employees.given_name, employees.family_name, SUM(working_with.total_sales) AS total_sales From Employees LEFT JOIN working_with ON employees.id = working_with.employee_id where CONCAT(given_name, ' ', family_name) like '%" + SearchTextbox.Text + "%' Group by employees.id;";
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sales sales = new Sales();
                    sales.Given_Name = rdr["given_name"].ToString();
                    sales.Family_Name = rdr["family_name"].ToString();
                    sales.Total_Sales = Convert.ToInt32(rdr["total_sales"]);
                    list.Add(sales);

                }
                SalesDataGrid.ItemsSource = list;
            }
            catch(Exception ex)
            {
                MessageBox.Show("No sals records");
            }
            conn.Close();
        }



        private void branch1CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            branch2CheckBox.IsEnabled = false;
            branch3CheckBox.IsEnabled = false; 
            string sqlQuery = "SELECT * FROM employees WHERE branch_id = 1;";

            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void branch2CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            branch1CheckBox.IsEnabled = false;
            branch3CheckBox.IsEnabled = false;
            string sqlQuery = "SELECT * FROM employees WHERE branch_id = 2;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void branch3CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            branch1CheckBox.IsEnabled = false;
            branch2CheckBox.IsEnabled = false;
            string sqlQuery = "SELECT * FROM employees WHERE branch_id = 3;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void branch1CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            branch1CheckBox.IsEnabled = true;
            branch3CheckBox.IsEnabled = true;
            branch2CheckBox.IsEnabled=true;
            string sqlQuery = "SELECT * FROM employees;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }



        private void branch2CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            branch1CheckBox.IsEnabled = true;
            branch3CheckBox.IsEnabled = true;
            branch2CheckBox.IsEnabled = true;
            string sqlQuery = "SELECT * FROM employees;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();

        }

        private void branch3CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            branch1CheckBox.IsEnabled = true;
            branch3CheckBox.IsEnabled = true;
            branch2CheckBox.IsEnabled = true;
            string sqlQuery = "SELECT * FROM employees;";
            try
            {
                EmployeeListbox.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeListbox.Items.Add($"ID:{rdr["id"]}, Fisrt Name:{rdr["given_name"]}, Last name: {rdr["family_name"]}, Gender:{rdr["gender_identity"]}, Gloss Salary: {rdr["gross_salary"]}, Branch ID: {rdr["branch_id"]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }
    }

    public class Sales
    {
        public string Given_Name { get; set; }
        public string Family_Name { get; set; }
        public int Total_Sales {  get; set; }
    }
}
