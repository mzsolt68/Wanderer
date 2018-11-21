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

        public void CreateCharacters()
        {
            CreateMonsters();
            CreateBoss();
            CreateHero();
        }

        public void StartBattle(Character attacker)
        {
            //Ellenőrizni, hogy van-e másik karakter a mezőn
            //Strike point számolás
            //Ha a strike point nagyobb, mint a másik karakter defend pontja, akkor
            //a másik karakter HP-ja csökken SP-DP értékkel.
            //Ha a védekező HP-je nulla lesz, akkor meghal és eltúnik.
            //Ha a támadó Hero volt, akkor a sikeres támadás után szintet lép.
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
            }
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
                Monster m = new Monster();
                dice = random.Next(1, 7);
                m.Level = GameLevel + _monsterLevels[random.Next(0, 10)];
                m.MaxHealthPoints = 2 * m.Level * dice;
                m.CurrentHealthPoints = m.MaxHealthPoints;
                m.DefendPoints = m.Level * dice / 2;
                m.StrikePoints = m.Level * dice;
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
            Boss b = new Boss();
            int dice = random.Next(1, 7);
            b.Level = GameLevel + _monsterLevels[random.Next(0, 10)];
            b.MaxHealthPoints = 2 * b.Level * dice + dice;
            b.CurrentHealthPoints = b.MaxHealthPoints;
            b.DefendPoints = (int)(b.Level / 2.0 * dice + dice / 2.0);
            b.StrikePoints = b.Level * dice + b.Level;
            SetCoord(b);
            _canvas.Children.Add(b.Picture);
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
