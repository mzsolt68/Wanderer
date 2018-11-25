using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public abstract class Character
    {
        public int Level { get; set; }
        public abstract int CurrentHealthPoints { get; set; }
        public abstract int DefendPoints { get; set; }
        public abstract int StrikePoints { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Image Picture { get; set; }

        public abstract void OnPropertyChanged(string propertyName);
    }
}
