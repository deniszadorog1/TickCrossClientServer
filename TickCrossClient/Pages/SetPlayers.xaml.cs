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
    /// Логика взаимодействия для SetPlayers.xaml
    /// </summary>
    public partial class SetPlayers : Page
    {
        private Frame _frame;
        public SetPlayers(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
        }

        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();

            _frame.Content = new GamePage();

        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();
        }
    }
}
