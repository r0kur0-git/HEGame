using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Vector3 moveDirection;
        PlayerInputManager inputManager;
        PlayerManager playerManager;
        Transform cameraObject;
        PlayerAnimatorManager playerAnimatorManager;
        Animator animator;
        public CharacterController characterController;

        [HideInInspector]
        public Transform myTransform;

        public float jumpForce = 5f;
        public float movementSpeed = 15;
        public float rotationSpeed = 15;
        public float gravity = -9.81f;
        public float attackLaunchForce = 5f;
        public int jumpCount = 0;
        public int maxJumps = 2;
        public int dashCount = 0;
        public int maxDashes = 0;
        public int airborneTime = 0;
        private bool _isOnCooldown;
        private Coroutine resetRoutine;
        [SerializeField] private float dashChainReset = 1f;
        [SerializeField] private float dashCooldown = 1.0f;
        private float lastDashTime = -Mathf.Infinity;
        private float cooldownEndTime = -Mathf.Infinity;

        [Header("Downward Force Settings")]
        public float _force = 100f;
        public float _offset;
        public float _radius;
        public LayerMask bridgeLayer;

        [Header("Ground Check Settings")]
        public bool _isFalling;
        public bool _isGrounded;
        public float groundCheckRadius = 0.3f;
        public float groundCheckDistance = 0.6f;
        public LayerMask groundLayer;

        public Vector3 velocity;

        void Start()
        {
            myTransform = transform;
            inputManager = GetComponent<PlayerInputManager>();
            cameraObject = Camera.main.transform;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
            playerAnimatorManager.Initialize();
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            inputManager.TickInput(delta);

            HandleMovement(delta);
            HandleRotation(delta);
            HandleGravityAndJump(delta);
            HandleDashing(delta);

            playerAnimatorManager.UpdateAnimatorValues(inputManager.moveAmount, 0);
        }

        private void FixedUpdate()
        {
            ApplyWeightToBridge();
        }

        #region Movement

        private void HandleMovement(float delta)
        {
            if (inputManager._isInteracting)
                return;

            if (!_isGrounded && playerAnimatorManager.isAnimationPlaying)
                return;

            moveDirection = cameraObject.forward * inputManager.vertical;
            moveDirection += cameraObject.right * inputManager.horizontal;
            moveDirection.Normalize();

            Vector3 groundNormal = Vector3.up;
            if (Physics.SphereCast(transform.position + Vector3.up * 0.2f, groundCheckRadius, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer))
                groundNormal = hit.normal;

            moveDirection *= movementSpeed;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);

            if (!_isGrounded && inputManager._actionTriggered)
                moveDirection *= 0.5f;

            if (_isGrounded)
                inputManager._actionTriggered = false;

            moveDirection.y = velocity.y;
            characterController.Move(moveDirection * delta);
        }

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;

            targetDir = cameraObject.forward * inputManager.vertical;
            targetDir += cameraObject.right * inputManager.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(targetDir);
            }
        }

        private void HandleGravityAndJump(float delta)
        {
            if (_isGrounded && playerAnimatorManager.isAnimationPlaying)
                return;

            _isGrounded = characterController.isGrounded;

            if (_isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
                jumpCount = 0;
            }

            if (inputManager._isJumping && jumpCount < maxJumps)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                inputManager._isJumping = false;
                jumpCount++;
            }

            if (playerAnimatorManager.isAnimationPlaying)
            {
                velocity.y = 0;
            }
            else
            {
                velocity.y += gravity * delta;
            }

            playerAnimatorManager.Grounded();
            playerAnimatorManager.Falling();

            characterController.Move(velocity * delta);
        }

        private void HandleGravitySuspension(float delta)
        {
            velocity.y = 0;
        }

        private void HandleDashing(float delta)
        {
            if (_isOnCooldown && Time.time >= cooldownEndTime)
            {
                dashCount = 0;
                _isOnCooldown = false;
            }

            if (!_isOnCooldown && dashCount > 0 && Time.time >= lastDashTime + dashChainReset)
            {
                dashCount = 0;
            }

            if (inputManager._isDashing)
            {
                if (!_isOnCooldown && dashCount < maxDashes)
                {
                    playerAnimatorManager.PlayTargetAnimation("Dash", true);
                    dashCount++;
                    lastDashTime = Time.time;

                    if (dashCount >= maxDashes)
                    {
                        _isOnCooldown = true;
                        cooldownEndTime = Time.time + dashCooldown;
                    }
                }

                inputManager._isDashing = false;
            }
        }

        void ApplyWeightToBridge()
        {
            var cols = Physics.OverlapSphere(transform.position - new Vector3(0, _offset, 0), _radius, bridgeLayer);

            foreach (var col in cols)
            {
                if (col.attachedRigidbody != null)
                {
                    // Simulate player weight on the bridge
                    col.attachedRigidbody.AddForceAtPosition(
                        Vector3.down * _force,
                        transform.position,
                        ForceMode.Force
                    );
                }
            }
        }

        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Vector3 origin = transform.position + Vector3.up * 0.2f;
            Gizmos.DrawWireSphere(origin + Vector3.down * groundCheckDistance, groundCheckRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * -_offset), _radius);
            Gizmos.DrawRay(transform.position, -transform.up);
        }
    }
}
