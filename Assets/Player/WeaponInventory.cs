using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class WeaponInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        WeaponHolderSlot weaponHolderSlot;
        PlayerAnimatorManager playerAnimatorManager;
        public WeaponType weaponType;
        public WeaponItem rightWeapon;
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public int currentRightWeaponIndex = -1;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadRightWeaponOnSlot(rightWeapon, true);
            DontDestroyOnLoad(rightWeapon);
        }

        private void Update()
        {
            weaponType = rightWeapon.WeaponType;
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
                        playerAnimatorManager.SwitchToDefault();
                        break;
                    case WeaponType.Sword:
                        Debug.Log("Switching to Sword Animator");
                        playerAnimatorManager.SwitchToSword();
                        break;
                }
                Debug.Log($"Equipped: {rightWeapon.WeaponType}");
            }
        }
    }
}
