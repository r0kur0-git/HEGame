using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class SwitchWeaponAnimationTEST : MonoBehaviour
    {
        public Animator animator;
        public RuntimeAnimatorController baseController;
        public AnimatorOverrideController swordController;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchToSword();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchToDefault();
            }
        }

        void SwitchToSword()
        {
            animator.runtimeAnimatorController = swordController;
        }

        void SwitchToDefault()
        {
            animator.runtimeAnimatorController = baseController;
        }
    }
}
