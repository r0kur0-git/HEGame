using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot rightHandSlot;
        DamageApplicator damageCollider;

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
                LoadRightWeaponDamageCollider();
            }
        }

        private void LoadRightWeaponDamageCollider()
        {
            damageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageApplicator>();
        }

        public void OpenRightDamageCollider()
        {
            damageCollider.EnableDamageCollider();
            vfx = true;
        }
        public void CloseRightDamageCollider()
        {
            damageCollider.DisableDamageCollider();
            vfx = false;
        }

    }
}
