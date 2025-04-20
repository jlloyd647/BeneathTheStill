using System;
using System.Collections;
using UnityEngine;

public class TackStripPlacer : MonoBehaviour
{
    public GameObject tackStripPreviewPrefab;
    public GameObject tackStripRealPrefab;
    public InteractBarController interactBarPrefab;

    private GameObject activePreview;
    private InteractBarController activeBar;
    private bool isPlacing = false;

    private Inventory inventory;
    private PlayerController playerController;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && inventory.HasItem("Hammer"))
        {
            isPlacing = !isPlacing;

            if (isPlacing)
                StartPlacement();
            else
                EndPlacement();
        }

        if (isPlacing)
        {
            UpdatePreview();

            if (Input.GetKeyDown(KeyCode.E) && activeBar == null)
            {
                StartCoroutine(PlaceTackStripRoutine());
            }
        }
    }

    private void StartPlacement()
    {
        Debug.Log("🔨 Placement mode activated");
        if (tackStripPreviewPrefab != null)
        {
            activePreview = Instantiate(tackStripPreviewPrefab);
            SetPreviewOpacity(0.5f);
        }
    }

    private void EndPlacement()
    {
        Debug.Log("❌ Placement mode exited");
        if (activePreview != null)
            Destroy(activePreview);
        activePreview = null;
        isPlacing = false;
    }

    private void UpdatePreview()
    {
        Vector3 playerPos = transform.position;
        Vector2Int tilePos = new Vector2Int(
            Mathf.FloorToInt(playerPos.x),
            Mathf.FloorToInt(playerPos.y)
        );
        Vector3 tileCorner = new Vector3(tilePos.x, tilePos.y, 0f);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input != Vector2.zero)
        {
            RotatePreviewToFace(input);
        }

        if (activePreview != null)
        {
            // All directions currently offset the same amount
            Vector3 offset = new Vector3(0.5f, 0f, 0f);
            activePreview.transform.position = tileCorner + offset;
        }
    }

    private void RotatePreviewToFace(Vector2 dir)
    {
        if (dir == Vector2.right)
            activePreview.transform.rotation = Quaternion.Euler(0, 0, 0);       // Right
        else if (dir == Vector2.up)
            activePreview.transform.rotation = Quaternion.Euler(0, 0, 90);      // Up
        else if (dir == Vector2.left)
            activePreview.transform.rotation = Quaternion.Euler(0, 0, 180);     // Left
        else if (dir == Vector2.down)
            activePreview.transform.rotation = Quaternion.Euler(0, 0, 270);     // Down
    }

    private void SetPreviewOpacity(float alpha)
    {
        var sr = activePreview?.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            var color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

    private IEnumerator PlaceTackStripRoutine()
    {
        // 1. Freeze the player
        playerController.Freeze();

        // 2. Show interact bar
        Vector3 barPosition = transform.position + Vector3.up * 1.2f;
        activeBar = Instantiate(interactBarPrefab, barPosition, Quaternion.identity, transform);
        activeBar.destroyOnFinish = false;

        bool isDone = false;
        activeBar.Show(transform, 1.5f, () => isDone = true);

        yield return new WaitUntil(() => isDone);

        // 3. Spawn the tack strip exactly where the preview is
        PlaceTackStrip();

        // 4. Clean up
        Destroy(activeBar.gameObject);
        Destroy(activePreview);
        activeBar = null;
        activePreview = null;

        // 5. Exit placement mode
        isPlacing = false;
        playerController.Unfreeze();
    }

    private void PlaceTackStrip()
    {
        Vector3 position = activePreview.transform.position;
        Quaternion rotation = activePreview.transform.rotation;

        Instantiate(tackStripRealPrefab, position, rotation);
        Debug.Log($"✅ Placed tack strip at {position} with rotation {rotation.eulerAngles.z}°");
    }
}
