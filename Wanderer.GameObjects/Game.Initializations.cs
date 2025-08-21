using System.Collections.Generic;
using System.Windows.Controls;
using Wanderer.GameCharacters;

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

        private T Spawn<T>(T character) where T : Character
        {
            SetCoord(character);
            _canvas.Children.Add(character.Picture);
            DrawCharacter(character);
            return character;
        }

        private void CreateEnemies()
        {
            CreateMonsters();
            CreateBoss();
        }

        private void CreateHero()
        {
            this.Hero = Spawn(_factory.CreateHero());
            CharacterStatModel.Hero = Hero;
            Hero.SecondStep += MoveEnemies;
            Hero.GotTheKey += HeroHasTheKey;
        }

        private void CreateMonsters()
        {
            var monsters = _factory.CreateMonsters(GameLevel, 2, 5); // [2..5] monsters, one has the key
            foreach (var m in monsters)
            {
                m.EnemyDied += EnemyDied;
                Spawn(m);
                Enemies.Add(m);
            }
        }

        private void CreateBoss()
        {
            Boss b = _factory.CreateBoss(GameLevel);
            b.EnemyDied += EnemyDied;
            Spawn(b);
            Enemies.Add(b);
        }
    }
}
