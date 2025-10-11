using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PRJCTA.HOLLOWECHOES
{
    public class ChangeSceneOnTriggerEnter : MonoBehaviour
    {
        [SerializeField] private string sceneName; // Put the name of the scene you want to load
        [SerializeField] private string playerTag = "Player"; // Make sure your player has this tag

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                Debug.Log("Player entered trigger. Loading scene: " + sceneName);
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
