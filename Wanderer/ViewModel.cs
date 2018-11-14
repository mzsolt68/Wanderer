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
        private Character _enemy;

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
        public Character Enemy
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
        public Game Game { get; set; }

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
