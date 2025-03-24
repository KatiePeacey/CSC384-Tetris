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
    private float timer = 0f;
    public Text timerText;
    public Text scoreText;
    public Text levelText;
    private bool isRunning = true;
    public Piece pieceManager;
    public Board board;

    public void Start()
    {
        board.tilemap.ClearAllTiles();
        MainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        game.SetActive(false);
        Leaderboard.SetActive(false);

        Debug.Log("Before calling PopulateLeaderboardPanel");
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

        pieceManager.SetSpeed(GetSpeedForLevel());
        board.SpawnPiece();
    }
    public void GameOver()
    {
        board.tilemap.ClearAllTiles();
        gameOverMenu.SetActive(true);
        game.SetActive(false);
        MainMenu.SetActive(false);
        Leaderboard.SetActive(false);

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
    }

    private float GetSpeedForLevel()
    {
        return Mathf.Max(0.1f, 1f - (level * 0.1f));
    }
    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString();
    }
    public int GetScore() => score;
    private void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + level.ToString();
    }
    public int GetLevel() => level;
    public void PopulateLeaderboardPanel()
    {
        foreach (Transform child in ScoreboardPanel)
        {
            Destroy(child.gameObject);
        }

        List<ScoreboardItem> scores = ScoreboardManager.Instance.ScoreboardList;
        Debug.Log("ScoreboardManager is available. Populating entries...");
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
                Debug.LogError("Failed to instantiate scoreboardItemPrefab.");
                continue;
            }

            index++;
        }
    }
}
