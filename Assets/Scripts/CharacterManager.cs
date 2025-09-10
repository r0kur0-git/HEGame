using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class CharacterManager : MonoBehaviour
    {
        public CharacterController characterController;
        public Animator animator;
        public Wand wand;

        public void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            wand = FindObjectOfType<Wand>();
        }

        private void Start()
        {
            //wand = FindObjectOfType<Wand>();
            wand.OnSpawned += OnWandSpawned; // Subscribe to the OnSpawned event of the Wand script
            AnimationEventSystem.OnAnimationEventTriggered += HandleBasicAnimationEvent;
        }

        #region Attacking
        private void OnWandSpawned(Wand spawnedWand)
        {
            wand = spawnedWand;
        }

        public void HandleBasicAnimationEvent(GameObject target)
        {
            if (target == gameObject)
            {
                ShootBasic();
            }
        }

        public void ShootBasic()
        {
            if (wand != null)
            {
                print("shoot");
                wand.ShootBullet();
            }
        }

        #endregion
    }
}
