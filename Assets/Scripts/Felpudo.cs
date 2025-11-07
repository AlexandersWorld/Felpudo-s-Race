using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float backwardSpeedMultiplier = 0.6f; // slower when moving left
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeightScale = 0.5f;
    [SerializeField] private KeyCode crouchKey = KeyCode.S;

    [Header("Scene Bounds")]
    [SerializeField] private float leftBound = -10f;
    [SerializeField] private float rightBound = 10f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;

    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    private bool isGrounded;
    private bool isCrouching;

    [Header("Health Settings")]
    [SerializeField] private float currentHealth = 0;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth((int)maxHealth);

        originalColliderSize = col.bounds.size;
        originalColliderOffset = col.offset;
    }

    void Update()
    {
        HandleMovement();
        HandleJumpAndCrouch();
        ClampPositionToBounds();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // -1 = A, +1 = D
        float speed = moveSpeed;

        // backward movement slower
        if (moveInput < 0)
            speed *= backwardSpeedMultiplier;

        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * speed;
        rb.linearVelocity = velocity;
    }

    private void HandleJumpAndCrouch()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump state animation
        if (isGrounded)
        {
            animator.SetBool("Jump", false);
        }
        else
        {
            animator.SetBool("Jump", rb.linearVelocity.y > 0.1f);
        }

        // Jump input
        if (isGrounded && Input.GetButtonDown("Jump") && !isCrouching)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Crouch input
        if (Input.GetKeyDown(crouchKey))
        {
            Crouch(true);
            animator.SetBool("Crounch", true);
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            Crouch(false);
            animator.SetBool("Crounch", false);
        }
    }

    private void ClampPositionToBounds()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
        transform.position = pos;
    }

    private void Crouch(bool crouching)
    {
        if (crouching && !isCrouching)
        {
            isCrouching = true;
            ShrinkCollider();
        }
        else if (!crouching && isCrouching)
        {
            isCrouching = false;
            ResetCollider();
        }
    }

    private void ShrinkCollider()
    {
        if (col is BoxCollider2D box)
        {
            box.size = new Vector2(box.size.x, box.size.y * crouchHeightScale);
            box.offset = new Vector2(box.offset.x, box.offset.y - box.size.y * 0.25f);
        }
        else if (col is CapsuleCollider2D capsule)
        {
            capsule.size = new Vector2(capsule.size.x, capsule.size.y * crouchHeightScale);
            capsule.offset = new Vector2(capsule.offset.x, capsule.offset.y - capsule.size.y * 0.25f);
        }
    }

    private void ResetCollider()
    {
        if (col is BoxCollider2D box)
        {
            box.size = new Vector2(box.size.x, originalColliderSize.y);
            box.offset = originalColliderOffset;
        }
        else if (col is CapsuleCollider2D capsule)
        {
            capsule.size = new Vector2(capsule.size.x, originalColliderSize.y);
            capsule.offset = originalColliderOffset;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(leftBound, -10f, 0f), new Vector3(leftBound, 10f, 0f));
        Gizmos.DrawLine(new Vector3(rightBound, -10f, 0f), new Vector3(rightBound, 10f, 0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IExplodable explodable = collision.gameObject.GetComponent<IExplodable>();
        if (explodable != null)
        {
            explodable.Explode();

            currentHealth = Mathf.Clamp(explodable.Adjust(currentHealth), 0, maxHealth);

            healthBar.SetHealth((int)currentHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IExplodable explodable = collision.GetComponent<IExplodable>();
        if (explodable != null)
        {
            explodable.Explode();

            currentHealth = Mathf.Clamp(explodable.Adjust(currentHealth), 0, maxHealth);

            healthBar.SetHealth((int)currentHealth);
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
