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
            boon.itemName = (string)EditorGUILayout.TextField("Item Name", boon.itemName);
            boon.boonType = (BoonType)EditorGUILayout.EnumPopup("Boon Type", boon.boonType);
            boon.boonDescription = EditorGUILayout.TextField("Description", boon.boonDescription);

            // Show fields depending on enum
            switch (boon.boonType)
            {
                case BoonType.weaponUpgrade:
                    boon.flatAttackBonus = EditorGUILayout.IntField("Flat Attack Bonus", boon.flatAttackBonus);
                    boon.percentAttackBonus = EditorGUILayout.FloatField("Percent Attack Bonus", boon.percentAttackBonus);
                    break;

                case BoonType.speedUpgrade:
                    boon.statIncrease = EditorGUILayout.IntField("Health Increase", boon.statIncrease);
                    break;

                case BoonType.abilityUpgrade:
                    boon.abilityName = EditorGUILayout.TextField("Ability Name", boon.abilityName);
                    break;

                case BoonType.healthUpgrade:
                    boon.statIncrease = EditorGUILayout.IntField("Health Increase", boon.statIncrease);
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
