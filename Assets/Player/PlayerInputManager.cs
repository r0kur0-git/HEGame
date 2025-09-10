using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public float moveAmount;
        [HideInInspector] public float vertical;
        public float horizontal;
        [HideInInspector] public float mouseX;
        [HideInInspector] public float mouseY;

        [Header("Inputs\n")]
        bool _jumpInput;
        public bool _dashInput;
        public bool _attackInput;

        [Header("Flags\n")]
        public bool _isJumping;
        public bool _isDashing;
        public bool _combo;
        public bool _isInteracting;
        public bool _actionTriggered;

        Vector2 movementInput;
        Vector2 cameraInput;

        PlayerControls inputActions;
        PlayerManager playerManager;
        PlayerAttack playerAttack;
        PlayerWeaponInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            playerInventory = GetComponent<PlayerWeaponInventory>();
            playerManager = GetComponentInChildren<PlayerManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();

                inputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                inputActions.PlayerMovement.Movement.canceled += i => movementInput = Vector2.zero;
                inputActions.PlayerMovement.Jump.performed += i => _jumpInput = true;
                inputActions.PlayerMovement.Dash.performed += i => _dashInput = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleJumpInput(delta);
            HandleAttackInput(delta);
            HandleDashInput(delta);
        }

        public void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal));
        }

        private void HandleJumpInput(float delta)
        {
            inputActions.PlayerMovement.Jump.performed += i => _jumpInput = true;

            if (_jumpInput)
            {
                _isJumping = true;
                _jumpInput = false;
            }
        }

        private void HandleDashInput(float delta)
        {
            inputActions.PlayerMovement.Dash.performed += i => _dashInput = true;

            if (_dashInput)
            {
                _isDashing = true;
            }
        }

        public void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.Basic.performed += i => _attackInput = true;

            if (_attackInput)
            {
                if (playerManager.canDoCombo)
                {
                    _combo = true;
                    playerAttack.HandleWeaponCombo(playerInventory.rightWeapon, playerInventory.rightWeapon.WeaponType);
                    _combo = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    playerAttack.HandleBasicAttack(playerInventory.rightWeapon, playerInventory.rightWeapon.WeaponType);
                }
            }
        }
    }
}
