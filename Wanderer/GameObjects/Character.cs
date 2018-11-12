using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public class Character
    {
        public int Level { get; set; }
        public int CurrentHealthPoints { get; set; }
        public int MaxHealthPoints { get; set; }
        public int DefendPoints { get; set; }
        public int StrikePoints { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Image Picture { get; set; }
    }
}
