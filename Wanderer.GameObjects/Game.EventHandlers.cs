using System;
using System.ComponentModel;
using Wanderer.GameCharacters;

namespace Wanderer.GameObjects
{
    public partial class Game
    {
        private void MoveEnemies(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //Itt kell megvalósítani az ellenfelek mozgását
            foreach (var enemy in Enemies)
            {
                MoveCharacter(enemy, enemy.Direction);
            }
        }

        private void EnemyDied(object sender, PropertyChangedEventArgs e)
        {
            var enemy = sender as Enemy;
            var hasKey = enemy is Monster m && m.HasTheKey;
            Area[enemy.PositionX, enemy.PositionY].EnemyOnIt = null;
            Enemies.Remove(enemy);
            enemy.EnemyDied -= EnemyDied;
            _renderer.Remove(enemy);
            if(CharacterStatModel.Enemy == enemy)
            {
                CharacterStatModel.Enemy = null;
            }
            if (hasKey)
            {
                Hero.HasTheKey = true;
            }
            Hero.LevelUp(random.Next(1, 7));
        }

        public void HeroHasTheKey(object sender, PropertyChangedEventArgs e)
        {
            LevelUp();
        }
    }
}
