using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace PRJCTA.HOLLOWECHOES
{
    public class Wand : MonoBehaviour
    {
        public Transform playerTransform;
        public Transform bulletSpawnPoint;
        public Transform[] spawnPoints;
        public GameObject bulletPrefab;
        public GameObject projectilePrefab;
        public GameObject spellOnePrefab;
        public GameObject spellTwoPrefab;
        public GameObject spellThreePrefab;
        public float bulletSpeed = 200;
        public float birdSpeed = 200;
        public float maxDistance = 10;
        public static List<GameObject> enemies = new List<GameObject>();
        public GameObject nearestEnemy;
        public string tagToDetect = "Lock On";
        public string spawnerTag = "Spawners";

        public event Action<Wand> OnSpawned;
        public static Wand instance;

        public PlayerStats playerStats;
        public WeaponType weaponType;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            playerTransform = FindObjectOfType<PlayerStats>().transform;
        }

        private void Start()
        {
            playerStats = GetComponentInParent<PlayerStats>();
        }

        void Update()
        {
            enemies = enemies.Where(f => f != null).ToList();

            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) > maxDistance)
            {
                nearestEnemy = null;
            }

            //print(nearestEnemy != null ? nearestEnemy.name : "No enemy nearby");
        }

        private void OnDisable()
        {
            Debug.Log($"{name}: {GetType().Name} was disabled!", this);
        }

        public static void RegisterEnemy(GameObject enemy)
        {
            if (!enemies.Contains(enemy))
                enemies.Add(enemy);
        }
        public static void UnregisterEnemy(GameObject enemy)
        {
            if (enemies.Contains(enemy))
                enemies.Remove(enemy);
        }

        public void ShootBullet()
        {
            nearestEnemy = NearestEnemy();

            Vector3 targetDirection;

            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= maxDistance)
            {
                targetDirection = (nearestEnemy.transform.position - bulletSpawnPoint.position).normalized;
            }
            else
            {
                targetDirection = playerTransform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Spawn bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, targetRotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = targetDirection * bulletSpeed;
            rb.useGravity = false;

            // Inject damage into bullet here
            DamageApplicator dmg = bullet.GetComponent<DamageApplicator>();
            if (dmg != null)
            {
                int totalDamage = playerStats.GetAttackDamage(WeaponType.Wand); // or Gun/Sword depending on weapon
                dmg.currentWeaponDamage = totalDamage;

                // also set who it’s targeting
                dmg.objectType = ObjectType.Enemy;
            }
        }

        public void CastSpellOne()
        {
            // Get the direction the camera is facing
            Vector3 targetDirection = Camera.main.transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            var bird = Instantiate(spellOnePrefab, bulletSpawnPoint.position, targetRotation);
            bird.GetComponent<Rigidbody>().velocity = targetDirection * birdSpeed;
            bird.GetComponent<Rigidbody>().useGravity = false; // Disable gravity for the bird
        }

        // Function to cast a spell under the nearest enemy
        public void CastSpellTwo()
        {
            nearestEnemy = NearestEnemy();

            Vector3 spellSpawnPosition;

            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= maxDistance)
            {
                // Calculate the position where you want the spell to be spawned under the nearest enemy
                spellSpawnPosition = nearestEnemy.transform.position;
                spellSpawnPosition.y = 0f; // Set the y-coordinate to 0 or any other value you desire
            }
            else
            {
                // Calculate the position where you want the spell to be spawned at the max distance
                spellSpawnPosition = transform.position + Camera.main.transform.forward * maxDistance;
                spellSpawnPosition.y = 0f; // Set the y-coordinate to 0 or any other value you desire
            }

            // Spawn the spell prefab at the calculated position
            Instantiate(spellTwoPrefab, spellSpawnPosition, Quaternion.identity);
        }

        public void CastSpellThree()
        {
            nearestEnemy = NearestEnemy();

            Vector3 spellSpawnPosition;

            if (nearestEnemy != null && Vector3.Distance(transform.position, nearestEnemy.transform.position) <= maxDistance)
            {
                // Calculate the position where you want the spell to be spawned under the nearest enemy
                spellSpawnPosition = nearestEnemy.transform.position;
                spellSpawnPosition.y = 0f; // Set the y-coordinate to 0 or any other value you desire
            }
            else
            {
                // Calculate the position where you want the spell to be spawned at the max distance
                spellSpawnPosition = transform.position + Camera.main.transform.forward * maxDistance;
                spellSpawnPosition.y = 0f; // Set the y-coordinate to 0 or any other value you desire
            }

            // Spawn the spell prefab at the calculated position
            Instantiate(spellThreePrefab, spellSpawnPosition, Quaternion.identity);
        }

        public void StartShooting()
        {
            StartCoroutine(ShootBulletsWithDelay());
        }

        private IEnumerator ShootBulletsWithDelay()
        {
            nearestEnemy = NearestEnemy();

            List<GameObject> bullets = new List<GameObject>();
            foreach (Transform spawnPoint in spawnPoints)
            {
                var bullet = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                bullet.GetComponent<Rigidbody>().velocity = Vector3.zero; // Set the velocity to zero to keep the bullet in place
                bullet.GetComponent<Rigidbody>().useGravity = false; // Disable gravity for the bullet
                bullets.Add(bullet);
            }

            // Wait for an additional 2 seconds after spawning the bullets
            yield return new WaitForSeconds(2f);

            // After the additional 2 seconds, shoot the bullets towards the target
            for (int i = 0; i < bullets.Count; i++)
            {
                Vector3 targetDirection;

                if (nearestEnemy != null && Vector3.Distance(bullets[i].transform.position, nearestEnemy.transform.position) <= maxDistance)
                {
                    targetDirection = (nearestEnemy.transform.position - bullets[i].transform.position).normalized;
                }
                else
                {
                    // Get the direction the camera is facing
                    targetDirection = Camera.main.transform.forward;
                }

                bullets[i].GetComponent<Rigidbody>().velocity = targetDirection * bulletSpeed;
            }
        }

        GameObject NearestEnemy()
        {
            GameObject nearestHere = null;
            float leastDistance = Mathf.Infinity;

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var enemy = enemies[i];

                if (enemy != gameObject)
                {
                    float distanceHere = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distanceHere < leastDistance)
                    {
                        leastDistance = distanceHere;
                        nearestHere = enemy;
                    }
                }
            }
            return nearestHere;
        }
    }
}