using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLev2 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AdvancedSceneTransition.Instance.LoadScene("Level 2", AdvancedSceneTransition.TransitionType.Fade);
            //SceneManager.LoadScene("Level 2");
        }
    }
}
