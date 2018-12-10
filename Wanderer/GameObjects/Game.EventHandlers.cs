using System;
using System.ComponentModel;

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
            Hero.HasTheKey = enemy.GetType().Equals(typeof(Monster)) ? (enemy as Monster).HasTheKey : false;
            Area[enemy.PositionX, enemy.PositionY].EnemyOnIt = null;
            Enemies.Remove(enemy);
            _canvas.Children.Remove(enemy.Picture);
            CharacterStatModel.Enemy = null;
            Hero.LevelUp(random.Next(1, 7));
        }

        public void HeroHasTheKey(object sender, PropertyChangedEventArgs e)
        {
            LevelUp();
        }
    }
}
