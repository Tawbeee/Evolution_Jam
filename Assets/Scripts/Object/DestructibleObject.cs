using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Collider objectCollider;
    [SerializeField] private KillingObjectType killedBy;

    public AK.Wwise.Event DestructedSound;

    private void Start()
    {
        objectCollider = GetComponent<Collider>();
        if (objectCollider == null)
        {
            Debug.LogError("Spikeable script requires a Collider component.");
        }
    }

    private void Update()
    {
        if (PlayerState.Instance.playerState == killedBy)
        {
            objectCollider.isTrigger = true;
        }
        else
        {
            objectCollider.isTrigger = false;
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
