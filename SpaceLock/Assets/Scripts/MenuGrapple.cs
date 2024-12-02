using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;

public class MenuGrapple : MonoBehaviour
{


    public float maxGrappleDistance = 30f;
    public GameObject Shootposi;
    private Transform grappledObject;
    public bool isGrappling { get; set; }
    public GrappleGun gun;
    private LineRenderer lineRenderer;
    //private float grappleTime = 1.0f;
    [SerializeField] float grappleSpeed = 10f; // Adjust this value to control grappling speed
    private Vector3 initialPosition;
    private float elapsedTime;
    public int maxGrapples = 5;
    public int remainingGrapples;
    public MenuCanvas cv;
    private TextMeshProUGUI GrappleCount;

    private Vector3 grapplePoint;
    private Vector3 grappleDirection;

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

        cv.UpdateGrappleNumber(remainingGrapples, maxGrappleDistance);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && remainingGrapples >= 0 && !lineRenderer.enabled)
        {
            TryGrapple();
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
            if (Vector3.Distance(transform.position, grapplePoint) < 0.01f)
            {
                EndGrapple();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null && grappledObject != null)
        {
            Vector3 startPoint = Shootposi.transform.position;
            Vector3 endPoint = grapplePoint;

            lineRenderer.SetPosition(0, startPoint);

        }
    }

    private IEnumerator AnimateGrapple()
    {
        float animationDuration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            Vector3 startPoint = Shootposi.transform.position;
            Vector3 currentEndPoint = Vector3.Lerp(startPoint, grappledObject.position, t);

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
                    gun.StartGrapple(grapplePoint);
                    initialPosition = transform.position;
                    elapsedTime = 0f;
                    StartCoroutine(AnimateGrapple());
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

    void EndGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
        transform.SetParent(grappledObject);
        cv.updateGrappless();
        gun.StopGrapple();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGrappling && collision.gameObject.CompareTag("Obstacle"))
        {
            isGrappling = false;
            lineRenderer.enabled = false;
            transform.SetParent(collision.transform);
        }
        Debug.Log("enter tutorial box");
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            transform.SetParent(null);
        }
        Debug.Log("exit tutorial box");
    }

    public void UpdateGrappleCountText()
    {
        cv.UpdateGrappleNumber(remainingGrapples, maxGrappleDistance);
    }
}