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
using System.Windows.Threading;
using TickCrossClient.Services;
using TickCrossLib.Models;

namespace TickCrossClient.Pages.GameReqs
{
    /// <summary>
    /// Логика взаимодействия для GameRequests.xaml
    /// </summary>
    public partial class GameRequests : Page
    {
        private User _user;
        public GameRequests(User user)
        {
            _user = user;
            InitializeComponent();
        }

        private DispatcherTimer _reqTimer;
        public void UpdateReqsByTimer()
        {

        }

        public void SetGotRequests()
        {
            SetSentReqsToUser();
            SetSentReqsByUser();
        }

        public async void SetSentReqsToUser()
        {
            List<TickCrossLib.Models.NonePlayable.GameRequestModel> reqs =
                await ApiService.GetGameRequestsSentToUser(_user.Id);
        }

        public async void SetSentReqsByUser()
        {
            List<TickCrossLib.Models.NonePlayable.GameRequestModel> reqs =
                 await ApiService.GetRequestsSentByUser(_user.Id);

        }

    }
}
