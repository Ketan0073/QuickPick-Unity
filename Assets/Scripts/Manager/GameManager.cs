using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour

{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Button stopButton;
    
    private int score = 0;
    private int lives = 3;
    private Tile[] allTiles; //Track all 9 Tiles
    
    void Start()
    {
        stopButton.onClick.AddListener(OnStopButton);
        UpdateUI();
        SpawnInitialGrid();
    }
    
    public void OnTileClicked()  // Public method for Tile
    {
        score += 100;
        UpdateUI();
        Debug.Log("Score: " + score);
    }
    
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }
    
    void OnStopButton()
    {
        Debug.Log("STOP! Final Score: " + score);
    }

    void SpawnInitialGrid()
    {
        GridSpawner spawner = FindAnyObjectByType<GridSpawner>();
        // Wait one frame for tiles to spawn
        Invoke(nameof(StartFirstRound), 0.1f);
    }
    
    void StartFirstRound()
    {
        allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        CreateNewRound();
    }
    
    void CreateNewRound()
    {
        // Pick random odd tile position
        int oddIndex = Random.Range(0, allTiles.Length);
        Tile oddTile = allTiles[oddIndex];
        
        // Set ONE odd tile (red), others normal (blue)
        foreach (Tile tile in allTiles)
        {
            tile.SetOddTile(tile == oddTile);
        }
        
        Debug.Log($"Round started! Odd tile at index {oddIndex}");
    }
    
    public void OnTileClicked(Tile clickedTile)
    {
        if (clickedTile.isOddTile)
        {
            // CORRECT!
            score += 100;
            Debug.Log("CORRECT! +100 points");
            UpdateUI();
            Invoke(nameof(CreateNewRound), 1f);  // Next round
        }
        else
        {
            // WRONG!
            lives--;
            Debug.Log("WRONG! Lives left: " + lives);
            UpdateUI();

           

            // ✅ FIX 2: STOP game completely at 0 lives
            if (lives <= 0)
            {
                CancelInvoke();  // Cancel ALL pending invokes
                OnStopButton();
                return;  // Exit method immediately
            }
        
            // New round after wrong click (1 second delay)
            Invoke(nameof(CreateNewRound), 1f);
        }
    }
}

