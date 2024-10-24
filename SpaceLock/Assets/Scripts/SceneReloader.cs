using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        Debug.Log("restart clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
}
