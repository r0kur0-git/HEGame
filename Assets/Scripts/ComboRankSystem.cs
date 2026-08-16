using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PRJCTA.HOLLOWECHOES
{
    public class ComboRankSystem : MonoBehaviour
    {
        [HideInInspector] public static ComboRankSystem Instance;
        public float rankPoints;
        public string currentRank;

        public float _decayRate = 3f;

        private int[] thresholds = { 0, 100, 200, 400, 800, 950 };
        private string[] ranks = { "D", "C", "B", "A", "S" };

        [Header("UI")]
        [SerializeField] private Slider rankSlider;
        [SerializeField] private TextMeshProUGUI rankText;

        public int currentRankIndex = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (rankSlider != null)
            {
                rankSlider.minValue = 0;
                rankSlider.maxValue = thresholds[thresholds.Length - 1];
                rankSlider.value = 0;
            }
        }

        private void Update()
        {
            if (rankPoints > 0)
            {
                rankPoints -= _decayRate * Time.deltaTime;
                rankPoints = Mathf.Max(rankPoints, 0);
            }

            UpdateRank();
            UpdateUI();
        }

        void UpdateRank()
        {
            float maxPoints = thresholds[thresholds.Length - 1];
            rankPoints = Mathf.Min(rankPoints, maxPoints);

            for (int i = thresholds.Length - 1; i >= 0; i--)
            {
                if (rankPoints >= thresholds[i])
                {
                    currentRankIndex = i;
                    currentRank = ranks[Mathf.Min(i, ranks.Length - 1)];

                    // Only show text if not at the lowest rank
                    if (rankPoints > 0)
                    {
                        rankText.gameObject.SetActive(true);
                        rankText.text = currentRank;
                    }
                    else
                    {
                        rankText.gameObject.SetActive(false);
                    }

                    break;
                }
            }
        }

        public void AddStylePoints(int amount)
        {
            rankPoints += amount;
            rankPoints = Mathf.Min(rankPoints, thresholds[thresholds.Length - 1]);
            UpdateRank();
        }

        void UpdateUI()
        {
            if (rankSlider == null)
            {
                Debug.LogWarning("[ComboRankSystem] rankSlider not assigned in Inspector.");
                return;
            }

            // Top-rank handled specially (no "next" threshold)
            if (currentRankIndex >= thresholds.Length - 1)
            {
                // Make the slider show full (1 of 1) for top rank
                rankSlider.minValue = 0;
                rankSlider.maxValue = 1;
                rankSlider.value = 1;
            }
            else
            {
                int lower = thresholds[currentRankIndex];
                int upper = thresholds[currentRankIndex + 1];
                int range = upper - lower;

                rankSlider.minValue = 0;
                rankSlider.maxValue = range;

                float val = rankPoints - lower;
                val = Mathf.Clamp(val, 0f, range);
                rankSlider.value = val;

                // Debug.Log($"UI: rank={currentRank} idx={currentRankIndex} lower={lower} upper={upper} value={val}");
            }


            if (rankText != null)
            {
                rankText.text = currentRank;
            }
        }
    }
}
