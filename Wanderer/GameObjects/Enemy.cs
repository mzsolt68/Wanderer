using System.ComponentModel;

namespace Wanderer.GameObjects
{
    public abstract class Enemy : Character
    {
        protected int[] _monsterLevels = { 0, 0, 2, 1, 0, 0, 1, 0, 1, 1 };
        private int _currhealthpts;

        public event PropertyChangedEventHandler EnemyDied;

        public override int CurrentHealthPoints
        {
            get { return _currhealthpts; }
            set
            {
                if (_currhealthpts != value)
                {
                    _currhealthpts = value;
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
