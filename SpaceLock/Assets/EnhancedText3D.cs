using UnityEngine;
using TMPro;

public class EnhancedText3D : MonoBehaviour
{
    [Header("References")]
    private TextMeshPro textMeshPro;
    private Camera mainCamera;

    [Header("Animation Settings")]
    public bool faceCamera = true;
    public bool hoverEffect = true;
    public bool scaleWithDistance = true;

    [Header("Hover Settings")]
    public float hoverAmount = 0.1f;
    public float hoverSpeed = 1f;

    [Header("Scale Settings")]
    public float minScale = 0.5f;
    public float maxScale = 2f;
    public float scaleDistance = 10f;

    private Vector3 startPos;

    void Start()
    {
        // Get references
        mainCamera = Camera.main;
        textMeshPro = GetComponent<TextMeshPro>();
        startPos = transform.position;

        if (textMeshPro == null)
        {
            Debug.LogError("No TextMeshPro component found on this object!");
            return;
        }

        // Set up basic appearance
        SetupTextAppearance();
    }

    void SetupTextAppearance()
    {
        // Add outline
        textMeshPro.outlineWidth = 0.2f;
        textMeshPro.outlineColor = Color.black;

        // Make sure the material renders properly in 3D space
        textMeshPro.renderer.material.renderQueue = 3000; // This will make it render after opaque objects

        // Enable shadows if you want them
        textMeshPro.renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        textMeshPro.renderer.receiveShadows = true;
    }

    void Update()
    {
        if (textMeshPro == null || mainCamera == null) return;

        // Face camera
        if (faceCamera)
        {
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }

        // Add hover effect
        if (hoverEffect)
        {
            float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
            transform.position = startPos + Vector3.up * hover;
        }

        // Scale based on distance
        if (scaleWithDistance)
        {
            float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
            float scale = Mathf.Lerp(maxScale, minScale, distance / scaleDistance);
            transform.localScale = Vector3.one * scale;
        }
    }

    // Call this if you need to reset the start position
    public void ResetStartPosition()
    {
        startPos = transform.position;
    }
}