using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public class Boss : Enemy
    {
        public Boss(int gameLevel, int enemyLevel, int dice)
        {
            Level = gameLevel + _monsterLevels[enemyLevel];
            CurrentHealthPoints = 2 * Level * dice + dice;
            DefendPoints = (int)(Level / 2.0 * dice + dice / 2.0);
            StrikePoints = Level * dice + Level;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            Picture.Source = new BitmapImage(new Uri("../Images/boss.png", UriKind.Relative));
        }
    }
}
