using System;

[Serializable]
public class ScoreboardItem
{
    public string playerName;
    public int score;
    public int levelCompleted;

    public ScoreboardItem(string playerName, int score, int levelCompleted)
    {
        this.playerName = playerName;
        this.score = score;
        this.levelCompleted = levelCompleted;
    }
}
