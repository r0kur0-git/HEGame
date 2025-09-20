using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerStats : CharacterStats
    {
        Animator animator;
        PlayerLocomotion locomotion;
        DamageApplicator damageApplicator;
        public WeaponType currentWeaponType;
        public GameObject destroyOnDeath;

        public Dictionary<WeaponType, int> flatBonuses = new Dictionary<WeaponType, int>();
        public Dictionary<WeaponType, float> percentBonuses = new Dictionary<WeaponType, float>();

        public bool swordUnlocked = false;
        public bool swordExtension;
        public bool hasFireDoT;
        public bool hasIceDoT;
        public bool hasLightningDoT;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            locomotion = GetComponent<PlayerLocomotion>();

            foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType)))
            {
                flatBonuses[type] = 0;
                percentBonuses[type] = 0;
            }
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void PlayerTakeDamage(int damage, ElementType elementalType)
        {
            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth(currentHealth);

            animator.Play("Hit");

            if (currentHealth <= 0)
            {
                isDead = true;
                gameObject.tag = "Dead";
                currentHealth = 0;

                if (isDead)
                {
                    animator.Play("Death");
                }
            }

            switch (elementalType)
            {
                case ElementType.None:
                    break;
                case ElementType.Fire:
                    print("burning");
                    break;
                case ElementType.Ice:
                    print("freezing");
                    break;
                case ElementType.Electric:
                    print("shock");
                    break;
                // Add cases for other elemental types if needed
                default:
                    break;
            }
        }

        public void ApplyBoon(BoonItem boon)
        {
            switch (boon.boonType)
            {
                case BoonType.healthUpgrade:
                    maxHealth += boon.statIncrease;
                    currentHealth = maxHealth;
                    Debug.Log($"Applied {boon.itemName}: Health increased to {maxHealth}");
                    break;

                case BoonType.speedUpgrade:
                    locomotion.movementSpeed += boon.statIncrease;
                    Debug.Log($"Applied {boon.itemName}: Speed increased to {locomotion.movementSpeed}");
                    break;

                case BoonType.weaponUpgrade:
                    flatBonuses[boon.affectedWeaponType] += boon.flatAttackBonus;
                    percentBonuses[boon.affectedWeaponType] += boon.percentAttackBonus;

                    Debug.Log($"Applied {boon.itemName} + {boon.flatAttackBonus} flat, + {boon.percentAttackBonus * 100}% for {boon.affectedWeaponType}");
                    break;

                case BoonType.abilityUpgrade:
                    switch (boon.abilityName)
                    {
                        case "switch sword":
                            UnlockSword(WeaponType.Sword);
                            Debug.Log($"Applied ability upgrade: {boon.itemName}");
                            break;

                        case "Fire DoT":
                            hasFireDoT = true;
                            Debug.Log($"Applied ability upgrade: {boon.itemName}");
                            break;

                        case "Ice DoT":
                            hasIceDoT = true;
                            Debug.Log($"Applied ability upgrade: {boon.itemName}");
                            break;

                        case "Lightning DoT":
                            hasLightningDoT = true;
                            Debug.Log($"Applied ability upgrade: {boon.itemName}");
                            break;

                        case "Sword Combo Ex":
                            if (swordUnlocked == true)
                            {
                                SwordEx(WeaponType.Sword);
                                Debug.Log($"Applied ability upgrade: {boon.itemName}");
                            }
                            break;
                    }
                    break;

                case BoonType.utility:
                    switch (boon.utilityType)
                    {
                        case UtilityType.doubleJump:
                            locomotion.maxJumps = 2;
                            Debug.Log($"Applied {boon.itemName} + Obtained double jump");
                            break;

                        case UtilityType.doubleDash:
                            locomotion.maxDashes = 2;
                            Debug.Log($"Applied {boon.itemName} + Obtained double dash");
                            break;

                        case UtilityType.HealingOverTime:
                            break;
                    }
                    break;
            }
        }

        public int GetAttackDamage(WeaponType type)
        {
            int damage = baseAttackDamage;

            damage += flatBonuses[type];
            damage += flatBonuses[WeaponType.All];

            float multiplier = 1f + percentBonuses[type] + percentBonuses[WeaponType.All];
            damage = Mathf.RoundToInt(damage * multiplier);

            return damage;
        }

        public void UnlockSword(WeaponType type)
        {
            type = WeaponType.Sword;
            swordUnlocked = true;
            Debug.Log("Player weapon type unlocked: " + type);
        }

        public void SwordEx(WeaponType type)
        {
            type = WeaponType.Sword;
            swordExtension = true;
            Debug.Log("Player weapon combo unlocked: " + type + " extension");
        }
    }
}
