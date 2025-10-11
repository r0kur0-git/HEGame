using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private void OnDisable()
        {
            SaveToPlayerData();
        }

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        }

        private void Start()
        {
            if (CompareTag("Player"))
            {
                LoadFromPlayerData();
            }
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
                SaveToPlayerData();
                Debug.Log($"Equipped: {rightWeapon.WeaponType}");
            }
        }

        public void SaveToPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;
            data.rightWeapon = rightWeapon;
            data.currentRightWeaponIndex = currentRightWeaponIndex;
            data.weaponsInRightHandSlots = new System.Collections.Generic.List<WeaponItem>(weaponsInRightHandSlots);
        }

        public void LoadFromPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;

            rightWeapon = data.rightWeapon;
            currentRightWeaponIndex = data.currentRightWeaponIndex;

            if (data.weaponsInRightHandSlots != null && data.weaponsInRightHandSlots.Count > 0)
            {
                weaponsInRightHandSlots = data.weaponsInRightHandSlots.ToArray();
            }
        }
    }
}
