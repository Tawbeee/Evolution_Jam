using UnityEngine;

public class Spike : KillingObject
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
}
