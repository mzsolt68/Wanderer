namespace Wanderer.GameCharacters
{
    public class Boss : Enemy
    {
        public Boss(int gameLevel, int enemyLevel, int dice)
        {
            Level = gameLevel + _monsterLevels[enemyLevel];
            CurrentHealthPoints = 2 * Level * dice + dice;
            DefendPoints = (int)(Level / 2.0 * dice + dice / 2.0);
            StrikePoints = Level * dice + Level;
        }
    }
}
