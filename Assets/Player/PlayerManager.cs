using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public AnimatorHandler animatorHandler;
        [HideInInspector] public PlayerLocomotion playerLocomotion;

        PlayerInputManager inputManager;

        [Header("Player Flags\n")]
        public bool isInteracting;
        public bool canDoCombo;
        public bool isAttacking;
        public bool isFalling;
        public bool isGrounded;
        public bool isAnimationPlaying;

        private void Awake()
        {
            base.Awake();

            playerLocomotion = GetComponent<PlayerLocomotion>();
            animator = GetComponentInChildren<Animator>();
            animatorHandler = GetComponent<AnimatorHandler>();
            inputManager = GetComponent<PlayerInputManager>();
            characterController = GetComponent<CharacterController>();

        }

        private void Update()
        {
            float delta = Time.deltaTime;
            inputManager._isInteracting = animator.GetBool("isInteracting");
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isFalling = animator.GetBool("isFalling");
            isGrounded = animator.GetBool("isGrounded");
            isAnimationPlaying = animator.GetBool("isAnimationPlaying");
        }

        private void LateUpdate()
        {
            inputManager._isJumping = false;
            inputManager._attackInput = false;
            inputManager._dashInput = false;
            inputManager._cycleWeaponInput = false;
            inputManager._specialInput = false;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            DamageApplicator damage = GetComponent<DamageApplicator>();

            if (hit.collider.attachedRigidbody != null)
            {
                Debug.Log("Hit rigidbody: " + hit.collider.name);
                // You can call a function on the rigidbody’s script here
                hit.collider.GetComponent<PlayerStats>()?.PlayerTakeDamage(damage.currentWeaponDamage, damage.elementalType);
            }
        }
    }
}
