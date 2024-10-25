using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public int currentLevel;
    public void ReloadScene()
    {
        Debug.Log("restart clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel2()
    {
        currentLevel++;
        AnalyticsManager.Instance.NextLevel(currentLevel);
        SceneManager.LoadScene("Level 2");
    }
}
