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

        private int RollDice()
        {
            return random.Next(1, 7);
        }

        private int RollEnemyLevel()
        {
            return random.Next(1, 10);
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
            this.Hero = Spawn(new Hero(RollDice()));
            CharacterStatModel.Hero = Hero;
            Hero.SecondStep += MoveEnemies;
            Hero.GotTheKey += HeroHasTheKey;
        }

        private void CreateMonsters()
        {
            int nrOfMonsters = random.Next(2, 6);
            List<Monster> createdMonsters = new List<Monster>(nrOfMonsters);
            for (int i = 0; i < nrOfMonsters; i++)
            {
                Monster m = Spawn(new Monster(GameLevel, RollEnemyLevel(), RollDice()));
                m.EnemyDied += EnemyDied;
                Enemies.Add(m);
                createdMonsters.Add(m);
            }
            createdMonsters[random.Next(0, createdMonsters.Count)].HasTheKey = true;
        }

        private void CreateBoss()
        {
            Boss b = Spawn(new Boss(GameLevel, RollEnemyLevel(), RollDice()));
            b.EnemyDied += EnemyDied;
            Enemies.Add(b);
        }
    }
}
