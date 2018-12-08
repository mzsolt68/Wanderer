using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public class Monster : Enemy
    {
        private int _currhealthpts;

        public override int CurrentHealthPoints
        {
            get { return _currhealthpts; }
            set
            {
                if (_currhealthpts != value)
                {
                    _currhealthpts = value;
                    OnPropertyChanged("CurrentHealthPoints");
                }
            }
        }

        public bool HasTheKey { get; set; }

        public Monster(int level, int dice)
        {
            Level = level;
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
