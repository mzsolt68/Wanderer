using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer
{
    public enum HeroDirection : int { Left, Right, Up, Down}

    public class Hero : Character
    {
        public Hero(int dice)
        {
            MaxHealthPoints = 20 + 3 * dice;
            CurrentHealthPoints = MaxHealthPoints;
            DefendPoints = 2 * dice;
            StrikePoints = 5 * dice;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            SetDirection(HeroDirection.Down);
        }

        public void SetDirection(HeroDirection direction)
        {
            switch(direction)
            {
                case HeroDirection.Left:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-left.png", UriKind.Relative));
                    break;
                case HeroDirection.Right:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-right.png", UriKind.Relative));
                    break;
                case HeroDirection.Up:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-up.png", UriKind.Relative));
                    break;
                case HeroDirection.Down:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-down.png", UriKind.Relative));
                    break;

            }
        }
    }
}
