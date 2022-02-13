using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<LeaderBoardEntry> highScoresList = FileManager.ReadFromJSON<LeaderBoardEntry>(FileManager.highScoreFileName);
        for (int i = 0; i < highScoresList.Count; i++)
        {
            LeaderBoardEntry highScore = highScoresList[i];
            Transform entry = transform.Find(string.Format("highscoreEntry{0}", i + 1));
            Text entryName = entry.Find("nameText").GetComponent<Text>();
            Text entryScore = entry.Find("scoreText").GetComponent<Text>();

            entryName.text = highScore.playerName;
            entryScore.text = highScore.score.ToString();
        }
    }
}
