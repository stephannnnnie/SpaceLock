using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextAnimation : MonoBehaviour
{
    public float floatSpeed = 2f;  // Speed of floating
    public float fadeDuration = 1f; // Duration for fading out

    private TextMeshProUGUI floatingText;
    private Color startColor;
    
    void Start()
    {
        floatingText = GetComponent<TextMeshProUGUI>();
        if (floatingText != null)
        {
            startColor = floatingText.color;
            floatingText.color = new Color(startColor.r, startColor.g, startColor.b, 0); // Start invisible
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component missing.");
        }
    }


    public void PlayFloatingText(string text)
    {
        if (floatingText == null)
        {
            floatingText = GetComponent<TextMeshProUGUI>();
            if (floatingText != null)
            {
                startColor = floatingText.color;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component missing.");
                return;
            }
        }

        floatingText.text = text;
        floatingText.color = startColor; // Make text visible
        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        Vector3 startPos = transform.position;
        float timer = 0;

        while (timer < fadeDuration)
        {
            transform.position = startPos + Vector3.up * floatSpeed * (timer / fadeDuration);
            floatingText.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1, 0, timer / fadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        // Set the text color back to transparent instead of deactivating the GameObject
        floatingText.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}
