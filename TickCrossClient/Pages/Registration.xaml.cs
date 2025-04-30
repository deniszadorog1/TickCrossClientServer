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
using System.Text.RegularExpressions;
using TickCrossLib.Services;

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

            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                !RegexService.RegistrationPasswordValid(PassBox.Password) ||
                !RegexService.RegistrationPasswordValid(LoginBox.Text) ||
            string.IsNullOrWhiteSpace(PassBox.Password))
            {
                MessageBox.Show("something went wrong!");
                return;
            }

            bool isAdded = await ApiService.AddNewUser(LoginBox.Text, PassBox.Password);
            if (!isAdded) return;

            MessageBox.Show("Added new user");

            LoginBox.Text = string.Empty;
            PassBox.Password = string.Empty;

            _frame.Content = new Login(_frame);
        }

        private bool CheckLogin()
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9_]{3,20}$");
            return rg.Match(LoginBox.Text).Success;
        }

        private bool CheckPassword()
        {
            Regex rg = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            return rg.Match(PassBox.Password).Success;
        }


        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Login(_frame);
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }

        private Size _basicMainPanelSize = 
            new Size(JsonService.GetNumByName("RegPageBasicMainPanelWidth"),
                JsonService.GetNumByName("RegPageBasicMainPanelHeight"));

        private Size _bigMainPanelSize = 
            new Size(JsonService.GetNumByName("RegPageBigMainPanelWidth"),
                JsonService.GetNumByName("RegPageBigMainPanelHeight"));

        private Size _firstStep = 
            new Size(JsonService.GetNumByName("RegPageFirstStepWidth"),
                JsonService.GetNumByName("RegPageFirstStepHeight"));

        public void ChangeCardSize(Size size)
        {
            if (_firstStep.Width > size.Width && _firstStep.Height > size.Height)
            {
                MainBorder.Width = _basicMainPanelSize.Width;
                MainBorder.Height = _basicMainPanelSize.Height;
                SetTextBoxSize();
            }
            else
            {
                MainBorder.Width = _bigMainPanelSize.Width;
                MainBorder.Height = _bigMainPanelSize.Height;
                SetTextBoxSize();
            }
        }

        public void SetTextBoxSize()
        {
            double marginDistance = MainPanel.Margin.Left + MainPanel.Margin.Right;
            LoginBox.Width = MainBorder.Width - marginDistance;
            PassBox.Width = MainBorder.Width - marginDistance;
        }
    }
}
