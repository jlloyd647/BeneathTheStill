using UnityEngine;

public class PlayerClimbController : MonoBehaviour
{
    private Transform mastAnchor;
    private Transform yardAnchor;
    private Transform yardExitLeft;
    private Transform yardExitRight;
    private Transform jumpTargetLeft;
    private Transform jumpTargetRight;
    private Transform returnToMastPoint = null;
    private Vector3 lastMastPosition;

    private bool isClimbingMast = false;
    private bool isOnYard = false;
    private bool canJumpToYard = false;

    public Vector2 ApplyClimbingConstraints(Vector2 nextPosition)
    {
        if (isClimbingMast && mastAnchor != null)
        {
            // Apply pull toward center of mast
            float pullStrength = 0.2f;
            float offset = mastAnchor.position.x - transform.position.x;
            nextPosition.x += offset * pullStrength;

            // Clamp sway after applying pull
            float maxSway = 0.4f;
            float swayOffset = nextPosition.x - mastAnchor.position.x;
            float clampedOffset = Mathf.Clamp(swayOffset, -maxSway, maxSway);
            nextPosition.x = mastAnchor.position.x + clampedOffset;
        }

        if (isOnYard && yardAnchor != null)
        {
            float maxSway = 0.1f;
            float swayOffset = nextPosition.y - yardAnchor.position.y;
            float clampedOffset = Mathf.Clamp(swayOffset, -maxSway, maxSway);
            nextPosition.y = yardAnchor.position.y + clampedOffset;

            // Clamp X to yard ends if defined
            if (yardExitLeft != null && yardExitRight != null)
            {
                float minX = yardExitLeft.position.x;
                float maxX = yardExitRight.position.x;
                nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
            }
        }

        return nextPosition;
    }

    public void HandleJumpInput(float horizontalInput)
    {
        bool jumpPressed = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space);

        if (canJumpToYard && jumpTargetLeft != null && jumpTargetRight != null && jumpPressed)
        {
            Transform target = horizontalInput < 0 ? jumpTargetLeft : horizontalInput > 0 ? jumpTargetRight : null;

            if (target != null)
            {
                lastMastPosition = transform.position;

                Vector3 newPos = target.position;
                newPos.z = transform.position.z;
                transform.position = newPos;

                isClimbingMast = false;
                isOnYard = true;
                yardAnchor = target;

                Debug.Log($"<color=cyan>Jumped to Yard: {target.name}</color>");
            }
        }

        // Return to mast from yard
        if (isOnYard && Input.GetKeyDown(KeyCode.Space))
        {
            float x = transform.position.x;
            float threshold = 0.3f;

            if (yardExitLeft != null && Mathf.Abs(x - yardExitLeft.position.x) < threshold)
            {
                Debug.Log("<color=cyan>Returning from left side of yard</color>");
                ReturnToMast();
                return;
            }

            if (yardExitRight != null && Mathf.Abs(x - yardExitRight.position.x) < threshold)
            {
                Debug.Log("<color=cyan>Returning from right side of yard</color>");
                ReturnToMast();
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MastClimbZone"))
        {
            isClimbingMast = true;
            mastAnchor = other.transform;
            Debug.Log("<color=green>Entered Mast Climb Zone</color>");
        }
        else if (other.CompareTag("YardClimbZone"))
        {
            isOnYard = true;
            yardAnchor = other.transform;

            yardExitLeft = other.transform.Find("YardExitPointLeft");
            yardExitRight = other.transform.Find("YardExitPointRight");
            returnToMastPoint = other.transform.Find("ReturnToMastPoint");
            if (returnToMastPoint != null)
            {
                Debug.Log($"<color=green>✅ Found ReturnToMastPoint: {returnToMastPoint.name}</color>");
            }
            else
            {
                Debug.LogWarning("⚠️ ReturnToMastPoint NOT found in YardClimbZone!");
            }

            Debug.Log($"<color=blue>Entered Yard Zone: {yardAnchor.name}</color>");
        }
        else if (other.CompareTag("YardJumpPoint"))
        {
            canJumpToYard = true;

            jumpTargetLeft = other.transform.Find("JumpTargetLeft");
            jumpTargetRight = other.transform.Find("JumpTargetRight");

            if (jumpTargetLeft == null || jumpTargetRight == null)
                Debug.LogWarning("YardJumpPoint is missing one or both jump targets.");
            else
                Debug.Log("<color=yellow>Entered Yard Jump Point</color>");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MastClimbZone"))
        {
            isClimbingMast = false;
            mastAnchor = null;
            Debug.Log("<color=red>Exited Mast Climb Zone</color>");
        }
        else if (other.CompareTag("YardClimbZone"))
        {
            isOnYard = false;
            yardAnchor = null;
            yardExitLeft = null;
            yardExitRight = null;
            Debug.Log("<color=orange>Exited Yard Zone</color>");
        }
        else if (other.CompareTag("YardJumpPoint"))
        {
            canJumpToYard = false;
            jumpTargetLeft = null;
            jumpTargetRight = null;
            Debug.Log("<color=orange>Exited Yard Jump Point</color>");
        }
    }

    private void ReturnToMast()
    {
        Vector3 newPos = lastMastPosition;
        newPos.z = transform.position.z;
        transform.position = newPos;

        isClimbingMast = true;
        isOnYard = false;
        yardAnchor = null;

        Debug.Log($"<color=green>Returned to Mast at saved position: {newPos}</color>");
    }
}
