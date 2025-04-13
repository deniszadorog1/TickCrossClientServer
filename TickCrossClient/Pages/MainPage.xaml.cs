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
        public MainPage(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
            //_frame.Background = Brushes.Transparent;
        }

        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetPageForSecondFrame(new SetPlayers(_frame));
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
            ((MainWindow)Window.GetWindow(_frame)).SetPageForSecondFrame(new UserOptions(_frame));
        }

        private void FriendOptionsBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new FriendPages.FriendsPage();
        }
    }
}
