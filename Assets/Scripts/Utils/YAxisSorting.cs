using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class YAxisSorting : MonoBehaviour
{
    private SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    void Update()
    {
        // Multiply by 100 to give enough sorting space
        sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}