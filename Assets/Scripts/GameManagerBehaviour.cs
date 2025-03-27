using System;
using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
 
 public class GameManagerBehaviour : MonoBehaviour
 {
     public GameObject gameOverMenu;
     public GameObject game;
     public GameObject MainMenu;
     public GameObject Leaderboard;
     public Transform ScoreboardPanel;
     public GameObject scoreboardItemPrefab;
     public int level = 0;
     public int score = 0;
     private string playerName;
     private float timer = 0f;
     public Text timerText;
     public Text scoreText;
     public Text levelText;
    public Text scorePBText;
    public Text levelPBText;
     private bool isRunning = true;
     public Piece pieceManager;
     public Board board;

     public bool gameOver;
     public bool mainMenu;
     private ScoreboardManager scoreboardManager;
     public void SetPlayerName(string name)
    {
        playerName = name;
    }
    public void ResetGameStats()
    {
        score = 0;
        level = 1;
        timer = 0f;
        timerText.text = "Time: " + timer.ToString("F2");
        scoreText.text = "Score: " + score;
        levelText.text = "Level: " + level;
        Debug.Log("Game stats reset.");
    }
     public void Start()
     {
        scoreboardManager = ScoreboardManager.Instance;
         board.tilemap.ClearAllTiles();
         MainMenu.SetActive(true);
         gameOverMenu.SetActive(false);
         game.SetActive(false);
         Leaderboard.SetActive(false);
         mainMenu = true;

         PopulateLeaderboardPanel();
     }
     void Update()
     {
         if (isRunning)
         {
             timer += Time.deltaTime;
             timerText.text = "Time: " + timer.ToString("F2");
         }
 
         if (ShouldIncreaseLevel())
         {
             IncreaseLevel();
         }
     }
     public void NewGame()
     {
         gameOverMenu.SetActive(false);
         MainMenu.SetActive(false);
         game.SetActive(true);
         Leaderboard.SetActive(false);
         
         ResetGameStats();
         pieceManager.SetSpeed(GetSpeedForLevel());
         board.SpawnPiece();
         ShowPB(score, level);
         gameOver = false;
         mainMenu = true;
     }
     public void GameOver()
     {
         board.tilemap.ClearAllTiles();
         gameOverMenu.SetActive(true);
         game.SetActive(false);
         MainMenu.SetActive(false);
         Leaderboard.SetActive(false);
         
         gameOver = true;
         mainMenu = false;
         ScoreboardManager.Instance.AddNewEntry(playerName, score, level);
         PopulateLeaderboardPanel();
     }
     public void ShowLeaderboard()
     {
         Leaderboard.SetActive(true);
         gameOverMenu.SetActive(false);
         game.SetActive(false);
         MainMenu.SetActive(false);
     }
 
     private bool ShouldIncreaseLevel()
     {
         return (int)(timer / 30) >= level;
     }
 
     private void IncreaseLevel()
     {
         pieceManager.SetSpeed(GetSpeedForLevel());
         SetScore(score + 100);
         SetLevel(level + 1);
         ShowPB(score, level);
     }
 
     private float GetSpeedForLevel()
     {
         return Mathf.Max(0.1f, 1f - (level * 0.1f));
     }
     public void SetScore(int score)
     {
         this.score = score;
         scoreText.text = "Score: " + score.ToString();
         ShowPB(score, level);

     }
     public void SetLevel(int level)
     {
         this.level = level;
         levelText.text = "Level: " + level.ToString();
         ShowPB(score, level);
     }

     public void ShowPB(int score, int level)
    {  
        // Find the existing player by name in the scoreboard
        ScoreboardItem existingItem = scoreboardManager.ScoreboardList.Find(item => item.playerName == playerName);

        if (existingItem == null)
        {
            // No previous entry for the player, so show as a new PB
            scorePBText.text = "New PB Score: " + score.ToString();
            scorePBText.color = Color.yellow;
            levelPBText.text = "New PB Level: " + level.ToString();
            levelPBText.color = Color.yellow;
        }
        else if (score > existingItem.score)
        {
            // If the new score is higher than the existing one, update to new PB
            scorePBText.text = "New PB Score: " + score.ToString();
            scorePBText.color = Color.yellow;
            levelPBText.text = "New PB Level: " + level.ToString();
            levelPBText.color = Color.yellow;
        }
        else
        {
            // If the new score is not higher, show the current PB
            scorePBText.text = "PB Score: " + existingItem.score.ToString();
            scorePBText.color = Color.white;
            levelPBText.text = "PB Level: " + existingItem.levelCompleted.ToString();
            levelPBText.color = Color.white;
        }
    }


    public void PopulateLeaderboardPanel()
     {
         foreach (Transform child in ScoreboardPanel)
         {
             Destroy(child.gameObject);
         }
 
         List<ScoreboardItem> scores = ScoreboardManager.Instance.ScoreboardList;
         int index = 1;
 
         foreach (ScoreboardItem item in scores)
         {
             GameObject go = Instantiate(scoreboardItemPrefab, ScoreboardPanel);
             Debug.Log($"Creating leaderboard item {index} for player: {item.playerName}, score: {item.score}, level: {item.levelCompleted}");
             ScoreboardItemUI itemUI = go.GetComponent<ScoreboardItemUI>();
 
 
             if (itemUI != null)
             {
                 itemUI.SetData(index, item);
             }
 
             if (go == null)
             {
                 continue;
             }
 
             index++;
         }
     }
 }