using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] InputField nameField;
    [SerializeField] Text scoreText;

    const int maximumLeaderboardEntries = 10;


    // Start is called before the first frame update
    void Start()
    {
        nameField.text = PlayerPrefs.GetString("Name");
        scoreText.text = ScoreManager.totalScore.ToString();
    }

    public void SavePlayerResult()
    {
        string playerName = nameField.text;
        int playerScore = ScoreManager.totalScore;

        PlayerPrefs.SetString("Name", playerName);
        PlayerPrefs.SetInt("Level", 1);
        ScoreManager.ResetScore();

        UpdateLeaderBoard(playerName, playerScore);
    }

    void UpdateLeaderBoard(string playerName, int score)
    {
        List<LeaderBoardEntry> leaderBoard = FileManager.ReadFromJSON<LeaderBoardEntry>(FileManager.highScoreFileName);

        LeaderBoardEntry newLeaderBoardEntry = new LeaderBoardEntry(playerName, score);
        leaderBoard.Add(newLeaderBoardEntry);
        leaderBoard = leaderBoard.OrderByDescending(x => x.score).ToList();
        if (leaderBoard.Count > maximumLeaderboardEntries)
            leaderBoard.RemoveAt(maximumLeaderboardEntries);

        FileManager.SaveToJSON<LeaderBoardEntry>(leaderBoard, FileManager.highScoreFileName);
    }
}

[Serializable]
class LeaderBoardEntry
{
    public string playerName;
    public int score;

    public LeaderBoardEntry(string name, int score) 
    {
        this.score = score;
        this.playerName = name;
    }
}
