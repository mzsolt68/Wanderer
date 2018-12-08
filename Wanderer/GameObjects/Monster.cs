using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public class Monster : Enemy
    {
        public bool HasTheKey { get; set; }

        public Monster(int gameLevel, int enemyLevel, int dice)
        {
            Level = gameLevel + _monsterLevels[enemyLevel];
            CurrentHealthPoints = 2 * Level * dice;
            DefendPoints = Level * dice / 2;
            StrikePoints = Level * dice;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            Picture.Source = new BitmapImage(new Uri("../Images/skeleton.png", UriKind.Relative));
        }
    }
}
