using UnityEngine;

public class WaterKillingObject : KillingObject
{
    
    public void Update()
    {
        if (PlayerState.Instance.playerState == killingObjectType)
        {
            objectCollider.isTrigger = false;
        }
        else
        {
            objectCollider.isTrigger = true;
        }
    }

    // D�sactive la m�thode de base OnTriggerEnter



}
