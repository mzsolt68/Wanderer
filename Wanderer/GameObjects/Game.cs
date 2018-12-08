﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private int[] _monsterLevels = { 0, 0, 2, 1, 0, 0, 1, 0, 1, 1 };

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

        private void MoveEnemies(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Itt kell megvalósítani az ellenfelek mozgását
            foreach (var enemy in Enemies)
            {
                Direction enemyDirection = (Direction)random.Next(Enum.GetNames(typeof(Direction)).Length);
                MoveCharacter(enemy, enemyDirection);
            }
        }

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

        public void LevelUp()
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

        public void DrawCharacter(Character character)
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

        private void CreateEnemies()
        {
            CreateMonsters();
            CreateBoss();
        }

        private void EnemyDied(object sender, PropertyChangedEventArgs e)
        {
            var enemy = sender as Enemy;
            Hero.HasTheKey = enemy.GetType().Equals(typeof(Monster)) ? (enemy as Monster).HasTheKey : false;
            Enemies.Remove(enemy);
            _canvas.Children.Remove(enemy.Picture);
            CharacterStatModel.Enemy = null;
            Hero.LevelUp(random.Next(1, 7));
        }

        public void HeroHasTheKey(object sender, PropertyChangedEventArgs e)
        {
            LevelUp();
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
            switch(character.GetType().ToString())
            {
                case "Hero":
                    if (CharacterStatModel.Enemy == null)
                    {
                        Area[character.PositionX, character.PositionY].HeroOnIt = false;
                    }
                    else
                    {
                        CharacterStatModel.Enemy = null;
                    }
                    break;
                case "Boss":
                case "Monster":
                    Area[character.PositionX, character.PositionY].EnemyOnIt = null;
                    CharacterStatModel.Enemy = null;
                    break;
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

        private void EndGame(object sender, PropertyChangedEventArgs e)
        {
            //
        }
    }
}
