using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffectMultiplier = 0.5f;
    public Vector2 initialOffset = Vector2.zero;

    private Vector3 lastCameraPosition;
    private bool initialized = false;

    void LateUpdate()
    {
        // If the cameraTransform isn't ready, try to grab it
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
                lastCameraPosition = cameraTransform.position;

                // Set initial position only once, based on offset
                transform.position = new Vector3(
                    cameraTransform.position.x + initialOffset.x,
                    cameraTransform.position.y + initialOffset.y,
                    transform.position.z
                );

                initialized = true;
            }
            return;
        }

        if (!initialized) return;

        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(
            deltaMovement.x * parallaxEffectMultiplier,
            deltaMovement.y * parallaxEffectMultiplier,
            0f
        );

        lastCameraPosition = cameraTransform.position;
    }
}
