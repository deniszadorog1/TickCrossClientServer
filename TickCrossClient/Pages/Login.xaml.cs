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

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private Frame _frame;
        public Login(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
        }

        private void RegistrationBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Registration(_frame);
        }

        private async void LoginBut_Click(object sender, RoutedEventArgs e)
        {
            TickCrossLib.Models.User? loggedUser = await ApiService.GetLoggedUser(LoginBox.Text, PasswordBox.Password);
            if (loggedUser is null || loggedUser.Id == -1)
            {
                MessageBox.Show("Something went wrong!");
                return;
            }
            ((MainWindow)Window.GetWindow(_frame)).SetLoggedUser(loggedUser);

            _frame.Content = new MainPage(_frame, (TickCrossLib.Models.User)loggedUser);
            ((MainWindow)Window.GetWindow(_frame)).SetGameRequestTimer();

        }
    }
}
