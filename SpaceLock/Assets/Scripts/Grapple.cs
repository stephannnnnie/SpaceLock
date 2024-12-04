using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Grapple : MonoBehaviour
{
    public float maxGrappleDistance = 30f;
    public GameObject Shootposi;
    private Transform grappledObject;
    public bool isGrappling { get; set; }
    public GrappleGun gun;
    private LineRenderer lineRenderer;
    [SerializeField] float grappleSpeed = 10f;
    private Vector3 initialPosition;
    private float elapsedTime;
    public int maxGrapples = 5;
    public int remainingGrapples;
    public Canvas cv;

    public int segmentCount = 30;
    public float segmentLength = 0.2f;
    public float stiffness = 20f;
    public float damping = 0.5f;

    private Vector3[] ropeSegments;
    private Vector3[] velocities;

    private bool hasWon = false;
    private bool firstGrappleCompleted = false;
    private bool redShown;

    private Vector3 grapplePoint;

    public RectTransform progressBarFill;
    public Transform frontWall;
    public Transform backWall;
    public float maxProgressWidth = 95f;
    public ScreenFlickerController screenFlickerController;

    private float currentRopeLength;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing. Please add it to the GameObject.");
            return;
        }

        Material ropeMaterial = Resources.Load<Material>("Materials/Line");
        lineRenderer.material = ropeMaterial != null ? ropeMaterial : new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 0;

        ropeSegments = new Vector3[segmentCount];
        velocities = new Vector3[segmentCount];

        remainingGrapples = maxGrapples;
        UpdateGrappleCountText();

        if (cv != null)
        {
            cv.UpdateGrappleNumber(remainingGrapples, maxGrappleDistance);
        }
    }

    void LateUpdate()
    {
        if (progressBarFill != null)
        {
            UpdateProgressBar();
        }

        if (Input.GetButtonDown("Fire1") && remainingGrapples >= 0)
        {
            if (remainingGrapples == 0 && !hasWon)
            {
                screenFlickerController?.StopFlickering();
                cv?.PlayerLose(2);
                Invoke("RestartGame", 2f);
            }
            else
            {
                TryGrapple();
            }
        }

        if (redShown && Input.GetButtonUp("Fire1"))
        {
            GetComponentInChildren<GrappleRangeCircle>()?.HideRedCircle();
            redShown = false;
        }

        if (isGrappling && grappledObject != null)
        {
            SimulateRopePhysics();
            DrawRope();
            elapsedTime += Time.deltaTime;

            grapplePoint = grappledObject.position;

            float step = grappleSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, grapplePoint, step);

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
            lineRenderer.positionCount = 0;
        }
    }

    void UpdateProgressBar()
    {
        float progress = Mathf.InverseLerp(backWall.position.x, frontWall.position.x, transform.position.x);
        progressBarFill.sizeDelta = new Vector2(maxProgressWidth * progress, progressBarFill.sizeDelta.y);
        progressBarFill.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, progress);
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null && grappledObject != null)
        {
            int segments = 20;
            lineRenderer.positionCount = segments;

            Vector3 startPoint = Shootposi.transform.position;
            Vector3 endPoint = grapplePoint;

            for (int i = 0; i < segments; i++)
            {
                float t = (float)i / (segments - 1);
                Vector3 basePosition = Vector3.Lerp(startPoint, endPoint, t);

                float sagAmount = Mathf.Sin(t * Mathf.PI) * 2f;
                float oscillation = Mathf.Sin(elapsedTime * 5f + t * Mathf.PI) * 0.2f;
                Vector3 sag = Vector3.down * (sagAmount + oscillation);

                lineRenderer.SetPosition(i, basePosition + sag);
            }
        }
    }

    private IEnumerator AnimateGrapple()
    {
        float animationDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            for (int i = 0; i < segmentCount; i++)
            {
                float segmentT = (float)i / (segmentCount - 1);
                Vector3 basePosition = Vector3.Lerp(Shootposi.transform.position, grapplePoint, segmentT * t);
                float sagAmount = Mathf.Sin(segmentT * Mathf.PI) * (1f - t) * 2f;
                basePosition += Vector3.down * sagAmount;

                float waveMagnitude = Mathf.Sin(Time.time * 10f + segmentT * Mathf.PI) * 0.1f * (1f - t);
                basePosition += Vector3.up * waveMagnitude;

                ropeSegments[i] = basePosition;
            }

            DrawRope();
            yield return null;
        }

        for (int i = 0; i < segmentCount; i++)
        {
            ropeSegments[i] = Vector3.Lerp(Shootposi.transform.position, grapplePoint, (float)i / (segmentCount - 1));
        }

        DrawRope();
    }

    private float EaseOutElastic(float t)
    {
        if (t == 0f || t == 1f) return t;

        float p = 0.3f;
        return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - p / 4f) * (2f * Mathf.PI) / p) + 1f;
    }

    void TryGrapple()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.CompareTag("Obstacle"))
            {
                float distanceToHit = Vector3.Distance(transform.position, hit.point);

                if (hit.collider.transform == transform.parent)
                {
                    Debug.Log("Cannot grapple your own obstacle.");
                    return;
                }

                if (distanceToHit <= maxGrappleDistance)
                {
                    grappledObject = hit.collider.transform;
                    grapplePoint = hit.point;
                    isGrappling = true;
                    transform.SetParent(null);

                    gun.StartGrapple(grapplePoint);
                    initialPosition = transform.position;
                    elapsedTime = 0f;
                    remainingGrapples--;
                    UpdateGrappleCountText();

                    lineRenderer.enabled = true;
                    lineRenderer.positionCount = segmentCount;

                    currentRopeLength = distanceToHit;
                    for (int i = 0; i < segmentCount; i++)
                    {
                        ropeSegments[i] = Vector3.Lerp(Shootposi.transform.position, grapplePoint, (float)i / (segmentCount - 1));
                    }

                    DrawRope();
                    StartCoroutine(AnimateGrapple());
                }
                else
                {
                    RedCircleWarning();
                }
            }
        }
    }

    void EndGrapple()
    {
        isGrappling = false;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }

        if (grappledObject != null)
        {
            transform.SetParent(grappledObject);
            transform.position = grappledObject.position;
        }

        cv.updateGrappless();
        gun.StopGrapple();
    }

    void SimulateRopePhysics()
    {
        if (grappledObject == null) return;

        ropeSegments[0] = Shootposi.transform.position;

        float retractSpeed = 5f; // Speed of retraction
        currentRopeLength = Mathf.MoveTowards(currentRopeLength, 0f, retractSpeed * Time.deltaTime);

        float segmentLength = currentRopeLength / (segmentCount - 1);

        for (int i = 1; i < segmentCount; i++)
        {
            float t = (float)i / (segmentCount - 1);

            ropeSegments[i] = Vector3.Lerp(Shootposi.transform.position, grapplePoint, t);

            float wiggle = Mathf.Sin(Time.time * 10f + i * 0.5f) * 0.02f;
            ropeSegments[i] += Vector3.up * wiggle;
        }

        ropeSegments[segmentCount - 1] = Vector3.Lerp(grapplePoint, Shootposi.transform.position, 1f - (currentRopeLength / maxGrappleDistance));

        SmoothRopeSegments(segmentLength);
    }


    void SmoothRopeSegments(float segmentLength)
    {
        for (int i = 1; i < segmentCount - 1; i++)
        {
            Vector3 toPrev = ropeSegments[i - 1] - ropeSegments[i];
            Vector3 toNext = ropeSegments[i + 1] - ropeSegments[i];

            float prevDist = toPrev.magnitude;
            float nextDist = toNext.magnitude;

            if (prevDist > segmentLength)
                ropeSegments[i] += toPrev.normalized * (prevDist - segmentLength) * 0.5f;

            if (nextDist > segmentLength)
                ropeSegments[i] += toNext.normalized * (nextDist - segmentLength) * 0.5f;
        }
    }
    void DrawRope()
    {
        ropeSegments[0] = Shootposi.transform.position;

        Vector3[] smoothRope = new Vector3[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            smoothRope[i] = ropeSegments[i];
        }

        lineRenderer.positionCount = smoothRope.Length;
        lineRenderer.SetPositions(smoothRope);
    }


    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2 * p1) +
            (-p0 + p2) * t +
            (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
            (-p0 + 3 * p1 - 3 * p2 + p3) * t3
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGrappling && collision.gameObject.CompareTag("Obstacle"))
        {
            isGrappling = false;
            lineRenderer.enabled = false;

            transform.SetParent(collision.transform);
            transform.position = collision.transform.position;
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
            Debug.Log("Parent set to null now");
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