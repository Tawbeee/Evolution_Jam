using UnityEngine;

public class Arrow : KillingObject
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerState.Instance.playerState != killingObjectType)
            {
                PlayerEvents.Kill(killingObjectType);
                PlayerState.Instance.SwitchMovement();

        }
        }

    }
}