using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour

{
    public GameObject homePanel, gamePanel, resultPanel, rulesPanel;
    public TextMeshProUGUI scoreText, livesText, finalScoreText, bestScoreText;
    public GameObject gridContainer;

/*    void Start()
{
    if(gridContainer) gridContainer.SetActive(false);  // HIDE GRID FIRST

}*/



    public void ShowHome() 
    { 
        HideAll(); 
        homePanel.SetActive(true);
        if(gridContainer) gridContainer.SetActive(false); 
    }
    public void ShowGame()
    {
         HideAll();
        gamePanel.SetActive(true);
        if(gridContainer) gridContainer.SetActive(true);

        // FORCE refresh tiles + rules
        GameManager gm = FindAnyObjectByType<GameManager>();
        gm.ResetGame();
    
        // Wait 1 frame then spawn new round
        StartCoroutine(SpawnNewRound(gm));
         gm.UpdateUI();
        
    }
    public void ShowResult(int score) 
    { 
        HideAll(); 
        finalScoreText.text = $"Your Score: {score}"; 
        bestScoreText.text = $"Best: {PlayerPrefs.GetInt("BestScore", 0)}";
        resultPanel.SetActive(true); 
        if(gridContainer) gridContainer.SetActive(false);
    }
    public void ShowRules() 
    { 
        HideAll(); 
        rulesPanel.SetActive(true); 
        if(gridContainer) gridContainer.SetActive(false);
    }
    public void QuitGame()
    { 
        Application.Quit(); 
        Debug.Log("Game Quit"); 
    }

    private IEnumerator SpawnNewRound(GameManager gm)
    {
        yield return null;  // Wait 1 frame
        gm.CreateNewRound();
    }

    
    void HideAll()
    {
        
        homePanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
        rulesPanel.SetActive(false);
        
    }
}

