using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer
{
    public class Monster : Character
    {
        public bool HasTheKey { get; private set; }

        public Monster(int dice, int level, bool hasthekey)
        {
            Level = level;
            MaxHealthPoints = 2 * level * dice;
            CurrentHealthPoints = MaxHealthPoints;
            DefendPoints = level / 2 * dice;
            StrikePoints = level * dice;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            Picture.Source = new BitmapImage(new Uri("../Images/skeleton.png", UriKind.Relative));
        }
    }
}
