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

        private Size _basicBorderSize = 
            new Size(JsonService.GetNumByName("UserOptionPageBasicBorderWidth"),
                JsonService.GetNumByName("UserOptionPageBasicBorderHeight"));
        
        private Size _bigBorderSize = 
            new Size(JsonService.GetNumByName("UserOptionPageBigBorderWidth"),
                JsonService.GetNumByName("UserOptionPageBigBorderHeight"));
        
        private Size _firstStep = 
            new Size(JsonService.GetNumByName("UserOptionFirstStepWidth"),
                JsonService.GetNumByName("UserOptionFirstStepHeight"));

        public void ChangeCardSize(Size size)
        {
            if (_firstStep.Width > size.Width && _firstStep.Height > size.Height)
            {
                SetSizeParams(false);
                return;
            }
            SetSizeParams(true);
        }

        public void SetSizeParams(bool isBig)
        {
            SetBorderSize(isBig);
            SetBlockFontSize(isBig);
            SetBorderNameBlockFontSize(isBig);
        }

        private int _basicBorderBlockNameFz = JsonService.GetNumByName("UserOptionBorderBlockNameBasicFZ");
        private int _bigBorderBlockNameFz = JsonService.GetNumByName("UserOptionBorderBlockNameBigFZ");
        public void SetBorderNameBlockFontSize(bool isBig)
        {
            int size = isBig ? _bigBorderBlockNameFz : _basicBorderBlockNameFz;

            OptionsBlock.FontSize = size;
        }

        private int _basicBlocksFontSize = JsonService.GetNumByName("UserOptionBlockBasicFZ");
        private int _bigBlocksFontSize = JsonService.GetNumByName("UserOptionBlockBigFZ");
        public void SetBlockFontSize(bool isBig)
        {
            int fontSize = isBig ? _bigBlocksFontSize : _basicBlocksFontSize;

            LoginBlock.FontSize = fontSize;
            LoginTextBox.FontSize = fontSize;
            PasswordBlock.FontSize = fontSize;
            PasswordTextBox.FontSize = fontSize;


            double marginMinus = PasswordTextBox.Margin.Right + PasswordTextBox.Margin.Left;

            PasswordTextBox.Width = OptionBorder.Width - marginMinus;
            LoginTextBox.Width = OptionBorder.Width - marginMinus;
        }

        public void SetBorderSize(bool isBig)
        {
            Size size = isBig ? _bigBorderSize : _basicBorderSize;

            OptionBorder.Width = size.Width;
            OptionBorder.Height = size.Height;
        }
    }
}
