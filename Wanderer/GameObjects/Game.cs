using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Wanderer.GameObjects
{
    public partial class Game
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
        }

        private void LevelUp()
        {
            GameLevel++;
            CharacterStatModel.OnPropertyChanged("Game");
            Hero.GoNextField(random.Next(0, 10));
            ClearArea();
            CreateEnemies();
            _canvas.Children.Add(Hero.Picture);
            DrawCharacter(Hero);
            Hero.HasTheKey = false;
        }

        private void DrawCharacter(Character character)
        {
            Canvas.SetLeft(character.Picture, character.PositionX * 72);
            Canvas.SetTop(character.Picture, character.PositionY * 72);
        }

        public void MoveCharacter(Character character, Direction direction)
        {
            if (character.GetType().Equals(typeof(Hero)))
            {
                (character as Hero).SetDirection(direction);
            }
            switch (direction)
            {
                case Direction.Up:
                    if (character.PositionY > 0 && NewPositionIsFloor(character.PositionX, character.PositionY - 1))
                    {
                        LeaveCell(character);
                        character.PositionY--;
                        EnterCell(character);
                    }
                    break;
                case Direction.Down:
                    if (character.PositionY < 9 && NewPositionIsFloor(character.PositionX, character.PositionY + 1))
                    {
                        LeaveCell(character);
                        character.PositionY++;
                        EnterCell(character);
                    }
                    break;
                case Direction.Left:
                    if (character.PositionX > 0 && NewPositionIsFloor(character.PositionX - 1, character.PositionY))
                    {
                        LeaveCell(character);
                        character.PositionX--;
                        EnterCell(character);
                    }
                    break;
                case Direction.Right:
                    if (character.PositionX < 9 && NewPositionIsFloor(character.PositionX + 1, character.PositionY))
                    {
                        LeaveCell(character);
                        character.PositionX++;
                        EnterCell(character);
                    }
                    break;
            }
        }

        public void StartBattle(Character attacker, Character defender)
        {
            int dice;
            int strikeValue;
            do
            {
                dice = random.Next(1, 7);
                strikeValue = dice * 2 + attacker.StrikePoints;
                defender.TakeAStrike(strikeValue);
                var tmp = attacker;
                attacker = defender;
                defender = tmp;
            } while (attacker.CurrentHealthPoints > 0 && defender.CurrentHealthPoints > 0);
        }

        private void ClearArea()
        {
            foreach (var item in Enemies)
            {
                _canvas.Children.Remove(item.Picture);
                Area[item.PositionX, item.PositionY].EnemyOnIt = null;
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
            if (character.GetType().Equals(typeof(Hero)))
            {
                Area[x, y].HeroOnIt = true;
            }
            else
            {
                Area[x, y].EnemyOnIt = character as Enemy;
            }
            character.PositionX = x;
            character.PositionY = y;
        }

        private bool NewPositionIsFloor(int x, int y)
        {
            return Area[x, y].Type == TileType.Floor;
        }

        private void LeaveCell(Character character)
        {
            if(character.GetType().Equals(typeof(Hero)))
            {
                if (CharacterStatModel.Enemy == null)
                {
                    Area[character.PositionX, character.PositionY].HeroOnIt = false;
                }
                else
                {
                    CharacterStatModel.Enemy = null;
                }
            }
            else
            {
                Area[character.PositionX, character.PositionY].EnemyOnIt = null;
                CharacterStatModel.Enemy = null;

            }
        }

        private void EnterCell(Character character)
        {
            DrawCharacter(character);
            if (character.GetType().Equals(typeof(Hero)))
            {
                Hero.Steps++;
                CharacterStatModel.Enemy = Area[character.PositionX, character.PositionY].EnemyOnIt;
            }
            else
            {
                Area[character.PositionX, character.PositionY].EnemyOnIt = character as Enemy;
            }
        }
    }
}
