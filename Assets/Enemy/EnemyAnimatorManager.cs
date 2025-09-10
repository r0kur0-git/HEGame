using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyAnimatorManager : AnimatorHandler
    {
        EnemyManager enemyManager;
        EnemyLocomotionManager enemyLocomotionManager;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        public void EnableParrying()
        {
            //enemyManager.isParrying = true;
        }

        public void DisableParrying()
        {
            //enemyManager.isParrying = false;
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}