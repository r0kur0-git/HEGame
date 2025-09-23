using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;
        public PlayerStats playerStats;
        public Transform enemyHead;
        public GameObject destroyOnDeath;
        //public new ParticleSystem particleSystem;
        public GameObject enemyLockOn;
        public GameObject spawnObject;

        private Coroutine fireDoTCoroutine;
        private Coroutine iceDoTCoroutine;
        private Coroutine electricDoTCoroutine;

        public static int enemyCount = 0;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthbar.SetMaxHealth(maxHealth);
            //particleSystem.Stop();
        }

        private void OnEnable()
        {
            FindCanvas();
            FindPlayer();
        }

        void FindCanvas() 
        { 
            canvas = FindObjectOfType<Canvas>(); 

            if (canvas != null) 
            { 
                enemyHealthbar = canvas.GetComponentInChildren<EnemyHealthBar>(true); 
            } 
        }

        void FindPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                playerStats = playerObj.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    Debug.Log("Found PlayerStats on: " + playerObj.name);
                }
                else
                {
                    Debug.LogWarning("Player found, but no PlayerStats component attached!");
                }
            }
            else
            {
                Debug.LogWarning("Player not found in scene!");
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void EnemyTakeDamage(int damage, ElementType elementalType)
        {
            currentHealth -= damage;
            ComboRankSystem.Instance.AddStylePoints(25);

            if (playerStats != null)
            {
                switch (ComboRankSystem.Instance.currentRank)
                {
                    case "D":
                        playerStats.GainMana(10);
                        break;
                    case "C":
                        playerStats.GainMana(5);
                        break;
                    case "B":
                        playerStats.GainMana(7);
                        break;
                    case "A":
                        playerStats.GainMana(10);
                        break;
                    case "S":
                        playerStats.GainMana(20);
                        break;
                }
            }

            // Show health bar only when damaged
            if (enemyHealthbar != null)
            {
                enemyHealthbar.SetTarget(enemyHead);
                enemyHealthbar.image.SetActive(true);
                enemyHealthbar.SetCurrentHealth(currentHealth);
            }

            animator.Play("Hit");

            if (currentHealth <= 0)
            {
                isDead = true;
                gameObject.tag = "Dead";
                enemyCount++;
                currentHealth = 0;

                if (isDead)
                {
                    animator.Play("Death");
                    StartCoroutine(DestroyEnemy());
                    GetComponent<EnemyManager>().enabled = false;

                    Destroy(destroyOnDeath);
                }

                print(enemyCount);
            }

            switch (elementalType)
            {
                case ElementType.None:
                    break;

                case ElementType.Fire:
                    if (playerStats != null && playerStats.hasFireDoT)
                    {
                        ApplyDamageOverTime(3, 1f, 5f, "");
                    }
                    print("burning");
                    break;

                case ElementType.Ice:
                    print("freezing");
                    break;

                case ElementType.Electric:
                    ApplyDamageOverTime(5, 1.5f, 5, "Hit");
                    print("shock");
                    break;

                default:
                    break;
            }
        }

        public void ApplyDamageOverTime(int damagePerTick, float tickInterval, float duration, string tickAnimation)
        {
            if (fireDoTCoroutine != null) StopCoroutine(fireDoTCoroutine);
            fireDoTCoroutine = StartCoroutine(DamageOverTime(damagePerTick, tickInterval, duration, tickAnimation));
        }

        private IEnumerator DamageOverTime(int damagePerTick, float tickInterval, float duration, string tickAnimation)
        {
            float elapsed = 0f;
            while (elapsed < duration && currentHealth > 0)
            {
                // Apply damage
                currentHealth -= damagePerTick;
                enemyHealthbar.SetCurrentHealth(currentHealth);
                Debug.Log($"Damage: {damagePerTick} (Remaining HP: {currentHealth})");

                if (!string.IsNullOrEmpty(tickAnimation))
                {
                    animator.Play(tickAnimation, 0, 0f);
                }

                // Play animation for each tick
                if (currentHealth <= 0)
                {
                    isDead = true;
                    gameObject.tag = "Dead";
                    enemyCount++;
                    currentHealth = 0;

                    if (isDead)
                    {
                        animator.Play("Death");
                        StartCoroutine(DestroyEnemy());
                        GetComponent<EnemyManager>().enabled = false;
                        //GetComponent<EnemyWeaponSlotManager>().enabled = false;
                        //particleSystem.Play();

                        Destroy(destroyOnDeath);
                    }

                    print(enemyCount);
                }

                // Wait until next tick
                elapsed += tickInterval;
                yield return new WaitForSeconds(tickInterval);
            }
        }

        IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(3.5f);
            Destroy(gameObject);
        }
    }
}