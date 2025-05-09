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
using TickCrossLib.EntityModels;
using TickCrossLib.Models;
using TickCrossLib.Models.NonePlayable;
using TickCrossLib.Services;

namespace TickCrossClient.Pages.GameReqs
{
    /// <summary>
    /// Логика взаимодействия для GameRequests.xaml
    /// </summary>
    public partial class GameRequests : Page
    {
        private TickCrossLib.Models.User _user;
        private Frame _frame;
        public GameRequests(Frame frame, TickCrossLib.Models.User user)
        {
            _frame = frame;
            _user = user;
            InitializeComponent();

            SetListBoxes();
        }

        public void SetListBoxes()
        {
            ToRemoveReqsListBox.Items.Clear();
            SetReqsListBox.Items.Clear();
            SetGotRequests();
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

            if (reqs is null) return;
            SetGameReqsInBox(SetReqsListBox, reqs);
        }

        public async void SetSentReqsByUser()
        {
            List<TickCrossLib.Models.NonePlayable.GameRequestModel> reqs =
                 await ApiService.GetRequestsSentByUser(_user.Id);

            if (reqs is null) return;

            SetGameReqsInBox(ToRemoveReqsListBox, reqs);
        }

        public void SetGameReqsInBox(ListBox box, List<TickCrossLib.Models.NonePlayable.GameRequestModel> reqs)
        {
            for (int i = 0; i < reqs.Count; i++)
            {
                AddEnemyInListBox(box, box == ToRemoveReqsListBox ?
                    reqs[i].ReceiverLogin : reqs[i].SenderLogin);
            }
        }

        public void AddEnemyInListBox(ListBox box, string enemyLogin)
        {
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock enemyLoginBox = new TextBlock()
            {
                Text = enemyLogin
            };

            TextBlock pointer = new TextBlock()
            {
                Text = " - is enemy"
            };

            panel.Children.Add(enemyLoginBox);
            panel.Children.Add(pointer);

            box.Items.Add(panel);
        }

        public string GetEnemyLogin(ListBox box)
        {
            StackPanel panel = (StackPanel)box.SelectedItem;

            if (panel is null) return null; 

            if (panel is null) return null;

            for (int i = 0; i < panel.Children.Count; i++)
            {
                if (panel.Children[i] is TextBlock block)
                {
                    return block.Text;
                }
            }
            return null;
        }

        private async void RemoveRequestBut_Click(object sender, RoutedEventArgs e)
        {
            string toRemoveLogin = GetEnemyLogin(ToRemoveReqsListBox);
            if (toRemoveLogin is null) return;

            await ApiService.RemoveGameRequest(_user.Id, toRemoveLogin);
            SetListBoxes();
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(new MainPage(_frame, _user));
        }

        private async void AcceptGameBut_Click(object sender, RoutedEventArgs e)
        {
            //Check is enemy is online or in game!!

            string enemyLogin = GetEnemyLogin(SetReqsListBox);
            if (enemyLogin is null) return;

            //is user online + is user in game
            bool isEnemyOnline = await ApiService.IsUserIsOnline(enemyLogin);
            if(!isEnemyOnline)
            {
                MessageBox.Show("This user is not online!");
                return;
            }
            SetListBoxes();
        }

        private async void RejectGameBut_Click(object sender, RoutedEventArgs e)
        {
            string toRemoveLogin = GetEnemyLogin(SetReqsListBox);
            if (toRemoveLogin is null) return;

            await ApiService.RejectGameRequest(_user.Id, toRemoveLogin);
            SetListBoxes();
        }

        private void UpdatePageBut_Click(object sender, RoutedEventArgs e)
        {
            SetListBoxes();
        }
    }
}
