using System.ComponentModel;
using System.Windows.Controls;

namespace Wanderer.GameCharacters
{
    public abstract class Character : INotifyPropertyChanged
    {
        private int _level;
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if(_level != value)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }
        public abstract int CurrentHealthPoints { get; set; }
        public int DefendPoints { get; set; }
        public int StrikePoints { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Image Picture { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void TakeAStrike(int strikeValue)
        {
            if (strikeValue > DefendPoints)
            {
                CurrentHealthPoints -= (strikeValue - DefendPoints);
            }
        }
    }
}
