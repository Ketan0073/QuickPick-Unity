using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    
    void Start()
    {
        SpawnGrid();
    }
    
    public void SpawnGrid()
    {
        // Clear existing tiles
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        
        // Create 3x3 grid
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3 pos = new Vector3(x * 140 - 140, -y * 140 + 140, 0);
                GameObject tile = Instantiate(tilePrefab, transform);
                RectTransform rt = tile.GetComponent<RectTransform>();
                rt.anchoredPosition = pos;
            }
        }
    }
}

