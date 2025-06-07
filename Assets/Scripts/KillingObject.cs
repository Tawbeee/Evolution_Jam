using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class KillingObject : MonoBehaviour
{
    [SerializeField] public KillingObjectType killingObjectType;
    private Collider Collider;

    private void Start()
    {
        Collider = GetComponent<Collider>();
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

    private void Update()
    {
        if (PlayerState.Instance.playerState == killingObjectType)
        {
            Collider.isTrigger = false;
        }
        else
        {
            Collider.isTrigger = true;
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
