using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]

    public class WeaponItem : Item
    {
        public WeaponType WeaponType;
        public GameObject modelPrefab;
        public RuntimeAnimatorController animatorController;
        public bool isUnarmed;

        [Header("Ground Attack")]
        public string AttackSeq1;
        public string AttackSeq2;
        public string AttackSeq3;

        [Header("Special Attack")]
        public string AttackSpecial1;

        [Header("Aerial Attack")]
        public string A_AttackSeq1;
        public string A_AttackSeq2;
        public string A_AttackSeq3;

        [Header("Spells")]
        public string SpellOne;
        public string SpellTwo;
        public string SpellThree;
    }
}