using System;
using System.Collections.Generic;

namespace Wanderer.GameCharacters
{
    public sealed class CharacterFactory : ICharacterFactory
    {
        private readonly Random _random;

        public CharacterFactory(Random random)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        private int RollDice() => _random.Next(1, 7);
        private int RollEnemyLevel() => _random.Next(0, 10);

        public Hero CreateHero()
        {
            return new Hero(RollDice());
        }

        public Monster CreateMonster(int gameLevel)
        {
            return new Monster(gameLevel, RollEnemyLevel(), RollDice());
        }

        public Boss CreateBoss(int gameLevel)
        {
            return new Boss(gameLevel, RollEnemyLevel(), RollDice());
        }

        public IReadOnlyList<Monster> CreateMonsters(int gameLevel, int minInclusive = 2, int maxInclusive = 5)
        {
            if (minInclusive < 0) minInclusive = 0;
            if (maxInclusive < minInclusive) maxInclusive = minInclusive;

            int count = _random.Next(minInclusive, maxInclusive + 1);

            var monsters = new List<Monster>(count);
            for (int i = 0; i < count; i++)
            {
                monsters.Add(CreateMonster(gameLevel));
            }

            if (monsters.Count > 0)
            {
                monsters[_random.Next(0, monsters.Count)].HasTheKey = true;
            }

            return monsters;
        }
    }
}