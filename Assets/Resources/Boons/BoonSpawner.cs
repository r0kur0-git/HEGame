using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PRJCTA.HOLLOWECHOES
{
    public class BoonSpawner : MonoBehaviour
    {
        public BoonManager boonManager; // Drag your BoonManager here
        public int numberOfBoonsToShow = 3;

        private void OnTriggerEnter(Collider other)
        {

            // Check if the player entered
            if (other.CompareTag("Player"))
            {

                // Show boon selection
                boonManager.ShowRandomBoons(numberOfBoonsToShow);

                // Optional: disable the trigger object after activation
                // gameObject.SetActive(false);
            }
        }
    }
}
