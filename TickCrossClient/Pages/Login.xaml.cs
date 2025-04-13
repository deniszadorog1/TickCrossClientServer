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

        private void LoginBut_Click(object sender, RoutedEventArgs e)
        {

            Services.ApiService.SetEnv();


            _frame.Content = new MainPage(_frame);
        }
    }
}
