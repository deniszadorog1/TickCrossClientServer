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
            //_frame.Background = Brushes.Transparent;

            SetUserParams();
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
/*
           Frame secFrame = ((MainWindow)Window.GetWindow(_frame)).SecondaryFrame;

            _frame.Effect = new BlurEffect()
            {
                Radius = 30
            };
            _frame.IsEnabled = false;

            Canvas.SetZIndex(secFrame, 10);
            Canvas.SetZIndex(_frame, -1);

            secFrame.Background = Brushes.Transparent;

            secFrame.Content = new SetPlayers(_frame);*/
        }

        private void ExitBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Login(_frame);
        }

        private void OptionsBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetPageForSecondFrame(new UserOptions(_frame, _user));
        }

        private void FriendOptionsBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new FriendPages.FriendsPage(_user);
        }
    }
}
