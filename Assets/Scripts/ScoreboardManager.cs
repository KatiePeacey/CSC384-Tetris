using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ScoreboardManager : MonoBehaviour
{
    public static ScoreboardManager Instance { get; private set; }

    private List<ScoreboardItem> scoreboardList = new List<ScoreboardItem>();
    public List<ScoreboardItem> ScoreboardList => scoreboardList;

    private string filename;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filename = Path.Combine(Application.persistentDataPath, "scorelist.json");
            LoadScoreboardData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewEntry(string playerName, int score, int levelCompleted)
    {
        Debug.Log($"Attempting to add new entry - Name: {playerName}, Score: {score}, Level: {levelCompleted}");

        // Check if the player already exists in the leaderboard
        ScoreboardItem existingItem = scoreboardList.Find(item => item.playerName == playerName);

        if (existingItem == null)
        {
            // If the player doesn't exist, add a new entry
            ScoreboardItem newItem = new ScoreboardItem(playerName, score, levelCompleted);
            scoreboardList.Add(newItem);
            Debug.Log("✅ New player added to scoreboard.");
        }
        else if (score > existingItem.score)
        {
            // If the score is higher, update the existing entry
            Debug.Log("🔄 Updating existing player score.");
            existingItem.score = score;
            existingItem.levelCompleted = levelCompleted;
        }
        else
        {
            // If the score is not higher, do nothing
            Debug.Log("⏩ Score is not higher, not updating.");
        }

        // Sort the scoreboard in descending order by score
        scoreboardList.Sort((a, b) => b.score.CompareTo(a.score));

        SaveScoreboardData();  // Save the updated leaderboard to a file
    }



    private void SaveScoreboardData()
    {
        File.WriteAllText(filename, JsonUtility.ToJson(new ScoreboardWrapper(scoreboardList)));
    }

    private void LoadScoreboardData()
    {
        if (File.Exists(filename))
        {
            string jsonData = File.ReadAllText(filename);
            Debug.Log($"Loading data: {jsonData}"); // ✅ Debug: See saved JSON data

            ScoreboardWrapper data = JsonUtility.FromJson<ScoreboardWrapper>(jsonData);
            if (data != null && data.scoreboardList != null)
            {
                scoreboardList = data.scoreboardList;
                Debug.Log($"✅ Loaded {scoreboardList.Count} scores.");
                foreach (var item in scoreboardList)
                {
                    Debug.Log($"✅ Loaded Entry - Name: {item.playerName}, Score: {item.score}, Level: {item.levelCompleted}");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Scoreboard data is empty or corrupted.");
                scoreboardList = new List<ScoreboardItem>();
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No scoreboard file found. Creating a new list.");
            scoreboardList = new List<ScoreboardItem>();
        }
    }
    [System.Serializable]
    private class ScoreboardWrapper
    {
        public List<ScoreboardItem> scoreboardList;
        public ScoreboardWrapper(List<ScoreboardItem> list) => scoreboardList = list;
    }
}

