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
using TickCrossClient.Services;
using TickCrossLib.Services;

namespace TickCrossClient.Pages.FriendPages
{
    /// <summary>
    /// Логика взаимодействия для FriendsPage.xaml
    /// </summary>
    public partial class FriendsPage : Page
    {
        private TickCrossLib.Models.User _user;
        public FriendsPage(TickCrossLib.Models.User user)
        {
            _user = user;
            InitializeComponent();

            AddBasicParams();
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
            List<TickCrossLib.Models.User> toRemove = await ApiService.GetUsersThatNotFriend(_user.Login);
            SetUsersInTextBlock(toRemove, FriendsToAddList);
        }

        public async void SetFriendsToAdd()
        {
            FriendsToRemoveList.Items.Clear();
            List<TickCrossLib.Models.User> friends = await ApiService.GetUserFriends(_user.Login);
            SetUsersInTextBlock(friends, FriendsToRemoveList);
        }

        public void SetUsersInTextBlock(List<TickCrossLib.Models.User> users, ListBox box)
        {
            foreach (var user in users)
            {
                box.Items.Add(GetTextBlockWithUserLogin(user.Login));
            }
        }

        public TextBlock GetTextBlockWithUserLogin(string login)
        {
            return new TextBlock()
            {
                Text = login,
                FontSize = 16,
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

    }
}
