using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wanderer.GameObjects;

namespace Wanderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game(canvas);
            mainWindow.DataContext = game.CharacterStatModel;
            game.Hero.HeroDied += EndGame;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    game.MoveCharacter(game.Hero, Direction.Up);
                    break;
                case Key.Down:
                    game.MoveCharacter(game.Hero, Direction.Down);
                    break;
                case Key.Left:
                    game.MoveCharacter(game.Hero, Direction.Left);
                    break;
                case Key.Right:
                    game.MoveCharacter(game.Hero, Direction.Right);
                    break;
                case Key.Space:
                    if (game.Area[game.Hero.PositionX, game.Hero.PositionY].EnemyOnIt != null)
                    {
                        game.StartBattle(game.Hero, game.Area[game.Hero.PositionX, game.Hero.PositionY].EnemyOnIt);
                    }
                    break;
            }
        }

        private void EndGame(object sender, PropertyChangedEventArgs e)
        {
            this.Close();
        }

    }
}
