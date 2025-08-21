using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wanderer.GameCharacters;

namespace Wanderer.GameObjects.Rendering
{
    public sealed class WpfGameRenderer : IGameRenderer
    {
        private const int CellSize = 72;

        private readonly Canvas _canvas;
        private readonly List<Image> _tileImages = new List<Image>();
        private readonly Dictionary<Character, Image> _characterImages = new Dictionary<Character, Image>();
        private readonly Dictionary<Hero, Direction> _heroFacing = new Dictionary<Hero, Direction>();

        public WpfGameRenderer(Canvas canvas)
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

        public void RenderArea(Tile[,] area)
        {
            // Clear previous tiles
            foreach (var img in _tileImages)
            {
                _canvas.Children.Remove(img);
            }
            _tileImages.Clear();

            int width = area.GetLength(0);
            int height = area.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var img = new Image { Width = CellSize, Height = CellSize };
                    img.Source = new BitmapImage(new Uri(
                        area[x, y].Type == TileType.Floor ? "../Images/floor.png" : "../Images/wall.png",
                        UriKind.Relative));

                    _canvas.Children.Add(img);
                    Canvas.SetLeft(img, x * CellSize);
                    Canvas.SetTop(img, y * CellSize);
                    _tileImages.Add(img);
                }
            }
        }

        public void Spawn(Character character)
        {
            if (_characterImages.ContainsKey(character))
                return;

            var img = new Image { Width = CellSize, Height = CellSize };
            img.Source = SelectCharacterSprite(character);

            _canvas.Children.Add(img);
            _characterImages[character] = img;

            UpdatePosition(character);
        }

        public void Remove(Character character)
        {
            if (_characterImages.TryGetValue(character, out var img))
            {
                _canvas.Children.Remove(img);
                _characterImages.Remove(character);
            }
            if (character is Hero h)
            {
                _heroFacing.Remove(h);
            }
        }

        public void UpdatePosition(Character character)
        {
            if (_characterImages.TryGetValue(character, out var img))
            {
                Canvas.SetLeft(img, character.PositionX * CellSize);
                Canvas.SetTop(img, character.PositionY * CellSize);
            }
        }

        public void SetFacing(Hero hero, Direction direction)
        {
            _heroFacing[hero] = direction;

            if (_characterImages.TryGetValue(hero, out var img))
            {
                img.Source = SelectHeroSprite(direction);
            }
        }

        private BitmapImage SelectCharacterSprite(Character character)
        {
            if (character is Hero hero)
            {
                var dir = _heroFacing.TryGetValue(hero, out var d) ? d : Direction.Down;
                return SelectHeroSprite(dir);
            }
            if (character is Boss)
            {
                return new BitmapImage(new Uri("../Images/boss.png", UriKind.Relative));
            }
            // default: monster
            return new BitmapImage(new Uri("../Images/skeleton.png", UriKind.Relative));
        }

        private BitmapImage SelectHeroSprite(Direction direction)
        {
            string path = "../Images/hero-down.png";
            switch (direction)
            {
                case Direction.Left:  path = "../Images/hero-left.png";  break;
                case Direction.Right: path = "../Images/hero-right.png"; break;
                case Direction.Up:    path = "../Images/hero-up.png";    break;
                case Direction.Down:  path = "../Images/hero-down.png";  break;
            }
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
    }
}