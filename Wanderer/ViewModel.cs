using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanderer.GameObjects;

namespace Wanderer
{
    class ViewModel
    {
        public Hero Hero { get; set; }
        public Character Enemy { get; set; }
        public Game Game { get; set; }
    }
}
