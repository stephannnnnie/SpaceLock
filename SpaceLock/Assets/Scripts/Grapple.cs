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
    private float grappleSpeed;
    private Vector3 initialPosition;
    private float elapsedTime;
    public int maxGrapples = 5;
    public int remainingGrapples;
    public Canvas cv;
    private TextMeshProUGUI GrappleCount;

    //public GameObject finalWall;
    private bool hasWon = false;

    public float wiggleFrequency =9f;
    public float wiggleMagnitude = 0.5f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing.");
        }
        lineRenderer.enabled = false;

        remainingGrapples = maxGrapples;
       
        GrappleCount = cv.GrapplesNumber.GetComponent<TextMeshProUGUI>() ;
        UpdateGrappleCountText();
    }

    void Update()
    {
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetButtonDown("Fire1") && remainingGrapples > 0)
        {
            TryGrapple();
        }

        if (isGrappling && grappledObject != null)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.MoveTowards(initialPosition, grappledObject.position, grappleSpeed * elapsedTime);
            // lineRenderer.SetPosition(0, Shootposi.transform.position);
            // lineRenderer.SetPosition(1, grappledObject.position);
            UpdateLineRendererWithWiggle();

            if (elapsedTime >= grappleTime || Vector3.Distance(transform.position, grappledObject.position) < 0.1f)
            {
                isGrappling = false;
                lineRenderer.enabled = false;
                transform.SetParent(grappledObject);
                cv.updateGrappless();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (remainingGrapples == 0 && !hasWon)
        {
            cv.PlayerLose();
            Invoke("RestartGame", 2f);
        }
    }

    void UpdateLineRendererWithWiggle()
    {
        if (lineRenderer != null && grappledObject != null)
        {
            lineRenderer.positionCount = 50;
            Vector3 startPoint = Shootposi.transform.position;
            Vector3 endPoint = grappledObject.position;

            lineRenderer.SetPosition(0, startPoint);

            for (int i = 1; i < lineRenderer.positionCount; i++)
            {
                float t = (float)i / (lineRenderer.positionCount - 1);
                Vector3 basePosition = Vector3.Lerp(startPoint, endPoint, t);

                float wiggleOffset = Mathf.Sin(t * wiggleFrequency + elapsedTime * wiggleFrequency) * wiggleMagnitude * Mathf.Pow((1 - t), 2);
                Vector3 offset = Vector3.Cross((endPoint - startPoint).normalized, Vector3.up) * wiggleOffset;

                lineRenderer.SetPosition(i, basePosition + offset);
            }
        }
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
                    Debug.Log("entered most inner else");
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
            Debug.Log("entered most outer else");
            //RedCircleWarning();
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

        if (GrappleCount != null)
        {
            GrappleCount.text = "Grapples Remaining: " + remainingGrapples + "\nGrapple Distance: " + maxGrappleDistance;
        }
    }

    void RedCircleWarning()
    {
        Debug.Log("called red circle warning");
        Transform circleTransform = transform.Find("GrappleDistanceCircle");

        if (circleTransform != null)
        {
            circleTransform.GetComponent<LineRenderer>().enabled = true;
            circleTransform.gameObject.GetComponent<GrappleRangeCircle>().ShowRedCircle(maxGrappleDistance, 0.5f);
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
