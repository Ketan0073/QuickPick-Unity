using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    
    void Start()
    {
        SpawnGrid();
    }
    
    void SpawnGrid()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                RectTransform rt = tile.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(x * 140, -y * 140);
            }
        }
    }
}

