using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public abstract class Character : INotifyPropertyChanged
    {
        public int Level { get; set; }
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
                CurrentHealthPoints = CurrentHealthPoints - (strikeValue - DefendPoints);
            }
        }
    }
}
