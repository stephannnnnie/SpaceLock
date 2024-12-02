using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLev3 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AdvancedSceneTransition.Instance.LoadScene("Level 3", AdvancedSceneTransition.TransitionType.Fade);
            //SceneManager.LoadScene("Test_tut2");
        }
    }
}
