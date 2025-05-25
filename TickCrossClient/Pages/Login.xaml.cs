using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;
using TickCrossLib.Enums;
using TickCrossLib.Services;

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
            if (string.IsNullOrWhiteSpace(LoginBox.Text) ||
                !TickCrossLib.Services.RegexService.LoginValidation(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("something went wrong!");
                return;
            }

            if (await ApiService.IsUserLoggedOnLoginPage(loggedUser.Id))
            {
                MessageBox.Show("Is logged");
                return;
            }
            loggedUser._token = JwtService.Generate(loggedUser);
            ApiService.SetToken(loggedUser._token);

            ApiService.SetUserLoginStatus(loggedUser.Id, UserStat.Online);

            ((MainWindow)Window.GetWindow(_frame)).SetLoggedUser(loggedUser);

            _frame.Content = new MainPage(_frame, (TickCrossLib.Models.User)loggedUser);
            ((MainWindow)Window.GetWindow(_frame)).SetGameRequestTimer();
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }

        private Size _basicMainPanelSize =
            new Size(JsonService.GetNumByName("LoginPageBasicMainPageWidth"),
                JsonService.GetNumByName("LoginPageBasicMainPageHeight"));

        private Size _bigMainPanelSize =
            new Size(JsonService.GetNumByName("LoginPageBigMainPanelWidth"),
                JsonService.GetNumByName("LoginPageBigMainPanelHeight"));

        private Size _firstStep =
            new Size(JsonService.GetNumByName("LoginPageFirstStepWidth"),
                JsonService.GetNumByName("LoginPageFirstStepHeight"));

        public void ChangeCardSize(Size size) //+-
        {
            SetCardParams(_firstStep.Width > size.Width && _firstStep.Height > size.Height);
/*            if (_firstStep.Width > size.Width && _firstStep.Height > size.Height)
            {
                SetMainBorderSize(_basicMainPanelSize);
                SetTextBoxSize();
                SetBoxNameSize(_baseTextBoxFontSize);
                KchauImage.Visibility = Visibility.Hidden;
            }
            else
            {
                SetMainBorderSize(_bigMainPanelSize);
                SetTextBoxSize();
                SetBoxNameSize(_bigTextBoxFontSize);
                KchauImage.Visibility = Visibility.Visible;
            }*/
        }

        public void SetCardParams(bool isBasic)
        {
            Size mainPanelSize = isBasic ? _basicMainPanelSize : _bigMainPanelSize;
            int textSize = isBasic ? _baseTextBoxFontSize : _bigTextBoxFontSize;
            Visibility vis = isBasic ? Visibility.Hidden : Visibility.Visible;

            SetMainBorderSize(mainPanelSize);
            SetTextBoxSize();
            SetBoxNameSize(textSize);
            KchauImage.Visibility = vis;
        }

        private int _baseTextBoxFontSize =
            JsonService.GetNumByName("LoginPageBasicFontSize");
        private int _bigTextBoxFontSize =
            JsonService.GetNumByName("LoginPageBigFontSize");
        public void SetBoxNameSize(int size)
        {
            UserNameBlock.FontSize = size;
            PasswordBlock.FontSize = size;

            LoginBox.FontSize = size;
            PasswordBox.FontSize = size;
        }

        public void SetMainBorderSize(Size size)
        {
            MainBorder.Width = size.Width;
            MainBorder.Height = size.Height;
        }

        public void SetTextBoxSize()
        {
            double marginDistance = MainPanel.Margin.Left + MainPanel.Margin.Right;
            LoginBox.Width = MainBorder.Width - marginDistance;
            PasswordBox.Width = MainBorder.Width - marginDistance;
        }
    }
}
