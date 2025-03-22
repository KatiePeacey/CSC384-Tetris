using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreboardMangager : MonoSingleton<ScoreboardMangager>
{
    private List<ScoreboardItem> scoreboardList = new List<ScoreboardItem>();
    public List<ScoreboardItem> ScoreboardList { get { return scoreboardList; } }

    protected override void Initialize()
    {
        base.Initialize();
        LoadScoreboardData();
    }

    public void AddNewEntry(MatchInfo info)
    {
        bool found = false;
        for (int itemIndex = 0; itemIndex < scoreboardList.Count; itemIndex++)
        {
            if (scoreboardList[itemIndex].playerName == info.PlayerName)
            {
                found = true;
                break;
            }
        }

        if (found == false)
        {
            ScoreboardItem item = new ScoreboardItem();
            item.SetData(info);
            scoreboardList.Add(item);
        }

        scoreboardList.Sort((item1, item2)=>item1.score.CompareTo(item2.score));

        SaveScoreboardData();
    }

    public void RemoveEntryByPlayerName(string playerName)
    {
        int indexFound = -1;
        for (int itemIndex = 0; itemIndex < scoreboardList.Count; ++itemIndex)
        {
            if (scoreboardList[itemIndex].playerName == playerName)
            {
                indexFound = itemIndex;
                break;
            }
        }

        if (indexFound != -1)
        {
            scoreboardList.RemoveAt(indexFound);
        }
    }

    public ScoreboardItem GetScoreboardItemByName(string playerName)
    {
        ScoreboardItem item = null;
        for (int itemIndex = 0; itemIndex < scoreboardList.Count; ++itemIndex)
        {
            if (scoreboardList[itemIndex].playerName == playerName)
            {
                item = scoreboardList[itemIndex];
                break;
            }
        }
        return item;
    }

    public void SaveScoreboardData()
    {
        string filename = string.Format("{0}/{1}", Application.persistentDataPath, "scorelist.json");
        StreamWriter streamWriter = new StreamWriter(filename);
        Debug.LogError("SAVING DATA...");
        foreach (ScoreboardItem item in scoreboardList)
        {
            string jsonData = JsonUtility.ToJson(item);
            Debug.LogError(jsonData);
            streamWriter.WriteLine(jsonData);
        }
        streamWriter.Close();
        Debug.LogError("...END");
    }

        public void LoadScoreboardData()
    {
        string filename = string.Format("{0}/{1}", Application.persistentDataPath, "scorelist.json");
        if (File.Exists(filename))
        {
            StreamReader streamReader = new StreamReader(filename);
            Debug.LogError("LOADING DATA...");
            while (!streamReader.EndOfStream)
            {
                string jsonData = streamReader.ReadLine();
                Debug.LogError(jsonData);
                ScoreboardItem item = JsonUtility.FromJson<ScoreboardItem>(jsonData);
                scoreboardList.Add(item);
            }
        streamReader.Close();
        Debug.LogError("...END");
        }
    }
}
