using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class LayerChangeOnState : StateMachineBehaviour
    {
        [Header("Layer Settings")]
        [Range(0, 31)] public int targetLayerStart = 0;   // directly assign the layer number in inspector
        [Range(0, 31)] public int targetLayerEnd = 0;   // directly assign the layer number in inspector
        public bool applyToAll = true;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform root = animator.transform.root;

            if (applyToAll)
                SetLayerRecursively(root, targetLayerStart);
            else
                animator.gameObject.layer = targetLayerStart;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Transform root = animator.transform.root;

            if (applyToAll)
                SetLayerRecursively(root, targetLayerEnd);
            else
                animator.gameObject.layer = targetLayerEnd;
        }

        private void SetLayerRecursively(Transform obj, int newLayer)
        {
            obj.gameObject.layer = newLayer;
            foreach (Transform child in obj)
            {
                SetLayerRecursively(child, newLayer);
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
