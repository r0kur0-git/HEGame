using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class BoonManager : MonoBehaviour
    {
        public BoonItem boonItem;
        public PlayerInventory playerInventory; // Drag Player’s Inventory here in Inspector
        public GameObject objectGroup;

        public void BoonSelect(BoonItem boonItem)
        {
            Debug.Log("button pressed");
            if (playerInventory == null)
            {
                Debug.LogWarning("No Inventory assigned!");
                return;
            }

            playerInventory.AddItem(boonItem, 1);
            Debug.Log($"Boon selected: {boonItem.itemName} ({boonItem.boonType})");

            objectGroup.SetActive(false);
        }
    }
}
