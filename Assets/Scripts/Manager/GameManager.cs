using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;
using UnityEditor.SpeedTree.Importer;
using System.Security.Cryptography;

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

    [Header("Adaptive Difficulty")]
    public float difficulty = 1f;  // 1.0 = normal, 2.0 = hard
    private int consecutiveCorrect = 0;
    private int consecutiveWrong = 0;


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
    
        // FASTER FADE: 5s → 3s based on difficulty
        float fadeTime = 3f - ((difficulty - 1f) * 2f);  // 5s → 3s
        fadeTime = Mathf.Max(2f, fadeTime);  // Minimum 2s
    
        if (Time.time - roundStartTime >= fadeTime)
        {
            roundsPlayed++;
            CreateNewRound();
        }
    }

    void UpdateDifficulty()
{
    consecutiveCorrect++;
    consecutiveWrong = 0;
    
    // 5+ correct → Harder
    if (consecutiveCorrect >= 5 && difficulty < 2f)
    {
        difficulty = Mathf.Min(2f, difficulty + 0.2f);
        Debug.Log($"⬆️ HARDER! Diff: {difficulty:F1} (Fade: {5f-((difficulty-1f)*2f):F1}s)");
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
            UpdateDifficulty();
            CreateNewRound();  // IMMEDIATE next
        }
        else
        {

            consecutiveWrong++;
            consecutiveCorrect = 0;
            if (consecutiveWrong >= 1 && difficulty > 1f)
            {
                difficulty = Mathf.Max(1f, difficulty - 0.5f);
                Debug.Log($"⬇️ EASIER! Diff: {difficulty:F1}");
            }

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
    
    
    RuleType rule = (RuleType)(roundsPlayed % 4);  
    int oddIndex = UnityEngine.Random.Range(0, allTiles.Length);
    
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
        float diff = difficulty;
        
        switch (rule)
        {
            case RuleType.Color:
                tile.tileImage.color = isOdd ? Color.red : Color.blue;
                break;
                
            case RuleType.Pattern:
                float rotationAmount = Mathf.Lerp(20f, 10f, (diff - 1f));
                tile.transform.rotation = Quaternion.Euler(0, 0, isOdd ? rotationAmount : 0f);
                tile.transform.localScale = Vector3.one * (isOdd ? 0.9f : 0.9f);
                tile.tileImage.color = Color.cyan;
                break;
                
            case RuleType.Size:
                float sizeOdd = Mathf.Lerp(085f, 0.9f, (diff -1f));
                tile.transform.localScale = Vector3.one * (isOdd ? sizeOdd : 1f);
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
        if (time <= 0.5f) return 3;      // 🟢 Fast
        else if (time <= 2.0f) return 2; // 🟡 Medium
        else if (time <= 3.0f) return 1; // 🔴 Slow
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


