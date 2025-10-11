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

        [Header("Inputs")]
        bool _jumpInput;
        [HideInInspector] public bool _dashInput;
        [HideInInspector] public bool _attackInput;
        [HideInInspector] public bool _cycleWeaponInput;
        [HideInInspector] public bool _specialInput;

        [Header("Flags")]
        public bool _isJumping;
        public bool _isDashing;
        public bool _combo;
        public bool _specialAttack;
        public bool _isInteracting;
        public bool _actionTriggered;

        Vector2 movementInput;
        Vector2 cameraInput;

        PlayerControls inputActions;
        PlayerManager playerManager;
        PlayerAttack playerAttack;
        PlayerStats playerStats;
        WeaponInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            playerAttack = GetComponent<PlayerAttack>();
            playerStats = GetComponent<PlayerStats>();
            playerInventory = GetComponent<WeaponInventory>();
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
                inputActions.PlayerActions.CycleWeapon.performed += i => _cycleWeaponInput = true;
                inputActions.PlayerActions.Special.performed += i => _specialInput = true;
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
            HandleSpecialInput(delta);
            HandleDashInput(delta);
            HandleCycleWeaponInput();
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

        public void HandleSpecialInput(float delta)
        {
            inputActions.PlayerActions.Special.performed += i => _specialInput = true;

            if (_specialInput)
            {
                if (playerManager.canDoCombo)
                {
                    _combo = true;
                    playerAttack.HandleSpecialAttack(playerInventory.rightWeapon, playerInventory.rightWeapon.WeaponType);
                    _combo = false;
                }
                else
                {
                    if (playerManager.isInteracting)
                        return;

                    if (playerManager.canDoCombo)
                        return;

                    playerAttack.HandleSpecialAttack(playerInventory.rightWeapon, playerInventory.rightWeapon.WeaponType);
                }
            }
        }

        public void HandleCycleWeaponInput()
        {
            if (!playerStats.swordUnlocked)
                return;

            inputActions.PlayerActions.CycleWeapon.performed += i => _cycleWeaponInput = true;

            if (_cycleWeaponInput)
            {
                playerInventory.ChangeRightWeapon();
            }
        }
    }
}
