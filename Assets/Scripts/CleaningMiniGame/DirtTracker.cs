using UnityEngine;

public class DirtTracker : MonoBehaviour
{
    public GameObject dirtPrefab;
    public float spawnDistance = 1.2f;

    private int remainingSpreads = 0;
    private Vector3 lastDropPosition;

    public void GetDirty(int numberOfSpreads)
    {
        remainingSpreads = numberOfSpreads;
        lastDropPosition = transform.position;
    }

    void Update()
    {
        if (remainingSpreads > 0)
        {
            float dist = Vector3.Distance(transform.position, lastDropPosition);
            if (dist >= spawnDistance)
            {
                SpawnDirt();
                remainingSpreads--;
                lastDropPosition = transform.position;
            }
        }
    }

    void SpawnDirt()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z = 0;
        Instantiate(dirtPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"{gameObject.name} left dirt behind!");
    }
}
