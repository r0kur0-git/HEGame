using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class DamageApplicator : MonoBehaviour
    {
        public float life = 3f;
        public int currentWeaponDamage = 35;
        public ElementType elementalType = ElementType.None;
        public ObjectType objectType = ObjectType.None;

        //public float raycastDistance = 0.1f;

        private Vector3 previousPosition;
        private Rigidbody rb;
        Collider damageCollider;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            if (CompareTag("Bullet"))
            {
                Destroy(gameObject, life);
            }
            else
            {
                return;
            }
        }

        private void Start()
        {
            previousPosition = transform.position;
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (CompareTag("Bullet"))
            {
                StartCoroutine(Predict());
            }
            else
            {
                return;
            }
        }

        IEnumerator Predict()
        {
            Vector3 prediction = transform.position + rb.velocity * Time.fixedDeltaTime;
            RaycastHit hit2;

            int layerMask = ~(LayerMask.GetMask("Bullet") | LayerMask.GetMask("Spells") | LayerMask.GetMask("Controller")); // Ignore Bullet and Spell layers

            if (Physics.Linecast(transform.position, prediction, out hit2, layerMask))
            {
                transform.position = hit2.point;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
                yield return 0;
                OnTriggerEnter(hit2.collider);
            }
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (objectType)
            {
                case ObjectType.Enemy:
                // Damage the enemy
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    switch (elementalType)
                    {
                        case ElementType.None:
                            enemyStats.EnemyTakeDamage(currentWeaponDamage, ElementType.None);
                            break;
                        case ElementType.Fire:
                            enemyStats.EnemyTakeDamage(currentWeaponDamage, ElementType.Fire);
                            break;
                        case ElementType.Ice:
                            enemyStats.EnemyTakeDamage(currentWeaponDamage, ElementType.Ice);
                            break;
                        case ElementType.Electric:
                            enemyStats.EnemyTakeDamage(currentWeaponDamage, ElementType.Electric);
                            break;
                        default:
                            enemyStats.EnemyTakeDamage(currentWeaponDamage, ElementType.None);
                            break;
                    }
                }

                if (CompareTag("Bullet") && other.CompareTag("Enemy"))
                {
                    Destroy(gameObject);
                }

                    break;

                case ObjectType.Player:
                    PlayerStats playerStats = other.GetComponent<PlayerStats>();

                    if (playerStats != null)
                    {
                        switch (elementalType)
                        {
                            case ElementType.None:
                                playerStats.PlayerTakeDamage(currentWeaponDamage, ElementType.None);
                                break;
                            case ElementType.Fire:
                                playerStats.PlayerTakeDamage(currentWeaponDamage, ElementType.Fire);
                                break;
                            case ElementType.Ice:
                                playerStats.PlayerTakeDamage(currentWeaponDamage, ElementType.Ice);
                                break;
                            case ElementType.Electric:
                                playerStats.PlayerTakeDamage(currentWeaponDamage, ElementType.Electric);
                                break;
                            default:
                                playerStats.PlayerTakeDamage(currentWeaponDamage, ElementType.None);
                                break;
                        }
                    }
                    break;
            }

            if (CompareTag("Bullet") && other.CompareTag("Object"))
            {
                Destroy(gameObject);
            }
        }
    }
}
