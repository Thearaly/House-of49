using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;
    public float jumpHeight = 1.2f;
    public float gravity = -20f;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
            return;

        Move();
        ApplyGravityAndJump();
    }

    void Move()
    {
        Debug.Log("Move called: " + Input.GetAxisRaw("Vertical"));
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cameraTransform.right; right.y = 0f; right.Normalize();
        Vector3 dir = (forward * v + right * h).normalized;

        if (dir.magnitude > 0.1f)
        {
            controller.Move(dir * moveSpeed * Time.deltaTime);
            Quaternion target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
            animator.SetFloat("Speed", dir.magnitude);
    }

    void ApplyGravityAndJump()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}