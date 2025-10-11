using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PRJCTA.HOLLOWECHOES
{
    public class BoonManager : MonoBehaviour
    {
        public List<BoonItem> allBoons;            // Master list of boons
        public PlayerInventory playerInventory;    // Player inventory
        public Button[] boonButtons; // Buttons in UI
        public TextMeshProUGUI[] boonNames;
        public TextMeshProUGUI[] boonDescriptions;
        public GameObject objectGroup;
        private List<BoonItem> availableBoons = new List<BoonItem>();
        public Transform centerPoint;

        float spacing = 50f;

#if UNITY_EDITOR
        [ContextMenu("Load All Boons")]
        public void LoadAllBoons()
        {
            allBoons = new List<BoonItem>();

            // Find all BoonItem assets in the project
            string[] guids = AssetDatabase.FindAssets("t:BoonItem");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BoonItem boon = AssetDatabase.LoadAssetAtPath<BoonItem>(path);
                if (boon != null)
                    allBoons.Add(boon);
            }

            // Initialize available boons for runtime
            availableBoons = new List<BoonItem>(allBoons);
            Debug.Log($"Loaded {allBoons.Count} boons automatically.");
        }
#endif

        private void Awake()
        {
            if (availableBoons.Count == 0)
                availableBoons = new List<BoonItem>(allBoons);
            availableBoons = new List<BoonItem>(allBoons);
        }

        public void ShowRandomBoons(int count = 3)
        {
            objectGroup.SetActive(true);

            // Filter available boons
            List<BoonItem> filteredBoons = new List<BoonItem>();
            foreach (var boon in availableBoons)
            {
                bool lowerLevelPicked = true;
                for (int l = 1; l < boon.level; l++)
                {
                    if (!playerInventory.HasBoon(boon.baseName, l))
                    {
                        lowerLevelPicked = false;
                        break;
                    }
                }
                if (lowerLevelPicked)
                    filteredBoons.Add(boon);
            }

            List<BoonItem> tempList = new List<BoonItem>(filteredBoons);

            int activeButtonCount = Mathf.Min(count, tempList.Count);
            float spacing = 150f; // Distance between images

            // Loop through all buttons
            int activeIndex = 0;
            for (int i = 0; i < boonButtons.Length; i++)
            {
                if (i >= activeButtonCount || tempList.Count == 0)
                {
                    boonButtons[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }

                int randIndex = Random.Range(0, tempList.Count);
                BoonItem selectedBoon = tempList[randIndex];
                tempList.RemoveAt(randIndex);

                boonNames[i].text = selectedBoon.itemName;
                boonDescriptions[i].text = selectedBoon.boonDescription;

                boonButtons[i].onClick.RemoveAllListeners();
                BoonItem boonToSelect = selectedBoon;
                boonButtons[i].onClick.AddListener(() => BoonSelect(boonToSelect));

                // Activate image
                boonButtons[i].transform.parent.gameObject.SetActive(true);

                // Calculate position around center
                RectTransform rt = boonButtons[i].transform.parent.GetComponent<RectTransform>();
                float imageWidth = rt.rect.width;
                float totalSpacing = imageWidth + spacing;
                float startX = centerPoint.position.x - ((activeButtonCount - 1) * totalSpacing * 0.5f);
                Vector3 newPos = new Vector3(startX + activeIndex * totalSpacing, centerPoint.position.y, centerPoint.position.z);
                boonButtons[i].transform.parent.position = newPos;

                activeIndex++;
                Debug.Log($"Activating button {i} for boon {selectedBoon.itemName}");
            }
        }

        public void BoonSelect(BoonItem boon)
        {
            if (playerInventory != null)
            {
                playerInventory.AddItem(boon, 1);
                Debug.Log($"Picked boon: {boon.itemName}");
            }

            availableBoons.Remove(boon);

            objectGroup.SetActive(false);
        }
    }
}
