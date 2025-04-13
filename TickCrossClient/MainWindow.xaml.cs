using System.Diagnostics;
using System.Text;
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

using TickCrossClient.Pages;
using TickCrossClient.Pages.FriendPages;

namespace TickCrossClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new Login(MainFrame);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!MainFrame.IsEnabled) ClearSecondaryFrame();
            
            if (MainFrame.Content is Registration || MainFrame.Content is MainPage || 
                MainFrame.Content is UserOptions )
            {
                MainFrame.Content = new Login(MainFrame);
                e.Cancel = true;
            }      
            else if(MainFrame.Content is GamePage || MainFrame.Content is FriendsPage)
            {
                MainFrame.Content = new MainPage(MainFrame);
                e.Cancel = true;
            }
            
        }

        public void ClearSecondaryFrame()
        {
            SecondaryFrame.Content = null;
            Canvas.SetZIndex(SecondaryFrame, -1);
            Canvas.SetZIndex(MainFrame, 100);

            MainFrame.Effect = null;
            MainFrame.IsEnabled = true;
        }

        private void MainFrame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MainFrame.IsEnabled) return;

            MainFrame.Effect = null;
            MainFrame.IsEnabled = true;
            SecondaryFrame.Content = null;
        }

        private void SecondaryFrame_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var clicked = e.OriginalSource as FrameworkElement;
            if(clicked is Border bor && bor.Background == Brushes.Transparent)
            {
                ClearSecondaryFrame();
            }
        }

        public void SetPageForSecondFrame(Page page)
        {
            MainFrame.Effect = new BlurEffect()
            {
                Radius = 30
            };
            MainFrame.IsEnabled = false;

            Canvas.SetZIndex(SecondaryFrame, 10);
            Canvas.SetZIndex(MainFrame, -1);

            SecondaryFrame.Background = Brushes.Transparent;

            SecondaryFrame.Content = page;
        }
    }
}