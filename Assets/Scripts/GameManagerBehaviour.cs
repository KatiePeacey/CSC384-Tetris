using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject game;
    public GameObject MainMenu;
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
        //NewGame();
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

        pieceManager.SetSpeed(GetSpeedForLevel());
        board.SpawnPiece();
    }
    public void GameOver()
    {
        board.tilemap.ClearAllTiles();
        gameOverMenu.SetActive(true);
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
    private void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + level.ToString();
    }
}
