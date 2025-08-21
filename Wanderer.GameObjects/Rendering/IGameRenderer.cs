using Wanderer.GameCharacters;

namespace Wanderer.GameObjects.Rendering
{
    public interface IGameRenderer
    {
        void RenderArea(Tile[,] area);

        // Characters
        void Spawn(Character character);
        void Remove(Character character);
        void UpdatePosition(Character character);

        // Orientation for the hero (renderer decides which sprite to use)
        void SetFacing(Hero hero, Direction direction);
    }
}