//by EvolveGames

using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UniRx;
using UnityEngine;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("PlayerController")]
        [SerializeField] public Transform Camera;

        [SerializeField] public ItemChange Items;
        [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
        [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
        [SerializeField, Range(2, 20)] float RuningSpeed = 4.0f;
        [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
        [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;

        [Space(20)]
        [Header("Advance")]
        [SerializeField] float RunningFOV = 65.0f;
        [SerializeField] float SpeedToFOV = 4.0f;
        [SerializeField] float CroughHeight = 1.0f;
        [SerializeField] public float gravity = 20.0f;
        [SerializeField] float timeToRunning = 2.0f;

        [HideInInspector] public bool canMove = true;
        [HideInInspector] public bool CanRunning = true;

        [Space(20)]
        [Header("Climbing")]
        [SerializeField] bool CanClimbing = true;
        [SerializeField, Range(1, 25)] float Speed = 2f;
        bool isClimbing = false;

        [Space(20)]
        [Header("HandsHide")]
        [SerializeField] bool CanHideDistanceWall = true;
        [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
        [SerializeField] int LayerMaskInt = 1;

        [Space(20)]
        [Header("Input")]
        [SerializeField] KeyCode CroughKey = KeyCode.LeftControl;
        [SerializeField] KeyCode VaultKey = KeyCode.E; // клавиша для залезания

        public float SensetivityMultiplier = 1f;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Vector3 moveDirection = Vector3.zero;
        public ReactiveProperty<bool> isCrough = new ReactiveProperty<bool>();

        float InstallCroughHeight;
        float rotationX = 0;
        [HideInInspector] public bool isRunning = false;
        Vector3 InstallCameraMovement;
        public float InstallFOV;
        Camera cam;

        [HideInInspector] public ReactiveProperty<bool> Moving = new ReactiveProperty<bool>();

        [HideInInspector] public float vertical;
        [HideInInspector] public float horizontal;
        [HideInInspector] public float Lookvertical;
        [HideInInspector] public float Lookhorizontal;

        [Header("Impulse")]
        [SerializeField] private float impulseDamping = 8f;
        private Vector3 impulseVelocity = Vector3.zero;

        float RunningValue;
        float installGravity;
        bool WallDistance;
        [HideInInspector] public float WalkingValue;

        [SerializeField] private PlayerCharacter _character;

        public bool HelpCrouching;
        public bool Crouching;
        public float DefaultFOV;

        // 🔹 TOGGLE ПРИСЕД
        private bool crouchToggle = false;

        // 🔹 LEDGE / VAULT
        [SerializeField] private float ledgeCheckDistance = 1f;
        [SerializeField] private float ledgeCheckHeight = 1.5f;
        [SerializeField] private float vaultSpeed = 5f;
        private bool isVaulting = false;

        public override void OnStartClient()
        {
            if (!IsOwner) return;

            characterController = GetComponent<CharacterController>();
            if (Items == null && GetComponent<ItemChange>()) Items = GetComponent<ItemChange>();
            cam = GetComponentInChildren<Camera>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            InstallCroughHeight = characterController.height;
            InstallCameraMovement = Camera.localPosition;
            InstallFOV = cam.fieldOfView;

            RunningValue = RuningSpeed;
            RunningFOV = cam.fieldOfView + 10;
            DefaultFOV = InstallFOV;
            installGravity = gravity;
            WalkingValue = walkingSpeed;
        }

        private void Update()
        {
            if (!IsOwner) return;

            RaycastHit CroughCheck;
            RaycastHit ObjectCheck;

            if (characterController.enabled && !characterController.isGrounded && !isClimbing && !isVaulting)
                moveDirection.y -= gravity * Time.deltaTime;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            isRunning = !isCrough.Value ? (CanRunning ? Input.GetKey(KeyCode.LeftShift) : false) : false;

            horizontal = _character.Binds.Character.Horizontal.ReadValue<float>();
            vertical = _character.Binds.Character.Vertical.ReadValue<float>();

            Vector3 direction = (forward * vertical) + (right * horizontal);
            if (direction.magnitude > 1f) direction.Normalize();

            float currentSpeed = isRunning ? RunningValue : WalkingValue;

            if (isRunning)
                RunningValue = Mathf.Lerp(RunningValue, RuningSpeed, timeToRunning * Time.deltaTime);
            else
                RunningValue = WalkingValue;

            float movementDirectionY = moveDirection.y;

            // 🔹 Логика движения
            moveDirection = direction * currentSpeed;
            moveDirection.y = movementDirectionY;

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isClimbing && !isVaulting)
                moveDirection.y = jumpSpeed;

            moveDirection += impulseVelocity;
            impulseVelocity *= Mathf.Exp(-impulseDamping * Time.deltaTime);

            characterController.Move(moveDirection * Time.deltaTime);

            Moving.Value = direction.magnitude > 0.1f;

            // 🔹 ПОВОРОТ КАМЕРЫ
            if (Cursor.lockState == CursorLockMode.Locked && canMove)
            {
                Lookvertical = -Input.GetAxis("Mouse Y") * SensetivityMultiplier;
                Lookhorizontal = Input.GetAxis("Mouse X") * SensetivityMultiplier;

                rotationX += Lookvertical * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

                cam.fieldOfView = Mathf.Lerp(
                    cam.fieldOfView,
                    isRunning && Moving.Value ? RunningFOV : InstallFOV,
                    SpeedToFOV * Time.deltaTime
                );
            }

            // 🔹 НАЖАТИЕ КНОПКИ ПРИСЕДА (TOGGLE)
            if (Input.GetKeyDown(CroughKey))
                crouchToggle = !crouchToggle;

            // 🔹 ЛОГИКА ПРИСЕДА
            if (crouchToggle || Crouching)
            {
                isCrough.Value = true;
                characterController.height = Mathf.Lerp(characterController.height, CroughHeight, 6 * Time.deltaTime);
                WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);
            }
            else
            {
                if (!Physics.Raycast(
                        GetComponentInChildren<Camera>().transform.position,
                        transform.TransformDirection(Vector3.up),
                        out CroughCheck,
                        0.8f,
                        1))
                {
                    isCrough.Value = false;
                    characterController.height = Mathf.Lerp(characterController.height, InstallCroughHeight, 6 * Time.deltaTime);
                    WalkingValue = Mathf.Lerp(WalkingValue, walkingSpeed, 4 * Time.deltaTime);
                }
            }

            // 🔹 СКРЫТИЕ РУК У СТЕН
            if (WallDistance != Physics.Raycast(
                    GetComponentInChildren<Camera>().transform.position,
                    transform.TransformDirection(Vector3.forward),
                    out ObjectCheck,
                    HideDistance,
                    LayerMaskInt) && CanHideDistanceWall)
            {
                WallDistance = Physics.Raycast(
                    GetComponentInChildren<Camera>().transform.position,
                    transform.TransformDirection(Vector3.forward),
                    out ObjectCheck,
                    HideDistance,
                    LayerMaskInt);

                Items.ani.SetBool("Hide", WallDistance);
                Items.DefiniteHide = WallDistance;
            }

            // 🔹 VAULT / LEDGE GRAB
            if (!isVaulting && Input.GetKeyDown(VaultKey))
            {
                TryVault();
            }
        }

        private void TryVault()
        {
            RaycastHit hitForward;
            Vector3 origin = transform.position + Vector3.up * 0.5f;

            if (Physics.Raycast(origin, transform.forward, out hitForward, ledgeCheckDistance))
            {
                Vector3 topPos = hitForward.point + Vector3.up * ledgeCheckHeight;

                if (!Physics.CheckBox(topPos, new Vector3(0.5f, 1f, 0.5f)))
                {
                    StartCoroutine(VaultToPosition(topPos));
                }
            }
        }

        private IEnumerator VaultToPosition(Vector3 targetPos)
        {
            isVaulting = true;
            characterController.enabled = false;

            Vector3 startPos = transform.position;
            float elapsed = 0f;
            float duration = Vector3.Distance(startPos, targetPos) / vaultSpeed;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            characterController.enabled = true;
            isVaulting = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsOwner) return;

            if (other.CompareTag("Ladder") && CanClimbing)
            {
                CanRunning = false;
                isClimbing = true;
                WalkingValue /= 2;
                Items.Hide(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsOwner) return;

            if (other.CompareTag("Ladder") && CanClimbing)
            {
                CanRunning = true;
                isClimbing = false;
                WalkingValue *= 2;
                Items.ani.SetBool("Hide", false);
                Items.Hide(false);
            }
        }

        public void AddImpulse(Vector3 direction, float force)
        {
            if (!IsOwner) return;
            direction.y = 0f;
            impulseVelocity += direction.normalized * force;
        }

        public void Crouch()
        {
            Crouching = true;
            isCrough.Value = true;
            characterController.height = CroughHeight;
            WalkingValue = CroughSpeed;
            StartCoroutine(RecoverCrouch());
        }

        private IEnumerator RecoverCrouch()
        {
            yield return new WaitForSeconds(0.2f);
            Crouching = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!IsOwner) return;

            if (other.CompareTag("Ladder") && CanClimbing)
            {
                moveDirection = new Vector3(
                    0,
                    Input.GetAxis("Vertical") * Speed * (-Camera.localRotation.x / 1.7f),
                    0
                );
            }
        }
    }
}
