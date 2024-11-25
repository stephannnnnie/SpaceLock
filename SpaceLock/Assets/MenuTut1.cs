using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTut1 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AdvancedSceneTransition.Instance.LoadScene("Test_tut", AdvancedSceneTransition.TransitionType.Fade);
            //SceneManager.LoadScene("Test_tut");
        }
    }
}
