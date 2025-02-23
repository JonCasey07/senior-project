using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject newGame;

    public void Start()
    {
        if (PlayerPrefs.GetInt("level") != 0)
        {
            newGame.SetActive(true);
        }
    }
    public void PlayGame()
    {
        EventManager.level = PlayerPrefs.GetInt("level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("level", 0);
        EventManager.level = PlayerPrefs.GetInt("level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
