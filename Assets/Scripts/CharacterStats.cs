using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 20;
        public int maxHealth;
        public int currentHealth;
        public int healingDone;
        public int baseAttackDamage = 10;
        public int bonusAttackDamage = 0;

        public int TotalAttackDamage => baseAttackDamage + bonusAttackDamage;

        public bool isDead;

        public EnemyHealthBar enemyHealthbar;
        public HealthBar healthbar;
        public Canvas canvas;
    }
}
