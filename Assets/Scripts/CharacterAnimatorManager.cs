using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager characterManager;

        public virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            characterManager.animator.SetFloat("Horizontal", horizontalMovement);
            characterManager.animator.SetFloat("Vertical", verticalMovement);
        }
    }
}