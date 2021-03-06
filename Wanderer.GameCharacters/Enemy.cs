﻿using System.ComponentModel;

namespace Wanderer.GameCharacters
{
    public abstract class Enemy : Character
    {
        protected int[] _monsterLevels = { 0, 0, 2, 1, 0, 0, 1, 0, 1, 1 };
        private int _currhealthpts;

        public event PropertyChangedEventHandler EnemyDied;

        public Direction Direction { get; set; } = Direction.Left;
        public override int CurrentHealthPoints
        {
            get { return _currhealthpts; }
            set
            {
                if (_currhealthpts != value)
                {
                    _currhealthpts = (value < 0 ? 0 : value);
                    OnPropertyChanged("CurrentHealthPoints");
                    if(_currhealthpts <= 0)
                    {
                        EnemyDied(this, new System.ComponentModel.PropertyChangedEventArgs("CurrentHealthPoints"));
                    }
                }
            }
        }

    }
}
