using Microsoft.AspNetCore.Mvc.ModelBinding;
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
using System.Windows.Media.Animation;
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

            if (!await IsGameReqExists(toRemoveLogin, _user.Login))
            {
                SetListBoxes();
                return;
            }

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
            bool isUserOnline = await ApiService.IsUserIsOnline(_user.Login);
            if (!isEnemyOnline )
            {
                MessageBox.Show("Enemy does not have online status!");
                return;
            }
            else if (!isUserOnline)
            {
                MessageBox.Show("User does not have online status!");
                return;
            }


            TickCrossLib.Models.User enemy = await ApiService.GetUserByLogin(enemyLogin);


            ApiService.SetUserLoginStatus(_user.Id, TickCrossLib.Enums.UserStat.InGame);
            ApiService.SetUserLoginStatus(enemy.Id, TickCrossLib.Enums.UserStat.InGame);
    
            SetListBoxes();

            //Start game
            StartGame(enemyLogin);
        }

        private async void StartGame(string enemyLogin)
        {
            if (!await IsGameReqExists(_user.Login, enemyLogin))
            {
                SetListBoxes();
                return;
            }
            //Get game request
            TickCrossLib.Models.GameRequest req =
                await ApiService.GetGameRequest(enemyLogin, _user.Login);
            if (req is null) return;

            //Clear secondary frame
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();


            //Get signs to play
            List<char>? signs = await ApiService.GetSigns();

            //Create new game
            TickCrossLib.Models.Game game =
                new TickCrossLib.Models.Game(req, signs);

            //change status for request
            await ApiService.SetRequestStatus(req, TickCrossLib.Enums.RequestStatus.Accepted);

            //add game value in DB
            await ApiService.AddGameInDB(game);

            //Set game id to game value
            game.AddGameId(await ApiService.GetLastGameId());

            //Create game page
            GamePage page = new GamePage(game, _frame, _user);

            //Set game page to Main Frame
            ((MainWindow)Window.GetWindow(_frame)).SetContentToMainFrame(page);
            ((MainWindow)Window.GetWindow(_frame))._timer.Stop();

            //Start game timer(to make moves
            page.SetGameMoveTimer(req);

            //Add temp game in DB
            ApiService.AddTempGameInDB(game.Id);
        }

        private async void RejectGameBut_Click(object sender, RoutedEventArgs e)
        {
            string toRemoveLogin = GetEnemyLogin(SetReqsListBox);
            if (toRemoveLogin is null) return;

            if (!await IsGameReqExists(_user.Login, toRemoveLogin))
            {
                SetListBoxes();
                return;
            }

            await ApiService.RejectGameRequest(_user.Id, toRemoveLogin);
            SetListBoxes();
        }

        private void UpdatePageBut_Click(object sender, RoutedEventArgs e)
        {
            SetListBoxes();
        }

        private async Task<bool> IsGameReqExists(string recieverLogin, string senderRequest)
        {
            TickCrossLib.Models.GameRequest req = await ApiService.GetGameRequest(senderRequest, recieverLogin);
            return req is null ? false : true;
        }

    }
}
