using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<Item> items = new List<Item>();
        public int maxSlots = 10;

        private PlayerStats playerStats;

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        public void AddItem(Item item, int qty = 1)
        {
            Item existing = items.Find(i => i.itemName == item.itemName);

            if (existing != null)
            {
                existing.quantity += qty;
                Debug.Log($"Added {qty} {item.itemName}. Now you have {existing.quantity}.");

                // If it’s a BoonItem, log its type too
                if (existing is BoonItem existingBoon)
                {
                    Debug.Log($"Boon type: {existingBoon.boonType}");
                    playerStats.ApplyBoon(existingBoon);
                }
            }
            else if (items.Count < maxSlots)
            {
                Item newItem = ScriptableObject.Instantiate(item);
                newItem.quantity = qty;

                items.Add(newItem);

                if (newItem is BoonItem boon)
                {
                    Debug.Log($"Picked up new boon: {boon.itemName} x{qty} (Type: {boon.boonType})");
                    playerStats.ApplyBoon(boon);
                }
                else
                {
                    Debug.Log($"Picked up new item: {newItem.itemName} x{qty}");
                }
            }
            else
            {
                Debug.Log("Inventory full!");
            }
        }


        public void RemoveItem(Item item, int qty = 1)
        {
            Item existing = items.Find(i => i.itemName == item.itemName);

            if (existing != null)
            {
                existing.quantity -= qty;
                if (existing.quantity <= 0)
                    items.Remove(existing);

                Debug.Log($"Removed {qty} {item.itemName}. Remaining: {existing?.quantity ?? 0}");
            }
        }
    }
}
