using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilePositionCollector : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private bool isEnabled = false;
    public List<Vector3Int> collectedPositions = new List<Vector3Int>();

    void Update()
    {
        if (!isEnabled) return;

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            Vector3 mouseWorldPos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = targetTilemap.WorldToCell(mouseWorldPos);
            if (!collectedPositions.Contains(cellPos))
            {
                collectedPositions.Add(cellPos);
                Debug.Log($"🧭 Collected tile position: {cellPos}");
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("🧾 All collected positions:");
            foreach (var pos in collectedPositions)
            {
                Debug.Log(pos);
            }
        }
    }
}