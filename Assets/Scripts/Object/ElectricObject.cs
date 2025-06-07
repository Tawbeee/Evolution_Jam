using UnityEngine;

public class ElectricObject : MonoBehaviour
{
    [SerializeField] private Transform movingObject;
    
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 3f;

    private bool moveToB = false;

    private void Update()
    {
        if (moveToB)
        {
            movingObject.position = Vector3.MoveTowards(movingObject.position, pointB.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered electric zone. State: " + PlayerState.Instance.playerState);
            if (PlayerState.Instance.playerState == KillingObjectType.Electricity)
            {
                Debug.Log("Activating movement to point B.");
                moveToB = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited electric zone.");
            moveToB = false;
        }
    }
}
