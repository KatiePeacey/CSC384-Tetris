using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject game;
    public int level = 0;
    public int score = 0;
    private float timer = 0f;
    public Text timerText;
    public Text scoreText;
    public Text levelText;
    private bool isRunning = true;
    private bool isGameOver = false;
    public Piece pieceManager;
    public Board board;

    void Start()
    {
        NewGame();
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
        isGameOver = false;
        gameOverMenu.SetActive(false);
        game.SetActive(true);
        score = 0;
        level = 1;
        timer = 0;

        pieceManager.SetSpeed(GetSpeedForLevel());
        board.SpawnPiece();
    }
    public void GameOver()
    {
        isGameOver = true;
        board.tilemap.ClearAllTiles();
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        game.SetActive(false);
    }
    public void RestartGame()
    {
        Time.timeScale = 1; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current scene
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
    private void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + level.ToString();
    }
}
