using System.Collections.Generic;

namespace Wanderer.GameCharacters
{
    public interface ICharacterFactory
    {
        Hero CreateHero();
        Monster CreateMonster(int gameLevel);
        Boss CreateBoss(int gameLevel);

        // Creates N monsters and ensures exactly one carries the key.
        IReadOnlyList<Monster> CreateMonsters(int gameLevel, int minInclusive = 2, int maxInclusive = 5);
    }
}