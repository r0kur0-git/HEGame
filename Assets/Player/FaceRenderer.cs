using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class FaceRenderer : MonoBehaviour
    {
        [SerializeField] private Renderer faceRenderer;

        [Header("Blink")]
        [SerializeField] private Texture openEyes;
        [SerializeField] private Texture closedEyes;

        [SerializeField] private float blinkInterval = 3f;
        [SerializeField] private float blinkDuration = 0.1f;

        private float timer;

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= blinkInterval)
            {
                StartCoroutine(Blink());
                timer = 0f;
            }
        }

        private System.Collections.IEnumerator Blink()
        {
            faceRenderer.material.mainTexture = closedEyes;
            yield return new WaitForSeconds(blinkDuration);

            faceRenderer.material.mainTexture = openEyes;
        }
    }
}
