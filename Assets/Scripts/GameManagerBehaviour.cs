using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public int level = 0;
    public int score = 0;
    private float timer = 0f;
    public Text timerText;
    public Text scoreText;
    public Text levelText;
    private bool isRunning = true;
    
    public Piece pieceManager;
    public Board board;

    
    private bool isGameOver = false;

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
        score = 0;
        level = 1;
        timer = 0;

        pieceManager.SetSpeed(GetSpeedForLevel());
        board.SpawnPiece();
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

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! Press R to Restart");
    }

    public void RestartGame()
    {
        NewGame();
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
