using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanderer.GameObjects;

namespace Wanderer
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Hero _hero;
        private Enemy _enemy;
        private Game _game;

        public event PropertyChangedEventHandler PropertyChanged;

        public Hero Hero
        {
            get { return _hero; }
            set
            {
                if(_hero != value)
                {
                    _hero = value;
                    OnPropertyChanged("Hero");
                }
            }
        }
        public Enemy Enemy
        {
            get { return _enemy; }
            set
            {
                if(_enemy != value)
                {
                    _enemy = value;
                    OnPropertyChanged("Enemy");
                }
            }
        }
        public Game Game
        {
            get { return _game; }
            set
            {
                if (_game != value)
                {
                    _game = value;
                    OnPropertyChanged("Game");
                }
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
