using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    public int level = 1;
    public int score = 0;
    public float gameTime = 0f;
    
    public Piece pieceManager;
    
    private bool isGameOver = false;

    void Start()
    {
        NewGame();
    }

    void Update()
    {
        if (isGameOver) return;

        gameTime += Time.deltaTime;
        
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
        gameTime = 0f;

        pieceManager.SetSpeed(GetSpeedForLevel());
        SpawnNewPiece();
    }

    private bool ShouldIncreaseLevel()
    {
        return (int)(gameTime / 30) >= level;
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
