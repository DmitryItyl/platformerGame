using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool isPaused = false;
    public void StartGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level", 1));
        ScoreManager.totalScore = PlayerPrefs.GetInt("Score", 0);
        Time.timeScale = 1f;
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        //Unpause game in case function was called from pause menu
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }

        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void LoadLeaderBoardsMenu()
    {
        SceneManager.LoadScene("LeaderBoardMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void SetPlayerName(InputField inputFieldSource)
    {
        PlayerPrefs.SetString("Name", inputFieldSource.text);
    }

}
