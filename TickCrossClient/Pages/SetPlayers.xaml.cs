using System.Windows;
using System.Windows.Controls;
using TickCrossClient.Services;
using TickCrossLib.Services;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для SetPlayers.xaml
    /// </summary>
    public partial class SetPlayers : Page
    {
        private Frame _frame;
        private TickCrossLib.Models.User _user;
        public SetPlayers(Frame frame, TickCrossLib.Models.User loggedUser)
        {
            _frame = frame;
            _user = loggedUser;

            InitializeComponent();
            SetEnemiesPages();
        }

        public void SetEnemiesPages()
        {
            EnemiesList.Items.Clear();
            FillEnemiesLoginsList();
        }

        public async void FillEnemiesLoginsList()
        {
            List<TickCrossLib.Models.User> enemies =
                await ApiService.GetUsersToSendGameReq(_user.Id);

            if (enemies is null) return;

            //Get enemys friends + that doesnt gave game req

            //enemies.AddRange(await ApiService.GetEnemies(_user.Login));

            foreach (var enemy in enemies)
            {
                TextBlock block = GetLoginBlock(enemy.Login);
                EnemiesList.Items.Add(block);
            }
        }

        public TextBlock GetLoginBlock(string login)
        {
            return new TextBlock()
            {
                Text = login,
                FontSize = JsonService.GetNumByName("SetPlayersPageTextBlockFontSize"),
            };
        }

        private async void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            string enemyLogin = GetChosenEnemyLogin();
            if (enemyLogin == string.Empty || !await IsEnemyCanBeChosen(enemyLogin))
            {
                SetEnemiesPages();
                return;
            }
            TickCrossLib.Models.User? enemy = await ApiService.GetEnemyPlayer(enemyLogin);
            //((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();

            List<char>? signs = await ApiService.GetSigns();
            if (signs is null || signs.Count == 0) throw new NullReferenceException("Something wrong with signs");

            char? enemySign = GetEnemySign();
            if (enemySign is null) return;

            await ApiService.AddNewGameRequest(_user.Login, enemyLogin, (char)enemySign,
                signs.First() == enemySign ? signs.Last() : signs.First(), TickCrossLib.Enums.RequestStatus.InProgress);
            SetEnemiesPages();
        }

        public async Task<bool> IsEnemyCanBeChosen(string enemyLogin)
        {
            List<TickCrossLib.Models.User> enemies =
                await ApiService.GetUsersToSendGameReq(_user.Id);
            return enemies.Where(x => x.Login == enemyLogin).Any();
        }

        public char? GetEnemySign()
        {
            if (EnemySignBox.SelectedItem is null) return null;
            return ((ComboBoxItem)EnemySignBox.SelectedItem).Content.ToString().First();
        }

        public string GetChosenEnemyLogin()
        {
            var item = EnemiesList.SelectedItem;

            return item is null ? string.Empty : ((TextBlock)item).Text;
        }

        private void BackBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();
        }

        private void UpdatePageBut_Click(object sender, RoutedEventArgs e)
        {
            SetEnemiesPages();
        }
    }
}
