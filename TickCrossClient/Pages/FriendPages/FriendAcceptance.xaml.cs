using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;
using TickCrossLib.Models.NonePlayable;
using TickCrossLib.Services;

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

            SetRequests();
        }

        private void SetRequests()
        {
            SentOffersList.Items.Clear();
            RecivedOffersList.Items.Clear();
            SetListBoxes();
        }

        private void SetListBoxes()
        {
            SetSentOffersList();
            SetReceivedOffersList();
        }

        private async void SetSentOffersList()
        {
            List<FriendRequestModel> reqs = await ApiService.GetFriendReqsSentByUser(_user.Id);

            for (int i = 0; i < reqs.Count; i++)
            {
                AddItemToListBox(reqs[i], SentOffersList);
            }
        }

        private async void SetReceivedOffersList()
        {
            List<FriendRequestModel> reqs = await ApiService.GetFriendReqsSentToUser(_user.Id);

            for (int i = 0; i < reqs.Count; i++)
            {
                AddItemToListBox(reqs[i], RecivedOffersList);
            }
        }

        private void AddItemToListBox(FriendRequestModel model, ListBox box)
        {
            const int textSize = 16;
            const int rightMarginPanel = 10;
            const int rightMarginEnemyPanel = 15;

            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(rightMarginPanel, 0, 0, 0)
            };

            TextBlock enemy = new TextBlock()
            {
                Text = _user.Login == model.SenderLogin ? model.ReceiverLogin : model.SenderLogin,
                FontSize = textSize
            };

            TextBlock isEnemy = new TextBlock()
            {
                Margin = new Thickness(rightMarginEnemyPanel, 0, 0, 0),
                Text = " - pot friend",
                FontSize = textSize
            };

            panel.Children.Add(enemy);
            panel.Children.Add(isEnemy);

            box.Items.Add(panel);
        }

        private string GetEnemyLogin(ListBox box)
        {
            StackPanel panel = (StackPanel)box.SelectedItem;

            for (int i = 0; i < panel.Children.Count; i++)
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
            SetRequests();
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
            SetRequests();
        }

        private async void AcceptBut_Click(object sender, RoutedEventArgs e)
        {
            if (RecivedOffersList.SelectedItem is null) return;
            string enemyLogin = GetEnemyLogin(RecivedOffersList);


            await ApiService.AddFriend(_user, enemyLogin);

            await ApiService.DeleteFriendReqBySenderLogin(_user.Id, enemyLogin);
            SetRequests();
            //remove req by senderLogin + receiver id
        }

        private void UpdatePageBut_Click(object sender, RoutedEventArgs e)
        {
            SetRequests();
        }

        private Size _bigMainPanelSize = new Size
            (JsonService.GetNumByName("FriendAcceptMainPanelWidth"),
            JsonService.GetNumByName("FriendAcceptMainPanelHeight"));

        private Size _firstStep = new Size(
            JsonService.GetNumByName("FriendAcceptFirstStepWidth"),
            JsonService.GetNumByName("FriendAcceptFirstStepHeight"));

        public void ChangeCardSize(Size size)
        {
            if (_firstStep.Width > size.Width && _firstStep.Height > size.Height)
            {
                SetParams(false);
                return;
            }
            SetParams(true);
        }

        private Size _basicBorderSize = new Size(
            JsonService.GetNumByName("FriendAcceptBasicBorderWidth"),
            JsonService.GetNumByName("FriendAcceptBasicBorderHeight"));


        private Size _bigBorderSize = new Size(
            JsonService.GetNumByName("FriendAcceptBigBorderWidth"),
            JsonService.GetNumByName("FriendAcceptBigBorderHeight"));

        private void SetParams(bool isBig)
        {
            //border
            SetBorderSize(isBig ? _bigBorderSize : _basicBorderSize);

            //Border name text block
            SetBorderNameParams(isBig);

            //ListBox
            SetListBoxesSize(isBig);
            //Buttons
            SetButtonHeight(isBig);
        }

        private int _basicButHeight = JsonService.GetNumByName("FriendAcceptBasicButHeight");
        private int _bigButHeight = JsonService.GetNumByName("FriendAcceptBigButHeight");

        private int _basicButFz = JsonService.GetNumByName("FriendAcceptBasicButFZ");
        private int _bigButFz = JsonService.GetNumByName("FriendAcceptBigButFZ");

        private void SetButtonHeight(bool isBig)
        {
            int height = isBig ? _bigButHeight : _basicButHeight;
            int fontSize = isBig ? _bigButFz : _basicButFz;

            SetButtonParams(height, fontSize, RemoveBut);
            SetButtonParams(height, fontSize, UpdatePageBut);
            SetButtonParams(height, fontSize, BackBut);
            SetButtonParams(height, fontSize, AcceptBut);
            SetButtonParams(height, fontSize, DeclineBut);
        }

        public void SetButtonParams(int height, int fontSize, Button but)
        {
            but.Height = height;
            but.FontSize = fontSize;
        }

        private int _basicListBoxHeight = JsonService.GetNumByName("FriendAcceptListBoxBasicHeight");
        private int _bigListBoxHeight = JsonService.GetNumByName("FriendAcceptListBoxBigHeight");

        private void SetListBoxesSize(bool isBig)
        {
            int height = isBig ? _bigListBoxHeight : _basicListBoxHeight;

            SentOffersList.Height = height;
            RecivedOffersList.Height = height;
        }

        private int _borderNameTextBlockBasicSize = JsonService.GetNumByName("FriendAcceptBorderNameBlockBasicFz");
        private int _borderNameTextBlockBigSize = JsonService.GetNumByName("FriendAcceptBorderNameBlockBigFz");

        private void SetBorderNameParams(bool isBig)
        {
            int fontSize = isBig ? _borderNameTextBlockBigSize : _borderNameTextBlockBasicSize;

            SentFriendOfferBlock.FontSize = fontSize;
            RecivedFriendOfferBlock.FontSize = fontSize;
        }

        public void SetBorderSize(Size size)
        {
            LeftBorder.Width = size.Width;
            LeftBorder.Height = size.Height;

            RightBorder.Width = size.Width;
            RightBorder.Height = size.Height;
        }
    }
}
