using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
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
using TickCrossLib.Models;

namespace TickCrossClient.Pages
{
    /// <summary>
    /// Логика взаимодействия для SetPlayers.xaml
    /// </summary>
    public partial class SetPlayers : Page
    {
        private Frame _frame;
        private User _user;
        public SetPlayers(Frame frame, User loggedUser)
        {
            _frame = frame;
            _user = loggedUser;

            InitializeComponent();

            FillEnemiesLoginsList();

        }

        public async void FillEnemiesLoginsList()
        {
            List<User> enemies = new List<User>();
            enemies.AddRange(await ApiService.GetEnemies(_user.Login));

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
                FontSize = 16,
            };
        }

        private async void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            string enemyLogin = GetChosenEnemyLogin();
            if (enemyLogin == string.Empty) return;

            User enemy = null;// = await ApiService.GetEnemyPlayer(enemyLogin);

            ((MainWindow)Window.GetWindow(_frame)).ClearSecondaryFrame();


            List<char>? signs = await ApiService.GetSigns();
            if (signs is null || signs.Count == 0) throw new NullReferenceException("Somthing wrong with signs");

            char enemySign = GetEnemySign();
            char userSign = enemySign == signs.First() ? signs.Last() : signs.First();

            int toStep = userSign == signs.First() ? 0 : 1;


            Game game = new Game(_user, enemy, toStep, userSign, enemySign);

            _frame.Content = new GamePage(game);

        }

        public char GetEnemySign()
        {
            return (char)EnemySignBox.SelectedValue;
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


    }
}
