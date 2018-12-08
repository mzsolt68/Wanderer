using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public partial class Game
    {
        private void InitArea()
        {
            for (int x = 0; x < Area.GetLength(0); x++)
                for (int y = 0; y < Area.GetLength(1); y++)
                {
                    Area[x, y] = new Tile(_firstmap[x, y] == 0 ? TileType.Wall : TileType.Floor);
                }
        }

        private void DrawArea()
        {
            for (int x = 0; x < Area.GetLength(0); x++)
            {
                for (int y = 0; y < Area.GetLength(1); y++)
                {
                    Image tile = Area[x, y].Picture;
                    tile.Height = Area[x, y].Height;
                    tile.Width = Area[x, y].Width;
                    _canvas.Children.Add(tile);
                    Canvas.SetLeft(tile, x * 72);
                    Canvas.SetTop(tile, y * 72);
                }
            }
        }

        private void CreateEnemies()
        {
            CreateMonsters();
            CreateBoss();
        }

        private void CreateHero()
        {
            this.Hero = new Hero(random.Next(1, 7));
            SetCoord(Hero);
            _canvas.Children.Add(Hero.Picture);
            DrawCharacter(Hero);
            CharacterStatModel.Hero = Hero;
            Hero.SecondStep += MoveEnemies;
            Hero.GotTheKey += HeroHasTheKey;
        }

        private void CreateMonsters()
        {
            int nrOfMonsters = random.Next(2, 6);
            int dice;
            do
            {
                Monster m = new Monster(GameLevel, random.Next(0, 10), random.Next(1, 7));
                m.EnemyDied += EnemyDied;
                SetCoord(m);
                _canvas.Children.Add(m.Picture);
                DrawCharacter(m);
                Enemies.Add(m);
                nrOfMonsters--;
            } while (nrOfMonsters > 0);
            dice = random.Next(0, Enemies.Count);
            (Enemies[dice] as Monster).HasTheKey = true;
        }

        private void CreateBoss()
        {
            Boss b = new Boss(GameLevel, random.Next(0, 10), random.Next(1, 7));
            b.EnemyDied += EnemyDied;
            SetCoord(b);
            _canvas.Children.Add(b.Picture);
            DrawCharacter(b);
            Enemies.Add(b);
        }
    }
}
