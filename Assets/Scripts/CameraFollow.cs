using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player
    public Vector2 deadZoneSize = new Vector2(1f, 2f); // Width/Height of dead zone
    public float followSpeed = 5f;

    private bool snapNextFrame = false;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 cameraPos = transform.position;
        Vector3 targetPos = target.position;

        if (snapNextFrame)
        {
            // Snap directly to target, bypassing dead zone and smoothing
            transform.position = new Vector3(targetPos.x, targetPos.y, cameraPos.z);
            snapNextFrame = false;
            return;
        }

        // Difference between player and camera
        Vector3 delta = targetPos - cameraPos;

        // Define the dead zone
        float dx = Mathf.Clamp(delta.x, -deadZoneSize.x, deadZoneSize.x);
        float dy = Mathf.Clamp(delta.y, -deadZoneSize.y, deadZoneSize.y);

        // Only follow if player moves *outside* dead zone
        Vector3 offset = new Vector3(delta.x - dx, delta.y - dy, 0f);

        transform.position = Vector3.Lerp(cameraPos, cameraPos + offset, Time.deltaTime * followSpeed);
    }

    public void SnapToTarget()
    {
        snapNextFrame = true;
    }
}
