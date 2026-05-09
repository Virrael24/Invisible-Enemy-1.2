using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float velocity = 5f;
    public float sprintAdittion = 3.5f;
    public float rotationSpeed = 15f; // Скорость поворота персонажа

    [Header("Jump Settings")]
    public float jumpForce = 18f;
    public float jumpTime = 0.85f;
    public float gravity = 9.8f;

    [Header("Audio (New)")]
    [SerializeField] private AudioSource footstepSource;

    private float jumpElapsedTime = 0;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

    private float inputHorizontal;
    private float inputVertical;
    private bool inputJump;
    private bool inputCrouch;
    private bool inputSprint;

    private Animator animator;
    private CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("Animator component missing. Animations won't play.");
    }

    void Update()
    {
        // Сбор ввода
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputJump = Input.GetButtonDown("Jump");
        inputSprint = Input.GetKey(KeyCode.LeftShift);
        inputCrouch = Input.GetKeyDown(KeyCode.LeftControl);

        if (inputCrouch)
            isCrouching = !isCrouching;

        HandleVisualsAndAudio();

        if (inputJump && cc.isGrounded)
        {
            isJumping = true;
        }

        HeadHittingDetect();
    }

    private void HandleVisualsAndAudio()
    {
        float currentSpeed = cc.velocity.magnitude;
        bool isMoving = currentSpeed > 0.1f;

        if (animator != null)
        {
            // Анимации
            animator.SetBool("air", !cc.isGrounded);

            if (cc.isGrounded)
            {
                animator.SetBool("crouch", isCrouching);
                animator.SetBool("run", isMoving);

                isSprinting = isMoving && inputSprint && !isCrouching;
                animator.SetBool("sprint", isSprinting);
            }
        }

        // Звук шагов (проигрывается, если персонаж на земле и движется)
        if (footstepSource != null)
        {
            if (isMoving && cc.isGrounded && !footstepSource.isPlaying)
                footstepSource.Play();
            else if (!isMoving || !cc.isGrounded)
                footstepSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        float velocityAddition = 0;
        if (isSprinting) velocityAddition = sprintAdittion;
        if (isCrouching) velocityAddition = -(velocity * 0.5f);

        // ДВИЖЕНИЕ: Теперь мы используем мировые оси вместо Camera.main.transform
        // Vector3.forward (Z) — это "вверх" по экрану, Vector3.right (X) — "вправо"
        Vector3 moveDirection = new Vector3(inputHorizontal, 0, inputVertical).normalized;

        float directionX = moveDirection.x * (velocity + velocityAddition) * Time.deltaTime;
        float directionZ = moveDirection.z * (velocity + velocityAddition) * Time.deltaTime;
        float directionY = 0;

        // Прыжок
        if (isJumping)
        {
            directionY = Mathf.SmoothStep(jumpForce, jumpForce * 0.3f, jumpElapsedTime / jumpTime) * Time.deltaTime;
            jumpElapsedTime += Time.deltaTime;
            if (jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0;
            }
        }

        directionY -= gravity * Time.deltaTime;

        // ПОВОРОТ: Персонаж смотрит в сторону движения относительно мировых координат
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 finalMovement = new Vector3(directionX, directionY, directionZ);
        cc.Move(finalMovement);
    }

    void HeadHittingDetect()
    {
        float headHitDistance = 1.1f;
        Vector3 ccCenter = transform.TransformPoint(cc.center);
        float hitCalc = cc.height / 2f * headHitDistance;

        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
        {
            jumpElapsedTime = 0;
            isJumping = false;
        }
    }
}