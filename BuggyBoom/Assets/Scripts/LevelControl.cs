using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour {

    [Tooltip("To activate enter > zero")] public float autoLoadLevelAfterSeconds;

    void Start()
    {
        if (autoLoadLevelAfterSeconds > 0)
        {
            Invoke("LoadNextLevel", autoLoadLevelAfterSeconds);
        }
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
