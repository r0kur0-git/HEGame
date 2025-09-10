using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerWeaponInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        WeaponHolderSlot weaponHolderSlot;
        AnimatorHandler animatorHandler;
        public WeaponType weaponType;
        public WeaponItem rightWeapon;
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public int currentRightWeaponIndex = -1;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            animatorHandler = GetComponent<AnimatorHandler>();
        }

        private void Start()
        {
            weaponSlotManager.LoadRightWeaponOnSlot(rightWeapon, true);
            DontDestroyOnLoad(rightWeapon);
        }

        private void Update()
        {
            weaponType = rightWeapon.WeaponType;

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeRightWeapon();
            }
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex++;

            if (currentRightWeaponIndex >= weaponsInRightHandSlots.Length)
            {
                currentRightWeaponIndex = 0;
            }

            if (weaponsInRightHandSlots[currentRightWeaponIndex] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadRightWeaponOnSlot(rightWeapon, true);

                switch (rightWeapon.WeaponType)
                {
                    case WeaponType.Wand:
                        Debug.Log("Switching to Default Animator");
                        animatorHandler.SwitchToDefault();
                        break;
                    case WeaponType.Sword:
                        Debug.Log("Switching to Sword Animator");
                        animatorHandler.SwitchToSword();
                        break;
                }
                Debug.Log($"Equipped: {rightWeapon.WeaponType}");
            }
        }
    }
}
