using UnityEngine;

public class GrappleWall : MonoBehaviour {
    public float GrappleDistance; 
    public Material inRangematerial;
    public Material outRangematerial;
   
    void Update() {
      
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Check if the raycast hits this object (the wall)
        if (Physics.Raycast(ray, out hit, GrappleDistance ))
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
