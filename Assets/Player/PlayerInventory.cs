using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<Item> items = new List<Item>();
        private HashSet<string> pickedBoons = new HashSet<string>();
        public int maxSlots = 10;

        private PlayerStats playerStats;

        private void Start()
        {
            LoadInventoryFromPlayerData();
        }

        private void Awake()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        public void AddItem(Item item, int qty = 1)
        {
            if (item is BoonItem boon)
            {
                BoonItem targetBoon = null;

                // Check if same baseName + level already exists
                BoonItem existing = items
                    .OfType<BoonItem>()
                    .FirstOrDefault(b => b.baseName == boon.baseName && b.level == boon.level);

                if (existing != null)
                {
                    existing.quantity += qty;
                    targetBoon = existing;

                    Debug.Log($"Added {qty} {boon.itemName} (level {boon.level}). Now you have {existing.quantity}.");
                }
                else if (items.Count < maxSlots)
                {
                    // Clone before storing
                    BoonItem newBoon = ScriptableObject.Instantiate(boon);
                    newBoon.quantity = qty;
                    items.Add(newBoon);
                    targetBoon = newBoon;

                    Debug.Log($"Picked up new boon: {newBoon.itemName} x{qty} (level {newBoon.level}, Type: {newBoon.boonType})");
                }
                else
                {
                    Debug.Log("Inventory full! Could not add boon.");
                    return;
                }

                playerStats.ApplyBoon(targetBoon);

                pickedBoons.Add($"{targetBoon.baseName}_L{targetBoon.level}");
            }
            else
            {
                // Regular items
                Item existingItem = items.Find(i => i.itemName == item.itemName);
                if (existingItem != null)
                {
                    existingItem.quantity += qty;
                }
                else if (items.Count < maxSlots)
                {
                    Item newItem = ScriptableObject.Instantiate(item);
                    newItem.quantity = qty;
                    items.Add(newItem);
                }
                else
                {
                    Debug.Log("Inventory full! Could not add item.");
                    return;
                }

                Debug.Log($"Picked up item: {item.itemName} x{qty}");
            }

            SaveInventoryToPlayerData();
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

            SaveInventoryToPlayerData();
        }

        public bool HasBoon(string baseName, int level)
        {
            foreach (var item in items)
            {
                if (item is BoonItem b && b.baseName == baseName && b.level == level)
                    return true;
            }
            return false;
        }

        public void SaveInventoryToPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;
            data.inventoryItems = new List<Item>(items);
        }

        public void LoadInventoryFromPlayerData()
        {
            var data = PlayerDataManager.Instance.playerData;
            items = new List<Item>(data.inventoryItems);
        }
    }
}
