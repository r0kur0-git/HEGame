using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class CharacterStats : MonoBehaviour
    {
        public float healthLevel = 10;
        public int manaLevel = 10;
        public float maxHealth;
        public float currentHealth;
        public int maxMana;
        public float currentMana;
        public int healingDone;
        public int baseAttackDamage = 10;
        public int bonusAttackDamage = 0;

        public int TotalAttackDamage => baseAttackDamage + bonusAttackDamage;

        public bool isDead;

        public EnemyHealthBar enemyHealthbar;
        public HealthBar healthbar;
        public ManaBar manaBar;
        public Canvas canvas;
    }
}
