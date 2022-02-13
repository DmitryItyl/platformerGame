using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSceneManager : MonoBehaviour
{
    [SerializeField] InputField nameField;
    [SerializeField] Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        nameField.text = PlayerPrefs.GetString("Name");
        scoreText.text = ScoreManager.totalScore.ToString();
    }
}
