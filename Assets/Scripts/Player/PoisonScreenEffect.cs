using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoisonScreenEffect : MonoBehaviour
{
    public static PoisonScreenEffect Instance { get; private set; }
    
    [Header("Poison Effect")]
    [SerializeField] private Image poisonOverlay;
    [SerializeField] private Color poisonColor = new Color(1f, 0f, 0f, 0f); // Rouge transparent au début
    [SerializeField] private float maxAlpha = 0.5f; // Opacité maximale (ne pas complètement bloquer la vue)
    
    private Coroutine poisonEffectCoroutine;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupPoisonOverlay();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void SetupPoisonOverlay()
    {
        // Si pas d'overlay assigné, créer automatiquement
        if (poisonOverlay == null)
        {
            // Trouver ou créer le Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasGO = new GameObject("PoisonEffectCanvas");
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 100; // Au-dessus de tout
                canvasGO.AddComponent<GraphicRaycaster>();
            }
            
            // Créer l'overlay
            GameObject overlayGO = new GameObject("PoisonOverlay");
            overlayGO.transform.SetParent(canvas.transform, false);
            
            poisonOverlay = overlayGO.AddComponent<Image>();
            poisonOverlay.color = new Color(1f, 0f, 0f, 0f); // Rouge transparent
            
            // Couvrir tout l'écran
            RectTransform rect = poisonOverlay.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        
        // S'assurer que l'overlay est invisible au début
        poisonOverlay.color = new Color(poisonColor.r, poisonColor.g, poisonColor.b, 0f);
    }
    
    public void StartPoisonEffect(float duration)
    {
        if (poisonEffectCoroutine != null)
            StopCoroutine(poisonEffectCoroutine);
            
        poisonEffectCoroutine = StartCoroutine(PoisonEffectCoroutine(duration));
    }
    
    public void StopPoisonEffect()
    {
        if (poisonEffectCoroutine != null)
        {
            StopCoroutine(poisonEffectCoroutine);
            poisonEffectCoroutine = null;
        }
        
        // Faire disparaître l'effet rapidement
        StartCoroutine(FadeOutEffect());
    }
    
    private IEnumerator PoisonEffectCoroutine(float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float progress = elapsed / duration;
            
            // Augmenter l'intensité de manière exponentielle pour plus d'impact vers la fin
            float intensity = Mathf.Pow(progress, 1.5f);
            float currentAlpha = intensity * maxAlpha;
            
            poisonOverlay.color = new Color(poisonColor.r, poisonColor.g, poisonColor.b, currentAlpha);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Effet final - rouge intense
        poisonOverlay.color = new Color(poisonColor.r, poisonColor.g, poisonColor.b, maxAlpha);
    }
    
    private IEnumerator FadeOutEffect()
    {
        Color currentColor = poisonOverlay.color;
        float fadeSpeed = 2f; // Vitesse de disparition
        
        while (currentColor.a > 0.01f)
        {
            currentColor.a = Mathf.Lerp(currentColor.a, 0f, fadeSpeed * Time.deltaTime);
            poisonOverlay.color = currentColor;
            yield return null;
        }
        
        poisonOverlay.color = new Color(poisonColor.r, poisonColor.g, poisonColor.b, 0f);
    }
} 