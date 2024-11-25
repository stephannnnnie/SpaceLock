using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AdvancedSceneTransition : MonoBehaviour
{
    public static AdvancedSceneTransition Instance;

    [Header("References")]
    public CanvasGroup fadeCanvasGroup;
    public Image transitionImage;

    [Header("Settings")]
    public float transitionDuration = 1f;
    public TransitionType transitionType = TransitionType.Fade;

    public enum TransitionType
    {
        Fade,
        CrossFade,
        CircleWipe,
        SlideLeft,
        SlideRight
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Make sure image is disabled at start
            if (transitionImage != null)
            {
                transitionImage.enabled = false;
                fadeCanvasGroup.alpha = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, TransitionType type = TransitionType.Fade)
    {
        transitionType = type;
        StartCoroutine(TransitionAndLoadScene(sceneName));
    }

    private IEnumerator TransitionAndLoadScene(string sceneName)
    {
        // Enable image before transition starts
        transitionImage.enabled = true;

        // Transition out
        yield return StartCoroutine(TransitionOut());

        // Load the scene
        SceneManager.LoadScene(sceneName);

        // Transition in
        yield return StartCoroutine(TransitionIn());

        // Disable image after transition is complete
        transitionImage.enabled = false;
    }

    private IEnumerator TransitionOut()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return FadeOut();
                break;
            case TransitionType.CrossFade:
                yield return CrossFadeOut();
                break;
            case TransitionType.CircleWipe:
                yield return CircleWipeOut();
                break;
            case TransitionType.SlideLeft:
                yield return SlideOut(-1);
                break;
            case TransitionType.SlideRight:
                yield return SlideOut(1);
                break;
        }
    }

    private IEnumerator TransitionIn()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return FadeIn();
                break;
            case TransitionType.CrossFade:
                yield return CrossFadeIn();
                break;
            case TransitionType.CircleWipe:
                yield return CircleWipeIn();
                break;
            case TransitionType.SlideLeft:
                yield return SlideIn(-1);
                break;
            case TransitionType.SlideRight:
                yield return SlideIn(1);
                break;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = elapsedTime / transitionDuration;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - (elapsedTime / transitionDuration);
            yield return null;
        }
    }

    private IEnumerator CrossFadeOut()
    {
        // Similar to fade but with a different shader effect
        yield return FadeOut();
    }

    private IEnumerator CrossFadeIn()
    {
        yield return FadeIn();
    }

    private IEnumerator CircleWipeOut()
    {
        // Implement circle wipe using shader or masked image
        yield return null;
    }

    private IEnumerator CircleWipeIn()
    {
        yield return null;
    }

    private IEnumerator SlideOut(int direction)
    {
        RectTransform rect = transitionImage.rectTransform;
        float startPos = -Screen.width * direction;
        float endPos = 0;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            rect.anchoredPosition = new Vector2(Mathf.Lerp(startPos, endPos, t), 0);
            yield return null;
        }
    }

    private IEnumerator SlideIn(int direction)
    {
        RectTransform rect = transitionImage.rectTransform;
        float startPos = 0;
        float endPos = Screen.width * direction;

        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            rect.anchoredPosition = new Vector2(Mathf.Lerp(startPos, endPos, t), 0);
            yield return null;
        }
    }
}