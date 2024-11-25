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
        SlideLeft,
        SlideRight
    }

    private RectTransform transitionRect;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            transitionRect = transitionImage.GetComponent<RectTransform>();

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
        // Reset position and enable image before transition
        transitionImage.enabled = true;
        fadeCanvasGroup.alpha = 1;

        // Set initial position for slide transitions
        if (transitionType == TransitionType.SlideLeft || transitionType == TransitionType.SlideRight)
        {
            // Position the panel outside the screen
            float startX = (transitionType == TransitionType.SlideRight) ? -Screen.width : Screen.width;
            transitionRect.anchoredPosition = new Vector2(startX, 0);
        }

        // Transition out
        yield return StartCoroutine(TransitionOut());

        // Load the scene
        SceneManager.LoadScene(sceneName);

        // Transition in
        yield return StartCoroutine(TransitionIn());

        // Cleanup after transition
        transitionImage.enabled = false;
        fadeCanvasGroup.alpha = 0;
    }

    private IEnumerator TransitionOut()
    {
        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return FadeOut();
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
        fadeCanvasGroup.alpha = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = elapsedTime / transitionDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        fadeCanvasGroup.alpha = 1;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = 1 - (elapsedTime / transitionDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
    }

    private IEnumerator CrossFadeOut()
    {
        // Create a screenshot of the current scene
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Create a new temporary image for the screenshot
        GameObject tempImage = new GameObject("Screenshot");
        tempImage.transform.SetParent(transform);
        Image screenshotImage = tempImage.AddComponent<Image>();
        screenshotImage.sprite = Sprite.Create(screenshot,
            new Rect(0, 0, screenshot.width, screenshot.height),
            new Vector2(0.5f, 0.5f));

        // Setup the screenshot image
        RectTransform rectTransform = tempImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        // Fade between the screenshot and the black overlay
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            screenshotImage.color = new Color(1, 1, 1, 1 - t);
            fadeCanvasGroup.alpha = t;
            yield return null;
        }

        Destroy(tempImage);
    }

    private IEnumerator SlideOut(int direction)
    {
        float elapsedTime = 0f;
        float startX = (direction == 1) ? -Screen.width : Screen.width;
        float endX = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            float currentX = Mathf.Lerp(startX, endX, t);
            transitionRect.anchoredPosition = new Vector2(currentX, 0);
            yield return null;
        }
        transitionRect.anchoredPosition = new Vector2(endX, 0);
    }

    private IEnumerator SlideIn(int direction)
    {
        float elapsedTime = 0f;
        float startX = 0;
        float endX = (direction == 1) ? Screen.width : -Screen.width;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            float currentX = Mathf.Lerp(startX, endX, t);
            transitionRect.anchoredPosition = new Vector2(currentX, 0);
            yield return null;
        }
        transitionRect.anchoredPosition = new Vector2(endX, 0);
    }
}