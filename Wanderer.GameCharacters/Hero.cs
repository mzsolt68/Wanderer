using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameCharacters
{
    public enum Direction : int { Left, Up, Right, Down }

    public class Hero : Character
    {
        private int _currhealthpts;
        private int _maxhealthpts;
        private int _steps;
        private bool _hasthekey;
        private readonly int[] _heroLevelupHealthPoints = { 10, 33, 100, 10, 33, 10, 33, 10, 10, 33 };
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
                        SecondStep?.Invoke(this, new PropertyChangedEventArgs("Steps"));
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
                    _currhealthpts = (value < 0 ? 0 : value);
                    if (value > MaxHealthPoints)
                    {
                        MaxHealthPoints = value;
                    }
                    OnPropertyChanged("CurrentHealthPoints");
                    if(_currhealthpts == 0)
                    {
                        HeroDied?.Invoke(this, new PropertyChangedEventArgs("CurrentHealthPoints"));
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

        public bool HasTheKey
        {
            set
            {
                if(_hasthekey != value)
                {
                    _hasthekey = value;
                    GotTheKey?.Invoke(this, new PropertyChangedEventArgs("HasTheKey"));
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
