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
    public Text abovePlayerText;
    public Text currentRank;
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
        ShowPB(score, level);
        ShowRank();
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
        PopulateLeaderboardPanel();
        ShowPB(score, level);
        ShowRank();
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
        ShowRank();
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
        ShowRank();
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
        ShowRank();
    }
    public void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + level.ToString();
        ShowPB(score, level);
        ShowRank();
    }
    public void ShowPB(int score, int level)
    {  
        ScoreboardItem existingItem = scoreboardManager.ScoreboardList.Find(item => item.playerName == playerName);

        if (existingItem == null)
        {
            scorePBText.text = "New PB Score: " + score.ToString();
            scorePBText.color = Color.yellow;
            levelPBText.text = "New PB Level: " + level.ToString();
            levelPBText.color = Color.yellow;
        }
        else if (score > existingItem.score)
        {
            scorePBText.text = "New PB Score: " + score.ToString();
            scorePBText.color = Color.yellow;
            levelPBText.text = "New PB Level: " + level.ToString();
            levelPBText.color = Color.yellow;
        }
        else
        {
            scorePBText.text = "PB Score: " + existingItem.score.ToString();
            scorePBText.color = Color.white;
            levelPBText.text = "PB Level: " + existingItem.levelCompleted.ToString();
            levelPBText.color = Color.white;
        }
    }

   private void ShowRank()
    {
        // Sort the leaderboard based on score in descending order
        List<ScoreboardItem> sortedList = new List<ScoreboardItem>(scoreboardManager.ScoreboardList);
        sortedList.Sort((a, b) => b.score.CompareTo(a.score));

        // Find the current player's rank in the sorted leaderboard
        int playerRank = sortedList.FindIndex(item => item.playerName == playerName);

        // If the player is not found, return early
        if (playerRank == -1)
        {
            return;
        }

        // Display the current player's rank (1-based index)
        currentRank.text = $"You're Rank: {playerRank + 1}";

        // Check if there is a player above the current player
        if (playerRank > 0)
        {
            // Get the player who is currently above the current player
            ScoreboardItem abovePlayer = sortedList[playerRank - 1];
            abovePlayerText.text = $"Above You: {abovePlayer.playerName} | Score: {abovePlayer.score}";
        }
        else
        {
            // If the current player is at the top, display a different message
            abovePlayerText.text = "You are the top player!";
        }

        // Change the color based on the player's rank
        if (playerRank == 0)
        {
            currentRank.color = Color.green;  // Top player
        }
        else if (playerRank <= 3)
        {
            currentRank.color = Color.blue;  // Top 3 players
        }
        else
        {
            currentRank.color = Color.white;  // Lower ranks
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