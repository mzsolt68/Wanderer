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

        public Game()
        {
            Area = new Tile[10, 10];
            GameLevel = 1;
            random = new Random();
        }

        public void InitArea()
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
                        Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        hero.PositionY--;
                        DrawCharacter(hero);
                        Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                    }
                    break;
                case Direction.Down:
                    hero.SetDirection(direction);
                    if (hero.PositionY < 9 && Area[hero.PositionX, hero.PositionY + 1].Type == TileType.Floor)
                    {
                        Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        hero.PositionY++;
                        DrawCharacter(hero);
                        Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                    }
                    break;
                case Direction.Left:
                    hero.SetDirection(direction);
                    if (hero.PositionX > 0 && Area[hero.PositionX - 1, hero.PositionY].Type == TileType.Floor)
                    {
                        Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        hero.PositionX--;
                        DrawCharacter(hero);
                        Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                    }
                    break;
                case Direction.Right:
                    hero.SetDirection(direction);
                    if (hero.PositionX < 9 && Area[hero.PositionX + 1, hero.PositionY].Type == TileType.Floor)
                    {
                        Area[hero.PositionX, hero.PositionY].IsOccupied = false;
                        hero.PositionX++;
                        DrawCharacter(hero);
                        Area[hero.PositionX, hero.PositionY].IsOccupied = true;
                    }
                    break;
            }
        }

        public Hero CreateHero()
        {
            Hero h = new Hero(random.Next(1, 7));
            SetCoord(h);
            return h;
        }

        public Monster CreateMonster()
        {
            Monster m = new Monster(random.Next(1, 7), 1, false);
            SetCoord(m);
            return m;
        }

        public Boss CreateBoss()
        {
            Boss b = new Boss(random.Next(1, 7), 1);
            SetCoord(b);
            return b;
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
    }
}