using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;
        Rigidbody enemyRigidbody;

        [Header("Ground Check")]
        public float groundCheckDistance = 0.3f;
        public LayerMask groundLayer;

        public bool isGrounded;

        public void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyManager = GetComponent<EnemyManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        public void HandleDetection()
        {
            
        }

        public void HandleMoveToTarget()
        {
            
        }

        private void Update()
        {
            CheckGrounded();
        }

        private void CheckGrounded()
        {
            bool grounded = Physics.Raycast(
                transform.position + Vector3.up * 0.1f,
                Vector3.down,
                groundCheckDistance,
                groundLayer
            );

            if (grounded != isGrounded) // only update when state changes
            {
                isGrounded = grounded;
                //enemyAnimatorManager.animator.SetBool("isGrounded", isGrounded);

                if (!isGrounded)
                {
                    Debug.Log("Enemy launched / in air!");
                }
                else
                {
                    enemyAnimatorManager.isLaunched = false;
                    Debug.Log("Enemy landed!");
                }
            }
        }

        public void SetLaunchedState(bool launched)
        {
            isGrounded = !launched;
            enemyAnimatorManager.animator.SetBool("isGrounded", !launched);
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.1f,
                            transform.position + Vector3.up * 0.1f + Vector3.down * groundCheckDistance);
        }
    }
}
