using UnityEngine;

public class KillingObject : MonoBehaviour
{
    [SerializeField] public KillingObjectType killingObjectType;

    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(PlayerState.Instance.playerState != killingObjectType)
            {
                PlayerEvents.Kill(killingObjectType);
            }
                      
        }
    }
    


}

public enum KillingObjectType
{
    None, 
    Spike,
    Fire,
    Water,
    Fall,
    Poison,
    Arrow,
    Electricity,
    Freeze,
}
