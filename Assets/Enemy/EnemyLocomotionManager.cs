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

        private void Start()
        {
        }

        public void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyManager = GetComponent<EnemyManager>();
        }

        public void HandleDetection()
        {
            
        }

        public void HandleMoveToTarget()
        {
            
        }
    }
}
