using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    [System.Serializable]
    public class PlayerData
    {
        // Core Stats
        public int maxHealth;
        public int currentHealth;
        public int maxMana;
        public float currentMana;

        // Base Attack and Bonuses
        public int baseAttackDamage;
        public Dictionary<WeaponType, int> flatBonuses = new Dictionary<WeaponType, int>();
        public Dictionary<WeaponType, float> percentBonuses = new Dictionary<WeaponType, float>();

        // Unlocks and Flags
        public bool swordUnlocked;
        public bool swordExtension;
        public bool hasFireDoT;
        public bool hasIceDoT;
        public bool hasLightningDoT;

        // Inventory
        public List<Item> inventoryItems = new List<Item>();

        // Weapon data
        public WeaponItem rightWeapon; // currently equipped
        public int currentRightWeaponIndex;
        public List<WeaponItem> weaponsInRightHandSlots = new List<WeaponItem>();

        // Example movement abilities
        public int maxJumps;
        public int maxDashes;
        public int dashCooldown;
        public int dashChainReset;

        // Animator
        public WeaponType currentWeaponType = WeaponType.All;

        public PlayerData() { }
    }
}
