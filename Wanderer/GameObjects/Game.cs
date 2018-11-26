using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public class Game
    {
        public Tile[,] Area { get; set; }
        private Canvas _canvas;
        private byte[,] _firstmap = new byte[,] 
        {
            {1, 1, 1, 0, 1, 0, 1, 1, 1, 1 },
            {1, 1, 1, 0, 1, 0, 1, 0, 0, 1 },
            {1, 0, 0, 0, 1, 0, 1, 0, 0, 1 },
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
            {1, 0, 1, 0, 1, 1, 1, 1, 0, 1 },
            {1, 0, 1, 0, 1, 0, 0, 1, 0, 1 },
            {1, 1, 1, 1, 1, 0, 0, 1, 0, 1 },
            {1, 0, 0, 0, 1, 1, 1, 1, 0, 1 },
            {1, 1, 1, 0, 1, 0, 0, 1, 0, 0 }
        };
        private static Random random;
        public int GameLevel { get; private set; }
        public Hero Hero;
        public List<Enemy> Enemies;
        public ViewModel CharacterStatModel;
        private int[] _monsterLevels = {0, 0, 2, 1, 0, 0, 1, 0, 1, 1 };
        private int[] _heroLevelupHealthPoints = {10, 33, 100, 10, 33, 10, 33, 10, 10, 33 };

        public Game(Canvas canvas)
        {
            _canvas = canvas;
            Area = new Tile[10, 10];
            GameLevel = 1;
            random = new Random();
            Enemies = new List<Enemy>();
            CharacterStatModel = new ViewModel();
            CharacterStatModel.Game = this;
            InitArea();
            DrawArea();
            CreateEnemies();
            CreateHero();
            Hero.SecondStep += Hero_SecondStep;
        }

        private void Hero_SecondStep(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Itt kell megvalósítani az ellenfelek mozgását
        }

        private void InitArea()
        {
            for(int x = 0; x < Area.GetLength(0); x++)
                for(int y = 0; y < Area.GetLength(1); y++)
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

        public void LevelUp()
        {
            GameLevel++;
            CharacterStatModel.OnPropertyChanged("Game");
            int dice = random.Next(0, 10);
            Hero.CurrentHealthPoints += Hero.MaxHealthPoints * _heroLevelupHealthPoints[dice] / 100;
            ClearArea();
            CreateEnemies();
            _canvas.Children.Add(Hero.Picture);
            DrawCharacter(Hero);
            Hero.HasTheKey = false;
        }

        public void DrawCharacter(Character character)
        {
            Canvas.SetLeft(character.Picture, character.PositionX * 72);
            Canvas.SetTop(character.Picture, character.PositionY * 72);
        }

        public void MoveHero(Hero hero, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    hero.SetDirection(direction);
                    if (hero.PositionY > 0 && Area[hero.PositionX, hero.PositionY - 1].Type == TileType.Floor)
                    {
                        LeaveCell(hero);
                        hero.PositionY--;
                        EnterCell(hero);
                    }
                    break;
                case Direction.Down:
                    hero.SetDirection(direction);
                    if (hero.PositionY < 9 && Area[hero.PositionX, hero.PositionY + 1].Type == TileType.Floor)
                    {
                        LeaveCell(hero);
                        hero.PositionY++;
                        EnterCell(hero);
                    }
                    break;
                case Direction.Left:
                    hero.SetDirection(direction);
                    if (hero.PositionX > 0 && Area[hero.PositionX - 1, hero.PositionY].Type == TileType.Floor)
                    {
                        LeaveCell(hero);
                        hero.PositionX--;
                        EnterCell(hero);                    }
                    break;
                case Direction.Right:
                    hero.SetDirection(direction);
                    if (hero.PositionX < 9 && Area[hero.PositionX + 1, hero.PositionY].Type == TileType.Floor)
                    {
                        LeaveCell(hero);
                        hero.PositionX++;
                        EnterCell(hero);
                    }
                    break;
            }
        }

        private void CreateEnemies()
        {
            CreateMonsters();
            CreateBoss();
        }

        public void StartBattle(Character attacker)
        {
            if (Area[attacker.PositionX, attacker.PositionY].IsOccupied)
            {
                int dice = random.Next(1, 7);
                int strikeValue = dice * 2 + attacker.StrikePoints;
                Enemy enemy = CharacterStatModel.Enemy;
                if(strikeValue > enemy.DefendPoints)
                {
                    enemy.CurrentHealthPoints = enemy.CurrentHealthPoints - (strikeValue - enemy.DefendPoints);
                    CharacterStatModel.OnPropertyChanged("Enemy");
                    if(enemy.CurrentHealthPoints <= 0)
                    {
                        Hero.HasTheKey = enemy.GetType().Equals(typeof(Monster)) ? (enemy as Monster).HasTheKey : false;
                        Enemies.Remove(enemy);
                        _canvas.Children.Remove(enemy.Picture);
                        CharacterStatModel.Enemy = null;
                        Hero.LevelUp(dice);
                    }
                }
                if(Hero.HasTheKey)
                {
                    LevelUp();
                }
            }
        }

        private void CreateHero()
        {
            this.Hero = new Hero(random.Next(1, 7));
            SetCoord(Hero);
            _canvas.Children.Add(Hero.Picture);
            DrawCharacter(Hero);
            CharacterStatModel.Hero = Hero;
        }

        private void CreateMonsters()
        {
            int nrOfMonsters = random.Next(2, 6);
            int dice;
            do
            {
                int level = GameLevel + _monsterLevels[random.Next(0, 10)];
                Monster m = new Monster(level, random.Next(1, 7));
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
            int level = GameLevel + _monsterLevels[random.Next(0, 10)];
            Boss b = new Boss(level, random.Next(1, 7));
            SetCoord(b);
            _canvas.Children.Add(b.Picture);
            DrawCharacter(b);
            Enemies.Add(b);
        }

        private void ClearArea()
        {
            foreach (var item in Enemies)
            {
                _canvas.Children.Remove(item.Picture);
                Area[item.PositionX, item.PositionY].IsOccupied = false;
            }
            Enemies.Clear();
            _canvas.Children.Remove(Hero.Picture);
        }

        private void SetCoord(Character character)
        {
            int x, y;
            do
            {
                do
                {
                    x = random.Next(0, 10);
                    y = random.Next(0, 10);
                } while (Area[x, y].Type != TileType.Floor);
            } while (Area[x, y].IsOccupied);
            Area[x, y].IsOccupied = true;
            character.PositionX = x;
            character.PositionY = y;
        }

        private Enemy GetEnemyOnPosition(int x, int y)
        {
            foreach (Enemy enemy in Enemies)
            {
                if(enemy.PositionX == x && enemy.PositionY == y)
                {
                    return enemy;
                }
            }
            return null;
        }

        private void LeaveCell(Character character)
        {
            if (character.GetType().Equals(typeof(Hero)))
            {
                if (CharacterStatModel.Enemy == null)
                {
                    Area[character.PositionX, character.PositionY].IsOccupied = false;
                }
                else
                {
                    CharacterStatModel.Enemy = null;
                }
            }
        }

        private void EnterCell(Character character)
        {
            DrawCharacter(character);
            if (character.GetType().Equals(typeof(Hero)))
            {
                Hero.Steps++;
                if (Area[character.PositionX, character.PositionY].IsOccupied)
                {
                    CharacterStatModel.Enemy = GetEnemyOnPosition(character.PositionX, character.PositionY);
                }
                else
                {
                    Area[character.PositionX, character.PositionY].IsOccupied = true;
                }
            }
        }
    }
}
