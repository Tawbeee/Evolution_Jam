using UnityEngine;


using System.Collections;

public class Electricity : KillingObject
{
    [SerializeField] private GameObject thunder;

    public override void OnTriggerEnter(Collider other)
    {
        StartCoroutine(HandleThunderAndKill(other));
    }

    private IEnumerator HandleThunderAndKill(Collider other)
    {
        if (PlayerState.Instance.playerState != killingObjectType)
        {
            thunder.SetActive(false);
            thunder.SetActive(true);
        }
        // Active le thunder visuellement (r�activation pour rejouer l'effet)
        

        // Attendre 0.2 seconde
        yield return new WaitForSeconds(0f);

        // V�rifie l'�tat du joueur
        if (other.CompareTag("Player"))
        {
            if (PlayerState.Instance.playerState != killingObjectType)
            {
                PlayerEvents.Kill(killingObjectType);
            }
        }
    }
}


