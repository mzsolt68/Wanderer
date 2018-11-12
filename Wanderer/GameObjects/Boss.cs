using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public class Boss : Character
    {
        public override int CurrentHealthPoints { get; set; }
        public override int DefendPoints { get; set; }
        public override int StrikePoints { get; set; }

        public Boss(int dice, int level)
        {
            Level = level;
            MaxHealthPoints = 2 * level * dice + dice;
            CurrentHealthPoints = MaxHealthPoints;
            DefendPoints = level / 2 * dice + dice / 2;
            StrikePoints = level * dice + level;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            Picture.Source = new BitmapImage(new Uri("../Images/boss.png", UriKind.Relative));
        }
    }
}
