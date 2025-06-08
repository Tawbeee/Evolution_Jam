using UnityEngine;

[System.Serializable]
public class PoisonEffectManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializePoisonEffect()
    {
        // Créer automatiquement le système d'effet poison
        GameObject poisonEffectGO = new GameObject("PoisonEffectManager");
        poisonEffectGO.AddComponent<PoisonScreenEffect>();
        
        // Persister entre les scènes
        DontDestroyOnLoad(poisonEffectGO);
    }
} 