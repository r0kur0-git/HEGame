using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyHealthBar : MonoBehaviour
    {
        public Transform enemyHead;   // Assign enemy's headPoint in Inspector
        public Slider slider;         // Assign the slider prefab
        private Camera cam;
        public GameObject image;

        private void Start()
        {
            cam = Camera.main;
            image.SetActive(false);
        }

        void Update()
        {
            if (enemyHead != null)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(enemyHead.position + Vector3.up);
                slider.transform.position = screenPos;
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetCurrentHealth(int currenthealth)
        {
            slider.value = currenthealth;
        }

        public void SetTarget(Transform enemy)
        {
            enemyHead = enemy;
        }

        public void ShowBar()
        {
            image.SetActive(true);
        }

        public void HideBar()
        {
            image.SetActive(false);
        }
    }
}
