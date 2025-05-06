using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TickCrossClient.Services;
using TickCrossLib.EntityModels;
using TickCrossLib.Models;
using TickCrossLib.Services;

namespace TickCrossClient.Pages.FriendPages
{
    /// <summary>
    /// Логика взаимодействия для FriendsPage.xaml
    /// </summary>
    public partial class FriendsPage : Page
    {
        private TickCrossLib.Models.User _user;
        private Frame _frame;
        public FriendsPage(TickCrossLib.Models.User user, Frame frame)
        {
            _user = user;
            _frame = frame;
            InitializeComponent();

            SetTimer();
        }

        private DispatcherTimer _timer;
        private void SetTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += async (sender, e) =>
            {
                FriendsToAddList.Items.Clear();
                FriendsToRemoveList.Items.Clear();
                AddBasicParams();
            };
            _timer.Start();
        }
        
        public void AddBasicParams()
        {
            SetListBoxes();
        }

        public void SetListBoxes()
        {
            SetFriendsToAdd();
            SetFriendsToRemove();
        }

        public async void SetFriendsToRemove()
        {
            FriendsToAddList.Items.Clear();
            List<string> toRemove = await ApiService.GetUserLoginsToAddInFriends(_user.Id);
            SetUsersInTextBlock(toRemove, FriendsToAddList);
        }

        public async void SetFriendsToAdd()
        {
            FriendsToRemoveList.Items.Clear();
            List<string> friends = await ApiService.GetFriendsLogins(_user.Id);
            SetUsersInTextBlock(friends, FriendsToRemoveList);
        }

        public void SetUsersInTextBlock(List<string> users, ListBox box)
        {
            foreach (var user in users)
            {
                box.Items.Add(GetTextBlockWithUserLogin(user));
            }
        }

        public TextBlock GetTextBlockWithUserLogin(string login)
        {
            return new TextBlock()
            {
                Text = login,
                FontSize = JsonService.GetNumByName("FriendPageBasicListFont"),
            };
        }

        private async void RemoveBut_Click(object sender, RoutedEventArgs e)
        {
            string toRemoveLogin = GetUserLoginFromListBox(FriendsToRemoveList);
            if (toRemoveLogin == string.Empty) return;

            await ApiService.RemoveFriend(_user, toRemoveLogin);

            SetListBoxes();
        }

        private async void AddBut_Click(object sender, RoutedEventArgs e)
        {
            if (FriendsToAddList.SelectedItem is null) return;
            //Check is Offer can be crated (already in offers or friends etc...)
            //Add In Friend offer

            await ApiService.AddRequest(_user.Id, ((TextBlock)FriendsToAddList.SelectedItem).Text);


            return;

            string? newFriendLoginName = GetUserLoginFromListBox(FriendsToAddList);
            if (newFriendLoginName == string.Empty) return;

            await ApiService.AddFriend(_user, newFriendLoginName);
           
            SetListBoxes();
        }

        public string GetUserLoginFromListBox(ListBox box)
        {
            TextBlock block = (TextBlock)box.SelectedItem;
            if (block is null) return string.Empty;

            return block.Text;
        }

        private Size _basicMainPanelSize =
            new Size(JsonService.GetNumByName("FriendPageBasicMainPageWidth"),
                JsonService.GetNumByName("FriendPageBasicMainPageHeight"));

        private Size _bigMainPanelSize =
            new Size(JsonService.GetNumByName("FriendPageBigMainPanelWidth"),
                JsonService.GetNumByName("FriendPageBigMainPanelHeight"));

        private Size _firstStep =
            new Size(JsonService.GetNumByName("FriendPageFirstStepWidth"),
                JsonService.GetNumByName("FriendPageFirstStepHeight"));

        public void OptionsParamsSize(Size size)
        {
            // isBig
            if (_firstStep.Width > size.Width || _firstStep.Height > size.Height)
            {
                SetBordersSize(false);

                return;
            }
            //IsLittle

            SetBordersSize(true);
        }

        public void SetBordersSize(bool isBig)
        {
            Size toSet = isBig ? _bigMainPanelSize : _basicMainPanelSize;

            SetBorderSize(LeftBorder, toSet);
            SetBorderSize(RightBorder, toSet);

            SetBorderNameFontSize(isBig ? _bigBorderBlockNameFontSize :
                _basicBorderBlockNameFontSize);

            SetLestBoxesHeight(isBig);

            SetButtonsParams(isBig);
        }

        public void SetButtonsParams(bool isBig)
        {
            int height = isBig ? JsonService.GetNumByName("FriendPageBigButHeight") :
                JsonService.GetNumByName("FriendPageBasicButHeight");

            int fontSize = isBig ? JsonService.GetNumByName("FriendPageBigButFontSize") :
                JsonService.GetNumByName("FriendPageBasicButFontSize");

            RemoveBut.Height = height;
            RemoveBut.FontSize = fontSize;

            BackBut.Height = height;
            BackBut.FontSize = fontSize;

            AddBut.Height = height;
            AddBut.FontSize = fontSize;
        }


        public void SetLestBoxesHeight(bool isBig)
        {
            int height = isBig ? JsonService.GetNumByName("UserPageBigListBoxSize") :
                JsonService.GetNumByName("UserPageBasicListBoxSize");

            FriendsToRemoveList.Height = height;
            FriendsToAddList.Height = height;

        }

        private int _basicBorderBlockNameFontSize =
            JsonService.GetNumByName("FriendPageBlockNameBasicFontSize");
        private int _bigBorderBlockNameFontSize =
             JsonService.GetNumByName("FriendPageBlockNameBigFontSize");

        public void SetBorderNameFontSize(int size)
        {
            RemoveFriendsBlock.FontSize = size;
            AddFriendsBlock.FontSize = size;
        }

        public void SetBorderSize(Border border, Size size)
        {
            border.Width = size.Width;
            border.Height = size.Height;
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new MainPage(_frame, _user);
            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize();
        }
    }
}
