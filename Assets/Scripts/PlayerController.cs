
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float runSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    public Transform cameraTransform;
    public float rotationSpeed = 12f;

    [Header("Animation")]
    public Animator animator;
    public float animationSmoothTime = 0.1f;

    private CharacterController controller;
    private float verticalVelocity;
    private float currentAnimationBlend;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError(
                "PlayerController: Assign the Main Camera."
            );
            enabled = false;
            return;
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogWarning(
                "PlayerController: No Animator found in children."
            );
        }
    }

    void Update()
    {
        if (cameraTransform == null)
            return;

        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 direction =
            forward * vertical + right * horizontal;

        direction = Vector3.ClampMagnitude(direction, 1f);

        bool isRunning =
            Input.GetKey(KeyCode.LeftShift) &&
            direction.sqrMagnitude > 0.01f;

        float currentSpeed = isRunning ? runSpeed : moveSpeed;

        bool wasGrounded = controller.isGrounded;

        if (wasGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (wasGrounded &&
            Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement = direction * currentSpeed;
        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        UpdateAnimations(direction, isRunning);
    }

    void UpdateAnimations(Vector3 direction, bool isRunning)
    {
        if (animator == null)
            return;

        float targetBlend = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            targetBlend = isRunning ? 1f : 0.5f;
        }

        currentAnimationBlend = Mathf.Lerp(
            currentAnimationBlend,
            targetBlend,
            1f - Mathf.Exp(
                -Time.deltaTime / animationSmoothTime
            )
        );

        animator.SetFloat("Blend", currentAnimationBlend);

        bool isGrounded = controller.isGrounded;
        bool isJumping = !isGrounded && verticalVelocity > 0.1f;

        animator.SetBool("IsJumping", isJumping);
    }
}
