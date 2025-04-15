using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
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
    public Text SuperText;
    private bool isRunning = true;
    public Piece pieceManager;
    public Board board;
    public bool gameOver;
    public bool mainMenu;
    private ScoreboardManager scoreboardManager;
    public bool isFeverMode;
    private float feverEndTime;
    public GameObject feverOverlay;
    public float feverDuration = 10f;
    public Camera mainCamera;
    public AudioSource musicSource;
    public AudioClip normalMusic;
    public AudioClip superMusic;
    public PowerupInventory powerupInventory = new PowerupInventory();
    public Text explosionCountText;
    public Text freezeCountText;
    public Text laserCountText;
    private int explosionMilestone = 0;
    private int freezeMilestone = 0;
    private int laserMilestone = 0;



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
        board.linesCleared = 0;
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
            
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }

        if (ShouldIncreaseLevel())
        {
            IncreaseLevel();
        }
        if (isFeverMode && Time.time >= feverEndTime)
        {
            isFeverMode = false;
            SuperText.gameObject.SetActive(false);
            StopCoroutine(PulseFeverTextColor());
            StartCoroutine(FadeMusic(normalMusic, 1f));

            pieceManager.SetSpeed(GetSpeedForLevel());
        }

        CheckPowerupConditions();
        UpdatePowerupUI();
        UpdateLeaderboardDuringGameplay();
        ShowPB(score, level);
        ShowRank(score);
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
        ShowRank(score);
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
        ShowRank(score);
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
        UpdateLeaderboardDuringGameplay();
        ShowPB(score, level);
        ShowRank(score);
    }

    private float GetSpeedForLevel()
    {
        return Mathf.Max(0.1f, 1f - (level * 0.1f));
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + score.ToString();
        UpdateLeaderboardDuringGameplay();
        ShowPB(score, level);
        ShowRank(score);
    }

    public void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + level.ToString();
        UpdateLeaderboardDuringGameplay();
        ShowPB(score, level);
        ShowRank(score);
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

    private void ShowRank(int score)
    {
        List<ScoreboardItem> sortedList = new List<ScoreboardItem>(scoreboardManager.ScoreboardList);
        sortedList.Sort((a, b) => b.score.CompareTo(a.score));

        int playerRank = sortedList.FindIndex(item => item.playerName == playerName);

        if (playerRank == -1)
        {
            return;
        }

        currentRank.text = $"{playerRank + 1}";

        if (playerRank > 0)
        {
            ScoreboardItem abovePlayer = sortedList[playerRank - 1];
            abovePlayerText.text = $"Above You: {abovePlayer.playerName} | Score: {abovePlayer.score}";
        }
        else
        {
            abovePlayerText.text = "You are the top player!";
        }

        if (playerRank == 0)
        {
            currentRank.color = Color.green;
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
    public void UpdateLeaderboardDuringGameplay()
    {
        ScoreboardManager.Instance.AddNewEntry(playerName, score, level);
        PopulateLeaderboardPanel();
    }

    public void ActivateFeverMode()
    {
        isFeverMode = true;
        StartCoroutine(FadeMusic(superMusic, 1f));
        feverEndTime = Time.time + feverDuration;
        if (SuperText) 
        {
            SuperText.gameObject.SetActive(true);
            StartCoroutine(PulseFeverTextColor());
        }

        List<Color> feverColors = new List<Color>
        {
            Color.red,
            Color.magenta,
            Color.yellow,
            Color.cyan,
            Color.green
        };

        StartCoroutine(FeverBackgroundEffect(feverColors, 10f));

        pieceManager.SetSpeed(GetSpeedForLevel() * 0.5f);
    }

    private IEnumerator PulseFeverTextColor()
    {
        Text text = SuperText.GetComponent<Text>();
        float pulseSpeed = 2f;

        while (true)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);

            Color pulseColor = Color.Lerp(Color.red, Color.yellow, t);
            text.color = pulseColor;

            yield return null;
        }
    }

    public IEnumerator FeverBackgroundEffect(List<Color> feverColors, float duration)
    {
        Color originalColor = mainCamera.backgroundColor;
        float timer = 0f;
        int colorIndex = 0;

        while (timer < duration)
        {
            Color fromColor = feverColors[colorIndex % feverColors.Count];
            Color toColor = feverColors[(colorIndex + 1) % feverColors.Count];
            
            float t = 0f;
            float segmentDuration = 0.5f; // how long to stay between two colors

            while (t < segmentDuration && timer < duration)
            {
                // Pulse with Mathf.PingPong for smoother transitions
                float pulse = Mathf.PingPong(t * 2f, 1f);
                mainCamera.backgroundColor = Color.Lerp(fromColor, toColor, pulse);

                t += Time.deltaTime;
                timer += Time.deltaTime;
                yield return null;
            }

            colorIndex++;
        }

        mainCamera.backgroundColor = originalColor;
    }
    IEnumerator FadeMusic(AudioClip newClip, float fadeTime = 1f)
    {
        float startVolume = musicSource.volume;

        // Fade out
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    public void CheckPowerupConditions()
    {
        // Explosion powerup for every 3 lines cleared
        int explosionThreshold = 3;
        if (board.linesCleared >= (explosionMilestone + 1) * explosionThreshold)
        {
            powerupInventory.AddPowerup(PowerupType.Explosion);
            explosionMilestone++;
        }

        // Freeze powerup for every 60 seconds
        int freezeThreshold = 60;
        if (timer >= (freezeMilestone + 1) * freezeThreshold)
        {
            powerupInventory.AddPowerup(PowerupType.Freeze);
            freezeMilestone++;
        }

        // Laser powerup for every 500 score
        int laserThreshold = 500;
        if (score >= (laserMilestone + 1) * laserThreshold)
        {
            powerupInventory.AddPowerup(PowerupType.Laser);
            laserMilestone++;
        }
    }
    void UpdatePowerupUI()
    {
        explosionCountText.text = "Explosion: " + powerupInventory.explosionCount;
        freezeCountText.text = "Freeze: " + powerupInventory.freezeCount;
        laserCountText.text = "Laser: " + powerupInventory.laserCount;
    }

}