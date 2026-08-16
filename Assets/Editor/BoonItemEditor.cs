using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PRJCTA.HOLLOWECHOES
{
    [CustomEditor(typeof(BoonItem))]
    public class BoonItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BoonItem boon = (BoonItem)target;

            // Always show type & description
            boon.baseName = (string)EditorGUILayout.TextField("Boon Name", boon.baseName);
            boon.itemName = (string)EditorGUILayout.TextField("Item Name", boon.itemName);
            boon.boonType = (BoonType)EditorGUILayout.EnumPopup("Boon Type", boon.boonType);
            boon.boonDescription = EditorGUILayout.TextField("Description", boon.boonDescription);
            boon.level = EditorGUILayout.IntField("Level", boon.level);

            // Show fields depending on enum
            switch (boon.boonType)
            {
                case BoonType.weaponUpgrade:
                    boon.affectedWeaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", boon.affectedWeaponType);
                    boon.flatAttackBonus = EditorGUILayout.IntField("Flat Attack Bonus", boon.flatAttackBonus);
                    boon.percentAttackBonus = EditorGUILayout.FloatField("Percent Attack Bonus", boon.percentAttackBonus);
                    break;

                case BoonType.speedUpgrade:
                    boon.statIncrease = EditorGUILayout.FloatField("Speed Increase", boon.statIncrease);
                    break;

                case BoonType.abilityUpgrade:
                    boon.abilityType = (AbilityType)EditorGUILayout.EnumPopup("Ability Type", boon.abilityType);
                    break;

                case BoonType.healthUpgrade:
                    boon.statIncrease = EditorGUILayout.FloatField("Health Increase", boon.statIncrease);
                    break;

                case BoonType.utility:
                    boon.utilityType = (UtilityType)EditorGUILayout.EnumPopup("Utility Effect", boon.utilityType);
                    break;
            }

            if (GUI.changed)
                EditorUtility.SetDirty(boon);
        }
    }
}
