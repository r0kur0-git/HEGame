using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Player Settings")]
        public GameObject playerPrefab;
        public Transform spawnPoint; // assign in inspector

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayerStats existingPlayer = FindObjectOfType<PlayerStats>();

            if (existingPlayer == null)
            {
                SpawnPlayer();
            }
            else
            {
                if (spawnPoint != null)
                {
                    existingPlayer.transform.position = spawnPoint.position;
                    existingPlayer.transform.rotation = spawnPoint.rotation;
                }
            }
        }

        private void SpawnPlayer()
        {
            if (playerPrefab == null || spawnPoint == null)
            {
                Debug.LogError("PlayerSpawner: Prefab or SpawnPoint missing!");
                return;
            }

            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

            // Restore player data
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
                stats.LoadFromPlayerData();

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
                inventory.LoadInventoryFromPlayerData();

            WeaponInventory weaponInventory = player.GetComponent<WeaponInventory>();
            if (weaponInventory != null)
                weaponInventory.LoadFromPlayerData();

            PlayerAnimatorManager animatorManager = player.GetComponent<PlayerAnimatorManager>();
            if (animatorManager != null)
                animatorManager.LoadFromPlayerData();

            DontDestroyOnLoad(player);
        }
    }
}
