using UnityEngine;

public class MaterialSwapOnHover : MonoBehaviour
{
    private Renderer objectRenderer;
    public Material normalMaterial; // Original material
    public Material hoverMaterial;  // Material to swap to when hovering

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        objectRenderer = GetComponent<Renderer>();
        // Store the initial material if not set in inspector
        if (normalMaterial == null)
        {
            normalMaterial = objectRenderer.material;
        }
    }

    void OnMouseEnter()
    {
        if (hoverMaterial != null)
        {
            objectRenderer.material = hoverMaterial;
        }
    }

    void OnMouseExit()
    {
        if (normalMaterial != null)
        {
            objectRenderer.material = normalMaterial;
        }
    }
}