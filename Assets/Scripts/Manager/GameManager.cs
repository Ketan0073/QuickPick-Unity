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
    
    void Start()
    {
        stopButton.onClick.AddListener(OnStopButton);
        UpdateUI();
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
}

