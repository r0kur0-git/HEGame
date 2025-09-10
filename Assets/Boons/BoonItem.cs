using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public enum BoonType
    {
        weaponUpgrade,
        abilityUpgrade,
        healthUpgrade,
        speedUpgrade,
        utility,
    }

    public enum UtilityType
    {
        doubleJump,
        doubleDash,
        HealingOverTime,
    }

    [CreateAssetMenu(menuName = "Items/Boon Item")]

    public class BoonItem : Item
    {
        public BoonType boonType;
        public UtilityType utilityType;
        public string boonDescription;

        public int flatAttackBonus;
        public float percentAttackBonus;
        public string abilityName;
        public int statIncrease;
        public string utilityEffect;

        public WeaponType affectedWeaponType = WeaponType.All;
    }
}
