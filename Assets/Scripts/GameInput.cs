using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
            ReloadLevel();
        else if (Input.GetKeyDown(KeyCode.Escape))
            Exit();
    }

    void ReloadLevel()
    {
        //prevent time freeze
        Time.timeScale = 1f;
        //reload scene
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }

    void Exit()
    {
        Application.Quit();
    }
}
