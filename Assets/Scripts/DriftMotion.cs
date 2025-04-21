using UnityEngine;
using System.Collections;

public class DriftMotion : MonoBehaviour
{
    public float driftStrength = 0.05f;      // How much to move per wave tick
    public float driftFrequency = 5f;        // Time in seconds between drift attempts
    public bool isSecured = false;

    private float timer;
    private Rigidbody2D rb;
    private float randomOffset;
    private Vector2 directionBias;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        randomOffset = Random.Range(0f, 1000f);                           // Perlin noise offset
        directionBias = Random.insideUnitCircle.normalized * 0.2f;       // Small directional variation
    }

    private void Update()
    {
        if (isSecured) return;

        timer += Time.deltaTime;
        if (timer >= driftFrequency)
        {
            timer = 0f;
            TryDrift();
        }
    }

    void TryDrift()
    {
        Vector2 driftDirection = new Vector2(
            Mathf.PerlinNoise(Time.time + randomOffset, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time + randomOffset) - 0.5f
        ).normalized + directionBias;

        driftDirection.Normalize();
        Vector2 moveVector = driftDirection * driftStrength;

        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = rb.Cast(driftDirection, new ContactFilter2D(), hits, moveVector.magnitude);

        if (hitCount == 0)
        {
            StartCoroutine(SmoothDrift(moveVector, 0.5f));  // Drift over half a second
        }
    }

    IEnumerator SmoothDrift(Vector2 moveVector, float duration)
    {
        float elapsed = 0f;
        Vector2 start = rb.position;
        Vector2 end = start + moveVector;

        while (elapsed < duration)
        {
            if (isSecured) yield break; // 🔒 Stop mid-drift if secured

            float t = elapsed / duration;
            rb.MovePosition(Vector2.Lerp(start, end, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (!isSecured)
        {
            rb.MovePosition(end);
        }
    }
}
