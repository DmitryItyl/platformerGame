using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int totalScore = 0;

    private const int coinValue = 100;

    public static void AddCoinScore()
    {
        totalScore += coinValue;
        Debug.Log(totalScore);
    }

    public static void ResetScore()
    {
        totalScore = 0;
    }
}
