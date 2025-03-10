using UnityEngine;
using System.Collections;
using System;

public class EventManager : MonoBehaviour
{
    public static int level = 0;
    public static bool levelEnding = false;

    public static EventManager genEvents;
    public event Action LevelComplete;

    public static EventManager level1;


    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    private void Awake()
    {
        if (genEvents == null)
        {
            genEvents = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteTutLevel()
    {
        LevelComplete?.Invoke();
    }

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("level", level);
    }

}
