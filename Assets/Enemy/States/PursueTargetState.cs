using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public IdleState idleState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (enemyManager.distanceFromTarget > enemyManager.detectionRadius)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return idleState;
            }

            if (enemyManager.distanceFromTarget > enemyManager.maximumFollowRange && enemyManager.distanceFromTarget <= enemyManager.detectionRadius)
            {
                enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                HandleRotateTowardsTarget(enemyManager);
            }

            if (enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange + 0.5f)
            {
                return combatStanceState;
            }

            HandleRotateTowardsTarget(enemyManager);

            enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            return this;
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            if (enemyManager.isPerformingAction)
            {
                // Manual rotation during attack animations
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;

                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                    enemyManager.transform.rotation = Quaternion.Slerp(
                        enemyManager.transform.rotation,
                        targetRotation,
                        enemyManager.rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Rotate based on navmesh path
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                Vector3 velocity = enemyManager.navMeshAgent.desiredVelocity;
                if (velocity.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
                    enemyManager.transform.rotation = Quaternion.Slerp(
                        enemyManager.transform.rotation,
                        targetRotation,
                        enemyManager.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
