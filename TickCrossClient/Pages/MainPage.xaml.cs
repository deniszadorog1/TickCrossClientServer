using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TickCrossClient.Services;
using TickCrossLib.Enums;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame _frame;
        private TickCrossLib.Models.User _user;
        public MainPage(Frame frame, TickCrossLib.Models.User user)
        {
            _frame = frame;
            _user = user;

            InitializeComponent();

            SetUserParams();
            SetUserGameParams();
        }

        public void SetUserParams()
        {
            LoginText.Text = _user.Login;
            WonsText.Text = _user.Wins.ToString();

            LosesText.Text = _user.Loses.ToString();
            TotalGamesText.Text = _user.TotalGames.ToString();
        }

        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetPageForSecondFrame(new SetPlayers(_frame, _user));
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }

        private void ExitBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Login(_frame);
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
            ApiService.SetUserLoginStatus(_user.Id, UserStat.Offline);
        }

        private void OptionsBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetPageForSecondFrame(new UserOptions(_frame, _user));
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }

        private void FriendOptionsBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new FriendPages.FriendsPage(_user, _frame);
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }

        public async void SetUserGameParams()
        {
            //Set get count params
            WonsText.Text = (await ApiService.GetUserWinsAmount(_user.Id)).ToString();
            LosesText.Text = (await ApiService.GetUserLosesAmount(_user.Id)).ToString();
            TotalGamesText.Text = (await ApiService.GetUserGamesAmount(_user.Id)).ToString();
        }

        private Size _basicMainPanelSize = new Size(190, 340);
        private Size _bigMainPanelSize = new Size(400, 550);
        private Size _firstStep = new Size(1200, 700);

        public void ChangeCardSize(Size size)
        {
            if (_firstStep.Width > size.Width && _firstStep.Height > size.Height)
            {
                SetParams(false);
                return;
            }
            SetParams(true);
        }

        public void SetParams(bool isBig)
        {
            Size panelSize = isBig ? _bigMainPanelSize : _basicMainPanelSize;

            SetCardSize(MenuBorder, panelSize);
            SetCardSize(UserInfoBorder, panelSize);
            SetPanelPramsSize(panelSize);

            SetUserParamsSizes(isBig);
            SetButtonsParams(isBig);
        }

        public void SetButtonsParams(bool isBig)
        {
            int height = isBig ? JsonService.GetNumByName("FriendPageBigButHeight") :
                JsonService.GetNumByName("FriendPageBasicButHeight");

            int fontSize = isBig ? JsonService.GetNumByName("MainPageBigButsFontSize") :
                JsonService.GetNumByName("FriendPageBasicButFontSize");

            SetFontSizeAndHeightForButton(height, fontSize, StartGameBut);
            SetFontSizeAndHeightForButton(height, fontSize, FriendOptionsBut);
            SetFontSizeAndHeightForButton(height, fontSize, OptionsBut);
            SetFontSizeAndHeightForButton(height, fontSize, ExitBut);
        }

        public void SetFontSizeAndHeightForButton(int height, int fontSize, Button but)
        {
            but.Height = height;
            but.FontSize = fontSize;
        }

        private int _littleBorderNameSize = JsonService.GetNumByName("MainPageLittleBorderNameSize");
        private int _bigBorderNameSize = JsonService.GetNumByName("MainPageBigBorderNameSize");
        public void SetUserParamsSizes(bool isBig)
        {
            SetUserInfoBlockSize(LoginName, LoginText, isBig);
            SetUserInfoBlockSize(WonName, WonsText, isBig);
            SetUserInfoBlockSize(LoseName, LosesText, isBig);
            SetUserInfoBlockSize(TotalName, TotalGamesText, isBig);

            SetSizeForBorderNames(MenuName, isBig);
            SetSizeForBorderNames(LoggedUserBlock, isBig);
        }

        public void SetSizeForBorderNames(TextBlock block, bool isBig)
        {
            if (isBig)
            {
                block.FontSize = _bigBorderNameSize;
                return;
            }
            block.FontSize = _littleBorderNameSize;
        }

        private int _littleBlockFontSize = JsonService.GetNumByName("MainPageLittleBlockFotSize");
        private int _bigBlockFontSize = JsonService.GetNumByName("MainPageBigBlockFotSize");

        public void SetUserInfoBlockSize(TextBlock fieldNameBlock, TextBlock activeBlock, bool isBig)
        {
            if (isBig)
            {
                fieldNameBlock.FontSize = _bigBlockFontSize;
                activeBlock.FontSize = _bigBlockFontSize;
                return;
            }
            fieldNameBlock.FontSize = _littleBlockFontSize;
            activeBlock.FontSize = _littleBlockFontSize;
        }

        public void SetCardSize(Border border, Size size)
        {
            border.Width = size.Width;
            border.Height = size.Height;

            SetPanelPramsSize(size);
        }

        public void SetPanelPramsSize(Size panelSize)
        {
            SetMenuButtsSize(panelSize);
        }

        public void SetMenuButtsSize(Size panelSize)
        {
            int butHeight = panelSize == _basicMainPanelSize ? _basicButHeight : _bigButHeight;

            for (int i = 0; i < MenuPanel.Children.Count; i++)
            {
                if (MenuPanel.Children[i] is Button but)
                {
                    SetSizeForButton(but, butHeight);
                }
            }
        }

        private int _basicButHeight = JsonService.GetNumByName("MainPageBasicButHeight");
        private int _bigButHeight = JsonService.GetNumByName("MainPageBigButHeight");
        public void SetSizeForButton(Button but, int height)
        {
            but.Height = height;
        }

        private void GameReqsBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new GameReqs.GameRequests(_frame, _user);
        }

        private void FriendRequest_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new FriendPages.FriendAcceptance(_frame, _user);
        }
    }
}
