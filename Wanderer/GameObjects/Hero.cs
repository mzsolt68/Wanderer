using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public enum Direction : int { Left, Right, Up, Down}

    public class Hero : Character, INotifyPropertyChanged
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
        public bool HasTheKey { get; set; }

        public Hero()
        {
            Level = 1;
            HasTheKey = false;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            SetDirection(Direction.Down);
        }

        public override void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.Left:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-left.png", UriKind.Relative));
                    break;
                case Direction.Right:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-right.png", UriKind.Relative));
                    break;
                case Direction.Up:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-up.png", UriKind.Relative));
                    break;
                case Direction.Down:
                    Picture.Source = new BitmapImage(new Uri("../Images/hero-down.png", UriKind.Relative));
                    break;

            }
        }

        public void LevelUp(int dice)
        {
            MaxHealthPoints += dice;
            DefendPoints += dice;
            StrikePoints += dice;
        }
    }
}
