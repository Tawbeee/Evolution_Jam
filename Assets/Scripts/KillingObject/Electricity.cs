using UnityEngine;


using System.Collections;

public class Electricity : KillingObject
{
    [SerializeField] private GameObject thunder;
    public AK.Wwise.Event IsKilledByLightning;

    public override void OnTriggerEnter(Collider other)
    {
        StartCoroutine(HandleThunderAndKill(other));
    }

    private IEnumerator HandleThunderAndKill(Collider other)
    {   
        PlayerState.Instance.transform.position = PlayerState.Instance.SpawnPoint.position;
        if (PlayerState.Instance.playerState != killingObjectType)
        {
            thunder.SetActive(false);
            thunder.SetActive(true);
        }
        // Active le thunder visuellement (r�activation pour rejouer l'effet)
        

        // Attendre 0.2 seconde
        yield return new WaitForSeconds(0.2f);

        // V�rifie l'�tat du joueur
        if (other.CompareTag("Player"))
        {
            if (PlayerState.Instance.playerState != killingObjectType)
            {   
                
                IsKilledByLightning.Post(gameObject);
                PlayerEvents.Kill(killingObjectType);
                

            }
        }
    }
}


