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
    /// Логика взаимодействия для Registartion.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private Frame _frame;
        public Registration(Frame frame)
        {
            _frame = frame;
            InitializeComponent();
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new Login(_frame);
        }

        private void RegisterBut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
