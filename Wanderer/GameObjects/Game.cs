﻿using System;
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

        public Game()
        {
            Area = new Tile[10, 10];
            GameLevel = 1;
            random = new Random();
            Enemies = new List<Enemy>();
            CharacterStatModel = new ViewModel();
            CharacterStatModel.Game = this;
            InitArea();
        }

        private void InitArea()
        {
            for(int x = 0; x < Area.GetLength(0); x++)
                for(int y = 0; y < Area.GetLength(1); y++)
                {
                    Area[x, y] = new Tile(_firstmap[x, y] == 0 ? TileType.Wall : TileType.Floor);
                }
        }

        public void DrawArea(Canvas canvas)
        {
            _canvas = canvas;
            for (int x = 0; x < Area.GetLength(0); x++)
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

        public void LevelUp()
        {
            GameLevel++;
        }

        public void DrawCharacter(Character character)
        {
            _canvas.Children.Remove(character.Picture);
            _canvas.Children.Add(character.Picture);
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
                        if (CharacterStatModel.Enemy == null)
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        }
                        else
                        {
                            CharacterStatModel.Enemy = null;
                        }
                        hero.PositionY--;
                        DrawCharacter(hero);
                        if (Area[hero.PositionX, hero.PositionY].IsOccupied)
                        {
                            CharacterStatModel.Enemy = GetEnemyOnPosition(hero.PositionX, hero.PositionY);
                        }
                        else
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                        }
                    }
                    break;
                case Direction.Down:
                    hero.SetDirection(direction);
                    if (hero.PositionY < 9 && Area[hero.PositionX, hero.PositionY + 1].Type == TileType.Floor)
                    {
                        if (CharacterStatModel.Enemy == null)
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        }
                        else
                        {
                            CharacterStatModel.Enemy = null;
                        }
                        hero.PositionY++;
                        DrawCharacter(hero);
                        if (Area[hero.PositionX, hero.PositionY].IsOccupied)
                        {
                            CharacterStatModel.Enemy = GetEnemyOnPosition(hero.PositionX, hero.PositionY);
                        }
                        else
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                        }
                    }
                    break;
                case Direction.Left:
                    hero.SetDirection(direction);
                    if (hero.PositionX > 0 && Area[hero.PositionX - 1, hero.PositionY].Type == TileType.Floor)
                    {
                        if (CharacterStatModel.Enemy == null)
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        }
                        else
                        {
                            CharacterStatModel.Enemy = null;
                        }
                        hero.PositionX--;
                        DrawCharacter(hero);
                        if (Area[hero.PositionX, hero.PositionY].IsOccupied)
                        {
                            CharacterStatModel.Enemy = GetEnemyOnPosition(hero.PositionX, hero.PositionY);
                        }
                        else
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                        }
                    }
                    break;
                case Direction.Right:
                    hero.SetDirection(direction);
                    if (hero.PositionX < 9 && Area[hero.PositionX + 1, hero.PositionY].Type == TileType.Floor)
                    {
                        if (CharacterStatModel.Enemy == null)
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        }
                        else
                        {
                            CharacterStatModel.Enemy = null;
                        }
                        hero.PositionX++;
                        DrawCharacter(hero);
                        if (Area[hero.PositionX, hero.PositionY].IsOccupied)
                        {
                            CharacterStatModel.Enemy = GetEnemyOnPosition(hero.PositionX, hero.PositionY);
                        }
                        else
                        {
                            Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                        }
                    }
                    break;
            }
        }

        public void CreateCharacters()
        {
            CreateHero();
            CreateMonsters();
            CreateBoss();
        }

        private void CreateHero()
        {
            this.Hero = new Hero();
            int dice = random.Next(1, 7);
            Hero.MaxHealthPoints = 20 + 3 * dice;
            Hero. CurrentHealthPoints = Hero.MaxHealthPoints;
            Hero.DefendPoints = 2 * dice;
            Hero.StrikePoints = 5 * dice;
            SetCoord(Hero);
            DrawCharacter(Hero);
            CharacterStatModel.Hero = Hero;
        }

        private void CreateMonsters()
        {
            int nrOfMonsters = random.Next(2, 6);
            int dice;
            do
            {
                Monster m = new Monster();
                dice = random.Next(1, 7);
                m.Level = GameLevel + _monsterLevels[random.Next(0, 10)];
                m.MaxHealthPoints = 2 * m.Level * dice;
                m.CurrentHealthPoints = m.MaxHealthPoints;
                m.DefendPoints = m.Level * dice / 2;
                m.StrikePoints = m.Level * dice;
                SetCoord(m);
                DrawCharacter(m);
                Enemies.Add(m);
                nrOfMonsters--;
            } while (nrOfMonsters > 0);
            dice = random.Next(0, Enemies.Count);
            (Enemies[dice] as Monster).HasTheKey = true;
        }

        private void CreateBoss()
        {
            Boss b = new Boss();
            int dice = random.Next(1, 7);
            b.Level = GameLevel + _monsterLevels[random.Next(0, 10)];
            b.MaxHealthPoints = 2 * b.Level * dice + dice;
            b.CurrentHealthPoints = b.MaxHealthPoints;
            b.DefendPoints = b.Level / 2 * dice + dice / 2;
            b.StrikePoints = b.Level * dice + b.Level;
            SetCoord(b);
            DrawCharacter(b);
            Enemies.Add(b);
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

        private Character GetEnemyOnPosition(int x, int y)
        {
            foreach (Character enemy in Enemies)
            {
                if(enemy.PositionX == x && enemy.PositionY == y)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
}
