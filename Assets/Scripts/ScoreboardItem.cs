using UnityEngine;

[System.Serializable]

public class ScoreboardItem
{
    [SerializeField] public string playerName = string.Empty;
    [SerializeField] public int score = 0;
    [SerializeField] public int levelsCompleted = 0;

    public void SetData(MatchInfo info)
    {
        playerName = info.playerName;
        score = info.score;
        levelsCompleted = info.levelsCompleted;
    }
}
