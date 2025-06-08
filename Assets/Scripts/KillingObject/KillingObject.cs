using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class KillingObject : MonoBehaviour
{
    [SerializeField] public KillingObjectType killingObjectType;
     protected Collider objectCollider;

    public virtual void Start()
    {
        objectCollider = GetComponent<Collider>();
    }


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
    Stomp,
}
