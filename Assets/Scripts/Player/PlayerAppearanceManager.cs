using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearanceManager : MonoBehaviour
{
    [Header("List of Appearances by Type")]
    [SerializeField] private List<Appearance> appearances;

    [Header("Renderer cible")]
    [SerializeField] private Renderer targetRenderer; // Le renderer du joueur (MeshRenderer ou SkinnedMeshRenderer)
    [SerializeField] private Transform fxSpawnPoint;  // Où instancier les FX (souvent transform du joueur)

    private void OnEnable()
    {
        PlayerEvents.OnKilled += OnPlayerKilled;
    }

    private void OnDisable()
    {
        PlayerEvents.OnKilled -= OnPlayerKilled;
    }

    private void OnPlayerKilled(KillingObjectType type)
    {
        // Recherche l'apparence correspondante
        Appearance match = appearances.Find(a => a.type == type);

        if (match != null)
        {
            // Appliquer le nouveau matériau
            if (targetRenderer != null && match.material != null)
            {
                targetRenderer.material = match.material;
            }

            // Supprimer les anciens FX enfants
            if (fxSpawnPoint != null)
            {
                foreach (Transform child in fxSpawnPoint)
                {
                    Destroy(child.gameObject);
                }
            }

            // Instancier le nouvel effet FX
            if (match.fxPrefab != null && fxSpawnPoint != null)
            {
                GameObject fx = Instantiate(match.fxPrefab, fxSpawnPoint.position, Quaternion.identity);
                fx.transform.SetParent(fxSpawnPoint); // Pour suivre le joueur
            }
        }
    }

}


[System.Serializable]
public class Appearance
{
    public KillingObjectType type;
    public Material material;
    public GameObject fxPrefab;
}
