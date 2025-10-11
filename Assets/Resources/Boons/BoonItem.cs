using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    [CreateAssetMenu(menuName = "Items/Boon Item")]

    public class BoonItem : Item
    {
        public BoonType boonType;
        public UtilityType utilityType;
        public AbilityType abilityType;
        public WeaponType affectedWeaponType;
        public string baseName;
        public string boonDescription;
        public int level = 1;

        public int flatAttackBonus;
        public float percentAttackBonus;
        public string abilityName;
        public float statIncrease;
        public string utilityEffect;
    }
}
