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
using TickCrossClient.Services;

using System.Data.SqlClient;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private Frame _frame;
        public Registration(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
        }

        private async void RegisterBut_Click(object sender, RoutedEventArgs e)
        {
            bool isExist = await ApiService.IsUserLoginExist(LoginBox.Text);
            if (isExist)
            {
                MessageBox.Show("User with such login is exist!");
                return;
            }

            bool isAdded =  await ApiService.AddNewUser(LoginBox.Text, PassBox.Password);
            if (!isAdded) return;

            MessageBox.Show("Added new user");

            LoginBox.Text = string.Empty;
            PassBox.Password = string.Empty;

            _frame.Content = new Login(_frame);
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Login(_frame);
        }
    }
}
