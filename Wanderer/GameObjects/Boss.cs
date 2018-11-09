﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wanderer
{
    public class Boss : Character
    {
        public Boss(int dice, int level)
        {
            Level = level;
            MaxHealthPoints = 2 * level * dice + dice;
            CurrentHealthPoints = MaxHealthPoints;
            DefendPoints = level / 2 * dice + dice / 2;
            StrikePoints = level * dice + level;
            Picture.Source = new BitmapImage(new Uri("../Images/boss.png", UriKind.Relative));
        }
    }
}