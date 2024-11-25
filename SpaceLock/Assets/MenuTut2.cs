using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTut2 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AdvancedSceneTransition.Instance.LoadScene("Test_tut2", AdvancedSceneTransition.TransitionType.Fade);
            //SceneManager.LoadScene("Test_tut2");
        }
    }
}
