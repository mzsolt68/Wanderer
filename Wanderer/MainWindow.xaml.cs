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
using Wanderer.GameObjects;

namespace Wanderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Hero hero;
        Monster monster;
        Boss boss;
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            game.InitArea();
            game.DrawArea(canvas);
            hero = game.CreateHero();
            game.DrawCharacter(hero);
            monster = game.CreateMonster();
            game.DrawCharacter(monster);
            boss = game.CreateBoss();
            game.DrawCharacter(boss);
            ViewModel m = new ViewModel();
            m.Hero = hero;
            m.Enemy = boss;
            baseGrid.DataContext = m;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    game.MoveHero(hero, Direction.Up);
                    break;
                case Key.Down:
                    game.MoveHero(hero, Direction.Down);
                    break;
                case Key.Left:
                    game.MoveHero(hero, Direction.Left);
                    break;
                case Key.Right:
                    game.MoveHero(hero, Direction.Right);
                    break;
            }
        }

    }
}
