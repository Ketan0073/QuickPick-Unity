using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour

{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public Button stopButton;

    
    [Header("Game State")]
    public int lives = 3;
    public int score = 0;
    public int roundsPlayed = 0;
    public int correctClicks = 0;
    public bool gameActive = true;


    private Tile[] allTiles; //Track all 9 Tiles
    private float roundStartTime;
    
    void Start()
    {
        stopButton.onClick.AddListener(ShowFinalStats);
        Invoke(nameof(InitializeGame), 0.1f);
    }

    void InitializeGame()
    {
        allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        CreateNewRound();
    }

    void Update()
    {
        if (!gameActive) return;

        // YOUR TIMEOUT: 5s → Next round (0 points)
        if (Time.time - roundStartTime >= 5f)
        {
            roundsPlayed++;
            CreateNewRound();
        }
    }

    public void OnTileClicked(Tile clickedTile)
    {
        if (!gameActive) return;
        
        float reactionTime = Time.time - roundStartTime;
        
        // Hide ALL other tiles immediately
        foreach (Tile tile in allTiles)
            if (tile != clickedTile) tile.gameObject.SetActive(false);
    
        if (clickedTile.isOddTile)
        {
            // YOUR PERFECT SCORING: 3/2/1
            int points = GetScoreForReactionTime(reactionTime);
            score += points;
            correctClicks++;
            
            UpdateUI();
            CreateNewRound();  // IMMEDIATE next
        }
        else
        {
            // YOUR WRONG CLICK: -1 life → Next round
            lives--;
            UpdateUI();
            
            if (lives <= 0)
            {
                gameActive = false;
                ShowFinalStats();
            }
            else
            {
                CreateNewRound();  // IMMEDIATE next
            }
        }
    }
    
    /*void CreateNewRound()
    {
        if (!gameActive) return;
        
        roundStartTime = Time.time;
        roundsPlayed++;
        
        // Random rule (1 of 4)
        RuleType rule = (RuleType)Random.Range(0, 4);
        int oddIndex = Random.Range(0, allTiles.Length);
        
        // Reset + apply rule to all tiles
        foreach (Tile tile in allTiles)
        {
            tile.ResetTile();
            bool isOdd = (tile == allTiles[oddIndex]);
            ApplyRule(tile, rule, isOdd);
            
            // Only NON-ODD tiles fade (YOUR SPEC)
            if (!isOdd)
                tile.StartFade();
        }
    }*/

    void CreateNewRound()
{
    if (!gameActive) return;
    
    roundStartTime = Time.time;
    roundsPlayed++;
    
    // FORCE test each rule (remove Random for testing)
    RuleType rule = (RuleType)(roundsPlayed % 4);  // Cycle: 0,1,2,3,0,1...
    int oddIndex = Random.Range(0, allTiles.Length);
    
    Debug.Log($"Round {roundsPlayed}: Rule = {(RuleType)rule}");  // SEE RULES
    
    foreach (Tile tile in allTiles)
    {
        tile.ResetTile();
        bool isOdd = (tile == allTiles[oddIndex]);
        ApplyRule(tile, rule, isOdd);
        
        if (!isOdd)
            tile.StartFade();
    }
}
    
    void ApplyRule(Tile tile, RuleType rule, bool isOdd)
    {
        tile.isOddTile = isOdd;
        
        switch (rule)
        {
            case RuleType.Color:
                tile.tileImage.color = isOdd ? Color.red : Color.blue;
                break;
                
            case RuleType.Pattern:
                tile.transform.localScale = Vector3.one * (isOdd ? 1.2f : 0.9f);
                tile.transform.rotation = Quaternion.Euler(0, 0, isOdd ? 45f : 0f);
                tile.tileImage.color = Color.cyan;
                break;
                
            case RuleType.Size:
                tile.transform.localScale = Vector3.one * (isOdd ? 1.4f : 1f);
                tile.tileImage.color = Color.yellow;
                break;
                
            case RuleType.Dots:
                tile.dotCount = isOdd ? 4 : 2;
                tile.SetupDots();
                tile.tileImage.color = Color.magenta;
                break;
        }
    }
    
    int GetScoreForReactionTime(float time)
    {
        if (time <= 2.0f) return 3;      // 🟢 Fast
        else if (time <= 3.5f) return 2; // 🟡 Medium
        else if (time <= 5.0f) return 1; // 🔴 Slow
        return 0;
    }
    
    void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";
    }
    
    void ShowFinalStats()
    {
        float accuracy = roundsPlayed > 0 ? (float)correctClicks / roundsPlayed * 100f : 0f;
        Debug.Log($"=== GAME OVER ===\nScore: {score}\nRounds: {roundsPlayed}\nAccuracy: {accuracy:F1}%\nLives Left: {lives}");
    }
}


