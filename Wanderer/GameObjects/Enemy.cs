using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanderer.GameObjects
{
    public abstract class Enemy : Character
    {
        protected int[] _monsterLevels = { 0, 0, 2, 1, 0, 0, 1, 0, 1, 1 };
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

    }
}
