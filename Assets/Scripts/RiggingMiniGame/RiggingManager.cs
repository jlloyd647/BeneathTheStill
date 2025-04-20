using System.Collections.Generic;
using UnityEngine;

public class RiggingManager : MonoBehaviour
{
    public static RiggingManager Instance;

    public LineRenderer linePrefab;
    private RiggingPoint selectedPoint;
    private List<LineRenderer> lines = new();
    private LineRenderer carryingLine;
    private GameObject playerRef;
    private bool isCarrying = false;

    [SerializeField] private RiggingPoint[] allRiggingPoints; // Drag all 8 in Inspector

    [SerializeField] private Color[] baseColors = new Color[]
    {
        new Color(192f/255f, 84f/255f, 84f/255f, 1f),   // Deep Coral
        new Color(88f/255f, 120f/255f, 153f/255f, 1f),  // Storm Blue
        new Color(110f/255f, 135f/255f, 103f/255f, 1f), // Moss Green
        new Color(193f/255f, 150f/255f, 86f/255f, 1f),  // Golden Ochre
    };

    void Start()
    {
        AssignRandomColorPairs();
    }

    private void AssignRandomColorPairs()
    {
        List<Color> colorPool = new();

        // Duplicate each color to make pairs
        foreach (var color in baseColors)
        {
            colorPool.Add(color);
            colorPool.Add(color);
        }

        // Shuffle the list
        for (int i = 0; i < colorPool.Count; i++)
        {
            Color temp = colorPool[i];
            int randomIndex = Random.Range(i, colorPool.Count);
            colorPool[i] = colorPool[randomIndex];
            colorPool[randomIndex] = temp;
        }

        // Assign to rigging points
        for (int i = 0; i < allRiggingPoints.Length; i++)
        {
            allRiggingPoints[i].SetColor(colorPool[i]);
        }
    }
    
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isCarrying && selectedPoint != null && carryingLine != null && playerRef != null)
        {
            carryingLine.SetPosition(0, selectedPoint.transform.position);
            carryingLine.SetPosition(1, playerRef.transform.position);
        }
    }

    public void OnRiggingSelected(RiggingPoint point)
    {
        if (selectedPoint == null)
        {
            selectedPoint = point;
            playerRef = GameObject.FindWithTag("Player");

            // Create a rope from selectedPoint to player
            carryingLine = Instantiate(linePrefab);
            carryingLine.positionCount = 2;
            isCarrying = true;
        }
        else
        {
            if (selectedPoint != point && selectedPoint.rigColor == point.rigColor)
            {
                // Complete the rope between the two points
                carryingLine.SetPosition(0, selectedPoint.transform.position);
                carryingLine.SetPosition(1, point.transform.position);
                lines.Add(carryingLine);

                selectedPoint.Tighten(50);
                point.Tighten(50);
            }
            else
            {
                Destroy(carryingLine.gameObject); // discard if not matched
            }

            selectedPoint = null;
            carryingLine = null;
            isCarrying = false;
        }
    }

    private void DrawLineBetween(Vector3 start, Vector3 end)
    {
        var line = Instantiate(linePrefab);
        line.positionCount = 2;
        line.SetPosition(0, start);
        line.SetPosition(1, end);
        lines.Add(line);
    }

    public void ResetRigging()
    {
        foreach (var line in lines)
            Destroy(line.gameObject);

        lines.Clear();
        selectedPoint = null;
    }
}
