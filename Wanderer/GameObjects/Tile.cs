using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Wanderer.GameObjects
{
    public enum TileType : int { Floor, Wall}

    public class Tile
    {
        public TileType Type { get; private set; }
        public Image Picture { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool HeroOnIt { get; set; }
        public Enemy EnemyOnIt { get; set; }
        public bool IsOccupied {
            get
            {
                return HeroOnIt || (EnemyOnIt != null);
            }
            //private set;
        }

        public Tile(TileType type)
        {
            Type = type;
            Width = 72;
            Height = 72;
            Picture = new Image();

            if (this.Type == TileType.Floor)
            {
                Picture.Source = new BitmapImage(new Uri("../Images/floor.png", UriKind.Relative));
            }
            else
            {
                Picture.Source = new BitmapImage(new Uri("../Images/wall.png", UriKind.Relative));
            }
        }
    }
}
