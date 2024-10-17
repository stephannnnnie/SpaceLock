using UnityEngine;

public class ObstacleSpeed : MonoBehaviour
{
    private float minSpeed = 1.5f;
    private float speed;
    private GameObject player;
    public Material farObstacle;
    public Material nearObstacle;
    private Grapple gp;

    void Start()
    {

      player = GameObject.FindWithTag("Player");
      if (player == null) {
          Debug.LogError("Player1 tag not found! Make sure your player object has the correct tag.");
          return;
      }
      gp = player.GetComponent<Grapple>();
      Vector3 scale = transform.localScale;

      float scaleFactor = (scale.x + scale.y + scale.z) / 3f;

      speed =  minSpeed * scaleFactor;
    }

    void Update()
    {
      transform.Translate(speed * Time.deltaTime * Vector3.right);

      float distance = Vector3.Distance(player.transform.position, transform.position);

      if (distance < gp.maxGrappleDistance) {
          GetComponent<Renderer>().material = nearObstacle;
      } else {
          GetComponent<Renderer>().material = farObstacle;
      }

    }
}
