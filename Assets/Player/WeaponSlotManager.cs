using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        public bool vfx;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadRightWeaponOnSlot(WeaponItem weaponItem, bool isRight)
        {
            if (weaponItem == null) return;

            rightHandSlot.UnloadWeaponAndDestroy();
            if (isRight)
            {
                rightHandSlot.LoadWeaponModel(weaponItem);
            }
        }
    }
}
