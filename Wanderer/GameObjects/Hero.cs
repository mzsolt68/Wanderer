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
    public enum Direction : int { Left, Right, Up, Down }

    public class Hero : Character
    {
        private int _currhealthpts;
        private int _maxhealthpts;
        private int _steps;
        private bool _hasthekey;
        private int[] _heroLevelupHealthPoints = { 10, 33, 100, 10, 33, 10, 33, 10, 10, 33 };
        public event PropertyChangedEventHandler SecondStep;
        public event PropertyChangedEventHandler HeroDied;
        public event PropertyChangedEventHandler GotTheKey;
        public int Steps
        {
            get { return _steps; }
            set
            {
                if(_steps != value)
                {
                    _steps = value;
                    if (_steps > 0 && _steps % 2 == 0)
                    {
                        SecondStep(this, new PropertyChangedEventArgs("Steps"));
                    }
                }
            }
        }
        public override int CurrentHealthPoints
        {
            get { return _currhealthpts; }
            set
            {
                if (_currhealthpts != value)
                {
                    _currhealthpts = value;
                    if (value > MaxHealthPoints)
                    {
                        MaxHealthPoints = value;
                    }
                    OnPropertyChanged("CurrentHealthPoints");
                    if(_currhealthpts <= 0)
                    {
                        HeroDied(this, new PropertyChangedEventArgs("CurrentHealthPoints"));
                    }
                }
            }
        }
        public int MaxHealthPoints
        {
            get { return _maxhealthpts; }
            set
            {
                if (_maxhealthpts != value)
                {
                    _maxhealthpts = value;
                    OnPropertyChanged("MaxHealthPoints");
                }
            }
        }

        public bool HasTheKey {
            //get;
            set
            {
                if(_hasthekey != value)
                {
                    _hasthekey = value;
                    GotTheKey(this, new PropertyChangedEventArgs("HasTheKey"));
                }
            }
        }

        public Hero(int dice)
        {
            Level = 1;
            MaxHealthPoints = 20 + 3 * dice;
            CurrentHealthPoints = MaxHealthPoints;
            DefendPoints = 2 * dice;
            StrikePoints = 5 * dice;
            HasTheKey = false;
            Picture = new Image
            {
                Width = 72,
                Height = 72
            };
            SetDirection(Direction.Down);
        }

        public void SetDirection(Direction direction)
        {
            switch (direction)
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
            Level++;
            MaxHealthPoints += dice;
            DefendPoints += dice;
            StrikePoints += dice;
        }

        public void GoNextField(int dice)
        {
            CurrentHealthPoints += MaxHealthPoints * _heroLevelupHealthPoints[dice] / 100;
        }
    }
}
