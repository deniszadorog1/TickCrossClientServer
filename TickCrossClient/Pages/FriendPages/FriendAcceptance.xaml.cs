using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
using TickCrossLib.Models.NonePlayable;

namespace TickCrossClient.Pages.FriendPages
{
    /// <summary>
    /// Логика взаимодействия для FriendAcceptance.xaml
    /// </summary>
    public partial class FriendAcceptance : Page
    {
        private Frame _frame;
        private TickCrossLib.Models.User _user;
        public FriendAcceptance(Frame frame, TickCrossLib.Models.User user)
        {
            _frame = frame;
            _user = user;

            InitializeComponent();

            FillListBoxes();
        }

        private DispatcherTimer _timer;
        private void FillListBoxes()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += async (sender, e) =>
            {
                SentOffersList.Items.Clear();
                RecivedOffersList.Items.Clear();
                SetListBoxes();
                //Get sent friend requests
                //Get recived friend Requests
            };
            _timer.Start();
        }

        private void SetListBoxes()
        {
            SetSentOffersList();
            SetReceivedOffersList();
        }

        private async void SetSentOffersList()
        {
            List<FriendRequestModel> reqs = await ApiService.GetFriendReqsSentByUser(_user.Id);

            for(int i = 0; i < reqs.Count; i ++)
            {
                AddItemToListBox(reqs[i], SentOffersList);
            }      
        }

        private async void SetReceivedOffersList()
        {
            List<FriendRequestModel> reqs = await ApiService.GetFriendReqsSentToUser(_user.Id);

            for(int i = 0; i < reqs.Count; i++)
            {
                AddItemToListBox(reqs[i], RecivedOffersList);
            }         
        }

        private void AddItemToListBox(FriendRequestModel model, ListBox box)
        {
            const int textSize = 16;
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 0, 0, 0)
            };

            TextBlock enemy = new TextBlock()
            {
                Text = _user.Login == model.SenderLogin ? model.ReceiverLogin : model.SenderLogin,
                FontSize = textSize
            };

            TextBlock isEnemy = new TextBlock()
            {
                Margin = new Thickness(15, 0, 0, 0),
                Text = " - is enemy",
                FontSize = textSize
            };

            panel.Children.Add(enemy);
            panel.Children.Add(isEnemy);

            box.Items.Add(panel);
        }

        private string GetEnemyLogin(ListBox box)
        {
            StackPanel panel = (StackPanel)box.SelectedItem;

            for(int i = 0; i < panel.Children.Count; i++)
            {
                if (panel.Children[i] is TextBlock block)
                {
                    return block.Text;
                }
            }
            return null;
        }

        private async void RemoveBut_Click(object sender, RoutedEventArgs e)
        {
            if (SentOffersList.SelectedItem is null) return;
            string senderLogin = GetEnemyLogin(SentOffersList);
            await ApiService.DeleteFriendReqByReceiverLogin(_user.Id, senderLogin);
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).
                SetContentToMainFrame(new MainPage(_frame, _user));
        }

        private async void DeclineBut_Click(object sender, RoutedEventArgs e)
        {
            if (RecivedOffersList.SelectedItem is null) return;
            string receiverLogin = GetEnemyLogin(RecivedOffersList);
            await ApiService.DeleteFriendReqBySenderLogin(_user.Id, receiverLogin);
        }

        private async void AcceptBut_Click(object sender, RoutedEventArgs e)
        {
            if (RecivedOffersList.SelectedItem is null) return;
            string enemyLogin = GetEnemyLogin(RecivedOffersList);


            await ApiService.AddFriend(_user, enemyLogin);

            await ApiService.DeleteFriendReqBySenderLogin(_user.Id, enemyLogin);
            //remove req by senderLogin + receiver id

        }
    }
}
