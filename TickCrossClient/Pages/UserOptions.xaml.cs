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
    /// Логика взаимодействия для UserOptions.xaml
    /// </summary>
    public partial class UserOptions : Page
    {
        private Frame _frame;
        private TickCrossLib.Models.User _user;
        public UserOptions(Frame frame, TickCrossLib.Models.User user)
        {
            _frame = frame;
            _user = user;

            InitializeComponent();
        }

        private async void ApplyBut_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
               string.IsNullOrWhiteSpace(PasswordTextBox.Text)) return;


            await ApiService.ChangeUserParams(_user, LoginTextBox.Text, PasswordTextBox.Text);
            _user.Login = LoginTextBox.Text;
            _user.Password = PasswordTextBox.Text;

            if (((MainWindow)Window.GetWindow(_frame)).MainFrame.Content is MainPage main) main.SetUserParams();
             ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();
        }
        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();
        }
    }
}
