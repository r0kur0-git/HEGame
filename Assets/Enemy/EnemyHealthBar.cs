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
        }

        void Update()
        {
            if (Wand.instance != null && Wand.instance.nearestEnemy != null)
            {
                enemyHead = Wand.instance.nearestEnemy.transform;
                Vector3 screenPos = cam.WorldToScreenPoint(enemyHead.position + Vector3.up * 2f);
                slider.transform.position = screenPos;
            }

            if (Wand.instance.nearestEnemy == null)
            {
                image.SetActive(false);
            }
            else
            {
                image.SetActive(true);
            }
            
            //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
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
            enemyHead = enemy; // now it will follow this enemy
        }
    }
}
