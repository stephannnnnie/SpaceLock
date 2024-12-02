using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLev1 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AdvancedSceneTransition.Instance.LoadScene("Level 1", AdvancedSceneTransition.TransitionType.Fade);
            //SceneManager.LoadScene("Level 1");
        }
    }
}
