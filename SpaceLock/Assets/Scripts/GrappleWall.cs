using UnityEngine;

public class GrappleWall : MonoBehaviour {
    public GameObject player;
    public Material inRangematerial;
    public Material outRangematerial;
   
    void Update() {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance <= player.GetComponent<Grapple>().maxGrappleDistance) {
            this.GetComponent<Renderer>().material = inRangematerial;
        } else {

            this.GetComponent<Renderer>().material = outRangematerial;
        }
    }
}
