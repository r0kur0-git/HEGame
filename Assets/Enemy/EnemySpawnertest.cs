using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemySpawnertest : MonoBehaviour
    {
        public GameObject objectPrefab;     // The object to spawn
        public Transform spawnPoint;        // Where to spawn it

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E)) // Change E to any key you like
            {
                SpawnObject();
            }
        }

        void SpawnObject()
        {
            Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
