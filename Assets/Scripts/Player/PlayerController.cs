using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 3f;
    private float currentSpeed;
    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerClimbController climbController;

    private bool isFrozen = false; // 🆕 Added

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = baseSpeed;
        climbController = GetComponent<PlayerClimbController>();
    }

    void Update()
    {
        if (isFrozen) return; // 🆕 Skip input if frozen

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<PlayerStats>()?.UseStamina(10);
        }

        climbController?.HandleJumpInput(movement.x);
    }

    void FixedUpdate()
    {
        if (isFrozen) return; // 🆕 Skip movement if frozen

        Vector2 nextPosition = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        nextPosition = climbController != null
            ? climbController.ApplyClimbingConstraints(nextPosition)
            : nextPosition;

        rb.MovePosition(nextPosition);
    }

    public void SetSpeedModifier(float multiplier)
    {
        currentSpeed = baseSpeed * multiplier;
    }

    public void ResetSpeed()
    {
        currentSpeed = baseSpeed;
    }

    // 🆕 External control methods
    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;
}
