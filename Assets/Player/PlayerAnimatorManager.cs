using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerAnimatorManager : AnimatorHandler
    {
        public RuntimeAnimatorController baseController;
        public AnimatorOverrideController swordController;
        public AnimatorOverrideController axeController;
        public PlayerLocomotion playerLocomotion;
        public PlayerInputManager inputManager;
        public int horizontal;
        public int vertical;
        public bool canRotate;
        public bool isAnimationPlaying = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            if (!inputManager._isInteracting) return;

            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0; // let gravity handle vertical
            playerLocomotion.characterController.Move(deltaPosition);
        }

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            inputManager = GetComponentInParent<PlayerInputManager>();
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            float snappedHorizontal;
            float snappedVertical;

            #region Snapped Horizontal
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }
            #endregion

            #region Snapped Vertical
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                snappedVertical = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }
            #endregion

            animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        public void IsInteractingBool()
        {
            animator.SetBool("isInteracting", false);
            Debug.Log("isinteracting false");
        }

        public void AnimationStarted()
        {
            isAnimationPlaying = true;
            animator.SetBool("isAnimationPlaying", true);
        }

        public void AnimationFinished()
        {
            isAnimationPlaying = false;
            animator.SetBool("isAnimationPlaying", false);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void Falling()
        {
            animator.SetBool("isFalling", !playerLocomotion._isGrounded && playerLocomotion.velocity.y < -0.1f);
        }

        public void Grounded()
        {
            animator.SetBool("isGrounded", playerLocomotion._isGrounded);
        }

        public void DashingTrue()
        {
            animator.SetBool("isDashing", inputManager._isDashing);
        }

        public void DashingFalse()
        {
            animator.SetBool("isDashing", inputManager._isDashing = false);
        }

        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public void SwitchToSword()
        {
            animator.runtimeAnimatorController = swordController;
        }

        public void SwitchToAxe()
        {
            animator.runtimeAnimatorController = axeController;
        }

        public void SwitchToDefault()
        {
            animator.runtimeAnimatorController = baseController;
        }
    }
}
