using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehaviour : MonoBehaviour
{
    public int level = 1;
    public int score = 0;
    public Text timerText;
    private float timer = 0f;
    private bool isRunning = true;
    
    public Piece pieceManager;
    
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
            timerText.text = timer.ToString("F2");
        }

        if (ShouldIncreaseLevel())
        {
            IncreaseLevel();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime()
    {
        return timer;
    }

    public void NewGame()
    {
        isGameOver = false;
        score = 0;
        level = 1;
        timer = 0;

        pieceManager.SetSpeed(GetSpeedForLevel());
        SpawnNewPiece();
    }

    private bool ShouldIncreaseLevel()
    {
        return (int)(timer / 30) >= level;
    }

    private void IncreaseLevel()
    {
        level++;
        pieceManager.SetSpeed(GetSpeedForLevel());
    }

    private float GetSpeedForLevel()
    {
        return Mathf.Max(0.1f, 1f - (level * 0.1f));
    }

    private void SpawnNewPiece()
    {
        // Logic to spawn a new tetromino
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

    public void AddScore(int linesCleared)
    {
        int points = linesCleared switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0
        };
        score += points;
    }
}
