using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] Canvas cv;
    public void ReloadScene()
    {
        Debug.Log("restart clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        cv.resetTime();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTut1()
    {
        SceneManager.LoadScene("Test_tut");
    }

    public void LoadTut2()
    {
        SceneManager.LoadScene("Test_tut2");
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
