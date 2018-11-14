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
        Boss boss;
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();
            game.DrawArea(canvas);
            game.CreateCharacters();
            mainWindow.DataContext = game.CharacterStatModel;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    game.MoveHero(game.Hero, Direction.Up);
                    break;
                case Key.Down:
                    game.MoveHero(game.Hero, Direction.Down);
                    break;
                case Key.Left:
                    game.MoveHero(game.Hero, Direction.Left);
                    break;
                case Key.Right:
                    game.MoveHero(game.Hero, Direction.Right);
                    break;
            }
        }

    }
}
