using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    public float pullRange = 1.5f;
    public float pullSpeedModifier = 0.5f;
    public KeyCode pullKey = KeyCode.Space;

    private Rigidbody2D pulledCrate;
    private DriftMotion driftMotion;
    private Vector2 offsetFromPlayer;
    private PlayerController playerController;
    private bool isPulling = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pullKey))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pullRange);
            foreach (var hit in hits)
            {
                PullableObject pullable = hit.GetComponent<PullableObject>();
                if (pullable != null)
                {
                    pulledCrate = pullable.GetComponent<Rigidbody2D>();
                    driftMotion = pullable.GetComponent<DriftMotion>();

                    if (driftMotion != null)
                        driftMotion.isSecured = true;

                    if (pulledCrate != null)
                        pulledCrate.bodyType = RigidbodyType2D.Dynamic; // ✅ Make crate interactive

                    offsetFromPlayer = pulledCrate.position - (Vector2)transform.position;
                    isPulling = true;

                    playerController?.SetSpeedModifier(pullSpeedModifier);

                    // Lock movement axis based on pull direction
                    if (Mathf.Abs(offsetFromPlayer.x) > Mathf.Abs(offsetFromPlayer.y))
                    {
                        playerController?.SetMovementAxisConstraint(Vector2.right); // horizontal movement
                    }
                    else
                    {
                        playerController?.SetMovementAxisConstraint(Vector2.up); // vertical movement
                    }

                    break;
                }
            }
        }

        if (Input.GetKeyUp(pullKey) && isPulling)
        {
            if (driftMotion != null)
            {
                driftMotion.isSecured = false;
                driftMotion = null;
            }

            if (pulledCrate != null)
            {
                pulledCrate.bodyType = RigidbodyType2D.Kinematic; // ✅ Disable interaction when released
                pulledCrate = null;
            }

            isPulling = false;

            playerController?.ResetSpeed();
            playerController?.ClearMovementAxisConstraint();
        }
    }

    void FixedUpdate()
    {
        if (isPulling && pulledCrate != null)
        {
            pulledCrate.MovePosition((Vector2)transform.position + offsetFromPlayer);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}
