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
    public class Boss : Character
    {
        private int _currhealthpts;
        private int _defpts;
        private int _strpts;
        public event PropertyChangedEventHandler PropertyChanged;

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
        public override int DefendPoints
        {
            get { return _defpts; }
            set
            {
                if (_defpts != value)
                {
                    _defpts = value;
                    OnPropertyChanged("DefendPoints");
                }
            }
        }
        public override int StrikePoints
        {
            get { return _strpts; }
            set
            {
                if (_strpts != value)
                {
                    _strpts = value;
                    OnPropertyChanged("StrikePoints");
                }
            }
        }

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

        public override void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
