using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyAnimatorManager : AnimatorHandler
    {
        public EnemyManager enemyManager;
        EnemyLocomotionManager enemyLocomotionManager;
        public bool isLaunched = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponent<EnemyManager>();
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
            if (isLaunched)
                return;

            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;
        }

        public override void LaunchUp(float force)
        {
            if (enemyManager != null && enemyManager.enemyRigidbody != null)
            {
                isLaunched = true;
                enemyManager.enemyRigidbody.velocity = new Vector3(0, force, 0);

                isLaunched = true;
                Debug.Log("Enemy Launched!");
            }
        }
    }
}