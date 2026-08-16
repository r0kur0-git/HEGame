using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        public bool isUpperAttack;
        
        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isInteracting", isInteracting);
            animator.CrossFade(targetAnim, 0f);
        }

        public virtual void LaunchUp(float force)
        {
            isUpperAttack = true;
            Debug.LogWarning("LaunchUp not implemented for this character!");
        }
    }
}