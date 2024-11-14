using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void button_tutorial()
    {
        SceneManager.LoadScene("Test_tut");
        Debug.Log("Tutorial button clicked");
        LogMaterialInfo();
    }
    
    public void button_Level1()
    {
        SceneManager.LoadScene("Level 1");
        Debug.Log("Level1 button clicked");
    }
    
    public void button_Level2()
    {
        SceneManager.LoadScene("Level 2");
        Debug.Log("Level2 button clicked");
    }
    
    public void button_exit()
    {
        Application.Quit();
        Debug.Log("Exit button clicked");
    }
    void LogMaterialInfo()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    Debug.Log($"Object: {obj.name}, Material: {mat.name}, Shader: {mat.shader.name}");
                }
            }
        }
    }
}
