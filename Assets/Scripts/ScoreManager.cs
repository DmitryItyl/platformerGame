using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static int totalScore = 0;

    private const int coinValue = 100;

    public static void AddCoinScore()
    {
        totalScore += coinValue;
    }

    public static void ResetScore()
    {
        totalScore = 0;
        PlayerPrefs.SetInt("Score", 0);
    }
}
