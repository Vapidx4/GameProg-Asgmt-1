using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("Level1");
        GameManager.Instance.score = 0;

    }

    public void OnGameExit()
    {
        // If it is running in the editor it closes the instance
        // If its running on a build, it closes it.
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}