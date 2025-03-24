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
        ScoreboardItem existingItem = scoreboardList.Find(item => item.playerName == playerName);

        if (existingItem == null)
        {
            ScoreboardItem newItem = new ScoreboardItem(playerName, score, levelCompleted);
            scoreboardList.Add(newItem);
        }
        else if (score > existingItem.score)
        {
            existingItem.score = score;
        }

        scoreboardList.Sort((a, b) => b.score.CompareTo(a.score));
        SaveScoreboardData();
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
            scoreboardList = JsonUtility.FromJson<ScoreboardWrapper>(jsonData)?.scoreboardList ?? new List<ScoreboardItem>();
        }
    }

    [System.Serializable]
    private class ScoreboardWrapper
    {
        public List<ScoreboardItem> scoreboardList;
        public ScoreboardWrapper(List<ScoreboardItem> list) => scoreboardList = list;
    }
}

