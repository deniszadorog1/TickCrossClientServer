using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;
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
