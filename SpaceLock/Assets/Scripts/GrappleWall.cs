using UnityEngine;

public class GrappleWall : MonoBehaviour {
    public GameObject player;
    public Material inRangematerial;
    public Material outRangematerial;

    void Update() {

        Grapple grappleScript = player.GetComponent<Grapple>();
        if (grappleScript == null) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the raycast hits this object (the wall)
        if (Physics.Raycast(ray, out hit, grappleScript.maxGrappleDistance))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                this.GetComponent<Renderer>().material = inRangematerial;
            }
            else
            {
                this.GetComponent<Renderer>().material = outRangematerial;
            }
        }
        else
        {
            this.GetComponent<Renderer>().material = outRangematerial;
        }
    }
}
