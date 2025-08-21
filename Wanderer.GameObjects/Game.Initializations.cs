using System.Collections.Generic;
using System.Windows.Controls;
using Wanderer.GameCharacters;
using Wanderer.GameObjects.Rendering;

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
            // No-op: area is rendered by the renderer in Game constructor now.
            _renderer.RenderArea(Area);
        }

        private T Spawn<T>(T character) where T : Character
        {
            SetCoord(character);
            _renderer.Spawn(character);
            _renderer.UpdatePosition(character);
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
            var monsters = _factory.CreateMonsters(GameLevel, 2, 5);
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
