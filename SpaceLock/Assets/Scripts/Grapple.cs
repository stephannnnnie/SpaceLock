using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Grapple : MonoBehaviour {


    public float maxGrappleDistance = 30f;
    private Transform grappledObject;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;
    private float grappleTime = 1.0f;
    private float grappleSpeed;
    private Vector3 initialPosition;
    private float elapsedTime;
    public int maxGrapples = 5;
    private int remainingGrapples;
    public TextMeshProUGUI grappleCountText;
    public Image gameOverImage;
    public Image gameWinImage;
    public GameObject finalWall;
    private bool hasWon = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing.");
        }
        lineRenderer.enabled = false;

        remainingGrapples = maxGrapples;
        gameOverImage.enabled = false;
        gameWinImage.enabled = false;
        Cursor.visible = false;
        UpdateGrappleCountText();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && remainingGrapples > 0)
        {
            TryGrapple();
        }

        if (isGrappling && grappledObject != null)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.MoveTowards(initialPosition, grappledObject.position, grappleSpeed * elapsedTime);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grappledObject.position);

            if (elapsedTime >= grappleTime || Vector3.Distance(transform.position, grappledObject.position) < 0.1f)
            {
                isGrappling = false;
                lineRenderer.enabled = false;
                transform.SetParent(grappledObject);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (remainingGrapples == 0 && !hasWon)
        {
            gameOverImage.enabled = true;
            grappleCountText.enabled = false;
            Invoke("RestartGame", 2f);
        }
    }

    void TryGrapple()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxGrappleDistance))
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.CompareTag("Obstacle"))
            {
                float distanceToHit = Vector3.Distance(transform.position, hit.point);
                Debug.Log("Distance to hit: " + distanceToHit);

                if (distanceToHit <= maxGrappleDistance)
                {
                    grappledObject = hit.collider.transform;
                    isGrappling = true;
                    lineRenderer.enabled = true;

                    initialPosition = transform.position;
                    elapsedTime = 0f;

                    grappleSpeed = distanceToHit / grappleTime;

                    remainingGrapples--;
                    UpdateGrappleCountText();

                    Debug.Log("Grappling to object: " + hit.collider.gameObject.name);
                }
                else
                {
                    Debug.Log("Object is too far to grapple.");
                }
            }
            else
            {
                Debug.Log("Hit object is not a valid obstacle.");
            }
        }
        else
        {
            Debug.Log("No object hit within grapple distance.");
        }
    }

    void FixedUpdate()
    {
        if (isGrappling && grappledObject != null)
        {
            transform.position = grappledObject.position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGrappling && collision.gameObject.CompareTag("Obstacle"))
        {
            isGrappling = false;
            lineRenderer.enabled = false;
            transform.SetParent(collision.transform);
        }

        if (collision.gameObject == finalWall)
        {
            WinGame();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            transform.SetParent(null);
        }
    }

    void UpdateGrappleCountText()
    {
        if (grappleCountText != null)
        {
            grappleCountText.text = "Grapples Remaining: " + remainingGrapples;
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void WinGame()
    {
        hasWon = true;
        gameWinImage.enabled = true;
        grappleCountText.enabled = false;
    }
}
