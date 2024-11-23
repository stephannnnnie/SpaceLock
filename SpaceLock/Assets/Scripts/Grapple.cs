using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class Grapple : MonoBehaviour {


    public float maxGrappleDistance = 30f;
    public GameObject Shootposi;
    private Transform grappledObject;
    private bool isGrappling = false;
    private LineRenderer lineRenderer;
    private float grappleTime = 1.0f;
    [SerializeField] float grappleSpeed = 10f; // Adjust this value to control grappling speed
    private Vector3 initialPosition;
    private float elapsedTime;
    public int maxGrapples = 5;
    public int remainingGrapples;
    public Canvas cv;
    private TextMeshProUGUI GrappleCount;

    private bool hasWon = false;
    private bool firstGrappleCompleted = false;

    // Removed wiggle variables
    // public float wiggleFrequency = 9f; 
    // public float wiggleMagnitude = 0.5f;

    private bool redShown;

    private Vector3 grapplePoint;
    private Vector3 grappleDirection;

    public RectTransform progressBarFill;
    public Transform frontWall;
    public Transform backWall;
    public float maxProgressWidth = 95f;
    public ScreenFlickerController screenFlickerController;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing.");
        }
        lineRenderer.enabled = false;

        remainingGrapples = maxGrapples;
        UpdateGrappleCountText();

        redShown = false;

        cv.UpdateGrappleNumber(remainingGrapples, maxGrappleDistance);
    }

    void Update()
    {
        if (progressBarFill != null) { UpdateProgressBar(); }

        if (Input.GetButtonDown("Fire1") && remainingGrapples >= 0 && !lineRenderer.enabled)
        {
            if (remainingGrapples == 0 && !hasWon)
            {
                if (screenFlickerController != null) { screenFlickerController.StopFlickering(); }
                cv.PlayerLose(2);
                Invoke("RestartGame", 2f);
            }
            else
            {
                TryGrapple();
            }
        }

        if (redShown && Input.GetButtonUp("Fire1"))
        {
            GetComponentInChildren<GrappleRangeCircle>().HideRedCircle();
            redShown = false;
        }

        if (isGrappling && grappledObject != null)
        {

            UpdateLineRenderer();
            elapsedTime += Time.deltaTime;

            // Update grapple point if the target is moving
            grapplePoint = grappledObject.position;

            // Move towards the updated grapple point
            float step = grappleSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, step);


            // Check if we've reached the grapple point
            if (Vector3.Distance(transform.position, grapplePoint) < 0.1f)
            {
                if (!firstGrappleCompleted && screenFlickerController != null)
                {
                    screenFlickerController.TriggerFirstGrapple();
                    firstGrappleCompleted = true;
                }
                EndGrapple();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void UpdateProgressBar()
    {
        float progress = Mathf.InverseLerp(backWall.position.x, frontWall.position.x, transform.position.x);
        progressBarFill.sizeDelta = new Vector2(maxProgressWidth * progress, progressBarFill.sizeDelta.y);
        progressBarFill.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, progress);
    }

/*    void UpdateLineRenderer()
    {
        if (lineRenderer != null && grappledObject != null)
        {
            lineRenderer.positionCount = 2; // Only need two points for a straight line
            Vector3 startPoint = Shootposi.transform.position;
            Vector3 endPoint = grapplePoint;

            lineRenderer.SetPosition(0, startPoint); // Set starting point
            lineRenderer.SetPosition(1, endPoint);   // Set ending point

            // Commented out the wiggle code
            *//*
            for (int i = 1; i < lineRenderer.positionCount; i++)
            {
                float t = (float)i / (lineRenderer.positionCount - 1);
                Vector3 basePosition = Vector3.Lerp(startPoint, endPoint, t);
                
                float wiggleOffset = Mathf.Sin(t * wiggleFrequency + elapsedTime * wiggleFrequency) * wiggleMagnitude * Mathf.Pow((1 - t), 2);
                Vector3 offset = Vector3.Cross((endPoint - startPoint).normalized, Vector3.up) * wiggleOffset;

                lineRenderer.SetPosition(i, basePosition + offset);
            }
            *//*
        }
    }*/

    void UpdateLineRenderer()
    {
        if (lineRenderer != null && grappledObject != null)
        {
            Vector3 startPoint = Shootposi.transform.position;
            Vector3 endPoint = grapplePoint;

            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }

    private IEnumerator AnimateGrapple(Vector3 endPoint)
    {
        float animationDuration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            Vector3 startPoint = Shootposi.transform.position;
            Vector3 currentEndPoint = Vector3.Lerp(startPoint, endPoint, t);

            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, currentEndPoint);

            yield return null;
        }

        // Ensure the final position is set correctly
        UpdateLineRenderer();
    }
    void TryGrapple()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.CompareTag("Obstacle"))
            {
                float distanceToHit = Vector3.Distance(transform.position, hit.point);

                if (distanceToHit <= maxGrappleDistance)
                {
                    this.transform.parent = null;
                    grappledObject = hit.collider.transform;
                    grapplePoint = hit.point;
                    isGrappling = true;
                    lineRenderer.enabled = true;

                    initialPosition = transform.position;
                    elapsedTime = 0f;
                    StartCoroutine(AnimateGrapple(grapplePoint));
                    remainingGrapples--;
                    UpdateGrappleCountText();
                    Debug.Log("Grappling to object: " + hit.collider.gameObject.name);
                }
                else
                {
                    RedCircleWarning();
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

    void EndGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
        transform.SetParent(grappledObject);
        cv.updateGrappless();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGrappling && collision.gameObject.CompareTag("Obstacle"))
        {
            isGrappling = false;
            lineRenderer.enabled = false;
            transform.SetParent(collision.transform);
        }

        if (collision.gameObject.tag == "FinalWall")
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

    public void UpdateGrappleCountText()
    {
        if (remainingGrapples >= 20)
        {
            remainingGrapples = 20;
        }

        cv.UpdateGrappleNumber(remainingGrapples, maxGrappleDistance);
    }

    void RedCircleWarning()
    {
        Debug.Log("called red circle warning");
        Transform circleTransform = transform.Find("GrappleDistanceCircle");

        if (circleTransform != null)
        {
            circleTransform.GetComponent<LineRenderer>().enabled = true;
            circleTransform.gameObject.GetComponent<GrappleRangeCircle>().ShowRedCircle(maxGrappleDistance);
            redShown = true;
        }
        else
        {
            Debug.LogError("GrappleDistanceCircle child not found under the player.");
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void WinGame()
    {
        cv.PlayerWon();
    }
}