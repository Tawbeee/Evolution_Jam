using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public GameObject shadow;
    public RaycastHit hit;
    public float offset;

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Set the shadow position to the hit point with an offset
            shadow.transform.position = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            shadow.SetActive(true);
        }
        else
        {
            // Hide the shadow if no ground is detected
            shadow.SetActive(false);
        }
    }
}

