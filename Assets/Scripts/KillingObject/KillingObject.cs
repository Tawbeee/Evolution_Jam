using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class KillingObject : MonoBehaviour
{
    [SerializeField] public KillingObjectType killingObjectType;
     protected Collider objectCollider;

     public AK.Wwise.Event KilledSound;
     public AK.Wwise.Event DeathSound;



    public virtual void Start()
    {
        objectCollider = GetComponent<Collider>();
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            if (PlayerState.Instance.isInvincible) return;

            if (PlayerState.Instance.playerState != killingObjectType)
            {
                PlayerEvents.Kill(killingObjectType);
                KilledSound.Post(gameObject);
               if(DeathSound != null) DeathSound.Post(gameObject);


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
    Gravity,
    Poison,
    Arrow,
    Electricity,
    Freeze,
    Stomp,
}
