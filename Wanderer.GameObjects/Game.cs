using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Wanderer.GameCharacters;
using Wanderer.GameObjects.Rendering;

namespace Wanderer.GameObjects
{
    public partial class Game
    {
        public Tile[,] Area { get; set; }
        private readonly Canvas _canvas;
        private readonly byte[,] _firstmap = new byte[,]
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
        private readonly ICharacterFactory _factory;
        private readonly IGameRenderer _renderer; // NEW

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
            _factory = new CharacterFactory(random);
            _renderer = new WpfGameRenderer(_canvas); // NEW
            Enemies = new List<Enemy>();
            CharacterStatModel = new ViewModel { Game = this };

            InitArea();
            _renderer.RenderArea(Area); // NEW: was DrawArea()
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
            _renderer.Spawn(Hero);             // NEW: was _canvas.Children.Add(Hero.Picture)
            _renderer.UpdatePosition(Hero);    // NEW: was DrawCharacter(Hero)
            Hero.HasTheKey = false;
        }

        // Replaces DrawCharacter: delegate to renderer
        private void DrawCharacter(Character character)
        {
            _renderer.UpdatePosition(character);
        }

        public void MoveCharacter(Character character, Direction direction)
        {
            Tile nextCell = null;
            switch(direction)
            {
                case Direction.Up:
                    if(character.PositionY > 0) nextCell = Area[character.PositionX, character.PositionY - 1];
                    break;
                case Direction.Down:
                    if (character.PositionY < 9) nextCell = Area[character.PositionX, character.PositionY + 1];
                    break;
                case Direction.Left:
                    if (character.PositionX > 0) nextCell = Area[character.PositionX - 1, character.PositionY];
                    break;
                case Direction.Right:
                    if (character.PositionX < 9) nextCell = Area[character.PositionX + 1, character.PositionY];
                    break;
            }

            if (character.GetType().Equals(typeof(Hero)))
            {
                if (nextCell != null && nextCell.Type == TileType.Floor)
                {
                    _renderer.SetFacing((Hero)character, direction); // NEW: renderer handles visuals
                    StepCharacter(character, direction);
                }
            }
            else
            {
                if (nextCell != null && nextCell.Type == TileType.Floor && nextCell.EnemyOnIt == null)
                {
                    StepCharacter(character, direction);
                }
                else
                {
                    ChangeEnemyDirection((Enemy)character);
                }
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
                if (defender.CurrentHealthPoints > 0)
                {
                    var tmp = attacker;
                    attacker = defender;
                    defender = tmp;
                }
            } while (attacker.CurrentHealthPoints > 0 && defender.CurrentHealthPoints > 0);
        }

        private void ClearArea()
        {
            foreach (var item in Enemies)
            {
                _renderer.Remove(item); // NEW
                Area[item.PositionX, item.PositionY].EnemyOnIt = null;
            }
            Enemies.Clear();
            _renderer.Remove(Hero); // NEW
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

        private void LeaveCell(Character character)
        {
            if(character.GetType().Equals(typeof(Hero)))
            {
                if (CharacterStatModel.Enemy != null)
                {
                    CharacterStatModel.Enemy = null;
                }
                Area[character.PositionX, character.PositionY].HeroOnIt = false;
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

        private void ChangeEnemyDirection(Enemy enemy)
        {
            int newDirection = (((int)enemy.Direction) + 1) % 4;
            enemy.Direction = (Direction)newDirection;
        }

        private void StepCharacter(Character character, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    LeaveCell(character);
                    character.PositionY--;
                    EnterCell(character);
                    break;
                case Direction.Down:
                    LeaveCell(character);
                    character.PositionY++;
                    EnterCell(character);
                    break;
                case Direction.Left:
                    LeaveCell(character);
                    character.PositionX--;
                    EnterCell(character);
                    break;
                case Direction.Right:
                    LeaveCell(character);
                    character.PositionX++;
                    EnterCell(character);
                    break;
            }
        }
    }
}
