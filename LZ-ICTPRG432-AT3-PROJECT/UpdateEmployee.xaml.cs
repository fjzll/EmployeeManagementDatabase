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
    /// Interaction logic for UpdateEmployee.xaml
    /// </summary>
    public partial class UpdateEmployee : Window
    {
        Employee employee;
        public UpdateEmployee(string searchdata)
        {
            InitializeComponent();
            employee = new Employee();
            employee.SearchEmployee(searchdata);
            this.DataContext = employee;
        }

        private void SaveSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            employee.UpdateSalary();
            MessageBox.Show("Salary updated");
        }

        private void ClearSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            GrossSalaryTextBox.Clear();
        }

        private void CloseSalaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
