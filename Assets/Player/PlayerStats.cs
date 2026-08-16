using System;
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
        private Dictionary<string, int> boonLevels = new Dictionary<string, int>();

        public bool swordUnlocked = false;
        public bool swordExtension;
        public bool hasFireDoT;
        public bool hasIceDoT;
        public bool hasLightningDoT;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            locomotion = GetComponent<PlayerLocomotion>();            
        }

        void Start()
        {
            LoadFromPlayerData();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth((int)maxHealth);

            maxMana = SetMaxManaFromManaLevel();
            manaBar.SetMaxMana(maxMana);
        }

        private void OnEnable()
        {
            FindCanvas();
        }

        private void OnDisable()
        {
            SaveToPlayerData();
        }

        void FindCanvas()
        {
            canvas = FindObjectOfType<Canvas>();

            if (canvas != null)
            {
                manaBar = canvas.GetComponentInChildren<ManaBar>(true);
            }
        }

        private void Update()
        {
            if (ComboRankSystem.Instance != null)
            {
                switch (ComboRankSystem.Instance.currentRank)
                {
                    case "D":
                        currentMana -= 4 * Time.deltaTime;
                        break;
                    case "C":
                        currentMana -= 2 * Time.deltaTime;
                        break;
                    case "B":
                        currentMana -= 2 * Time.deltaTime;
                        break;
                    case "A":
                        currentMana -= 1 * Time.deltaTime;
                        break;
                    case "S":
                        break;
                    default:
                        currentMana -= 7 * Time.deltaTime;
                        break;
                }
            }
            currentMana = Mathf.Max(currentMana, 0);
            manaBar.SetCurrentMana(currentMana);
        }

        private float SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxManaFromManaLevel()
        {
            maxMana = manaLevel * 10;
            return maxMana;
        }

        public void GainMana(int amount)
        {
            currentMana += amount;

            if (currentMana > maxMana)
                currentMana = maxMana;

            Debug.Log("Player gained mana. Current mana: " + currentMana);
        }

        public void PlayerTakeDamage(int damage, ElementType elementalType)
        {
            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth((int)currentHealth);

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
            string key = boon.baseName;
            int currentLevel = boonLevels.ContainsKey(key) ? boonLevels[key] : 0;

            // Only apply if this boon’s level is exactly next
            if (boon.level == currentLevel + 1)
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
                        switch (boon.abilityType)
                        {
                            case AbilityType.SwitchSword:
                                if (swordUnlocked == false)
                                {
                                    UnlockSword(WeaponType.Sword);
                                }
                                else if (swordUnlocked == true)
                                {
                                    swordExtension = true;
                                    SwordEx(WeaponType.Sword);
                                }
                                Debug.Log($"Applied ability upgrade: {boon.itemName}");
                                break;

                            case AbilityType.FireDoT:
                                hasFireDoT = true;
                                Debug.Log($"Applied ability upgrade: {boon.itemName}");
                                break;

                            case AbilityType.IceDoT:
                                hasIceDoT = true;
                                Debug.Log($"Applied ability upgrade: {boon.itemName}");
                                break;

                            case AbilityType.LightningDoT:
                                hasLightningDoT = true;
                                Debug.Log($"Applied ability upgrade: {boon.itemName}");
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

                boonLevels[key] = boon.level;
            }
            else
            {
                Debug.Log($"Cannot apply {boon.itemName} Level {boon.level}, previous levels missing.");
            }

            
        }

        public int GetAttackDamage(WeaponType type)
        {
            int damage = baseAttackDamage;

            damage += flatBonuses[type];

            float multiplier = 1f + percentBonuses[type];
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
            Debug.Log("Player weapon combo unlocked: " + type + " extension");
        }

        private void InitializeDictionaries()
        {
            foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
            {
                if (!flatBonuses.ContainsKey(type))
                    flatBonuses[type] = 0;

                if (!percentBonuses.ContainsKey(type))
                    percentBonuses[type] = 0f;
            }
        }

        public void SaveToPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;

            data.flatBonuses = new Dictionary<WeaponType, int>(flatBonuses);
            data.percentBonuses = new Dictionary<WeaponType, float>(percentBonuses);
            InitializeDictionaries();

            data.maxHealth = (int)maxHealth;
            data.currentHealth = (int)currentHealth;
            data.maxMana = maxMana;
            data.currentMana = currentMana;
            data.baseAttackDamage = baseAttackDamage;

            data.swordUnlocked = swordUnlocked;
            data.swordExtension = swordExtension;
            data.hasFireDoT = hasFireDoT;
            data.hasIceDoT = hasIceDoT;
            data.hasLightningDoT = hasLightningDoT;

            // movement upgrades
            data.maxJumps = locomotion.maxJumps;
            data.maxDashes = locomotion.maxDashes;
        }

        public void LoadFromPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;

            flatBonuses = new Dictionary<WeaponType, int>(data.flatBonuses);
            percentBonuses = new Dictionary<WeaponType, float>(data.percentBonuses);
            InitializeDictionaries();

            maxHealth = data.maxHealth;
            currentHealth = data.currentHealth;
            maxMana = data.maxMana;
            currentMana = data.currentMana;
            baseAttackDamage = data.baseAttackDamage;

            swordUnlocked = data.swordUnlocked;
            swordExtension = data.swordExtension;
            hasFireDoT = data.hasFireDoT;
            hasIceDoT = data.hasIceDoT;
            hasLightningDoT = data.hasLightningDoT;

            locomotion.maxJumps = data.maxJumps;
            locomotion.maxDashes = data.maxDashes;
            locomotion.dashCooldown = data.dashCooldown;
            locomotion.dashChainReset = data.dashChainReset;

            // refresh UI
            healthbar.SetMaxHealth((int)maxHealth);
            healthbar.SetCurrentHealth((int)currentHealth);
            manaBar.SetMaxMana(maxMana);
            manaBar.SetCurrentMana(currentMana);
        }
    }
}
