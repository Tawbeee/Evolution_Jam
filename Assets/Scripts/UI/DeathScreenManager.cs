using UnityEngine;
using TMPro;
using System.Collections;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private TextMeshProUGUI secondaryText;
    [SerializeField] private float screenDuration = 3f; // Durée d'affichage de l'écran

    private Animator panelAnimator;
    private bool isShowing = false; // Nouveau flag pour empêcher les déclenchements multiples

    void Awake()
    {
        if (deathScreenPanel != null)
        {
            panelAnimator = deathScreenPanel.GetComponent<Animator>();
            deathScreenPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Le Death Screen Panel n'est pas assigné dans le DeathScreenManager !");
        }
    }

    private void OnEnable()
    {
        PlayerEvents.OnKilled += HandlePlayerKilled;
    }

    private void OnDisable()
    {
        PlayerEvents.OnKilled -= HandlePlayerKilled;
    }

    private void HandlePlayerKilled(KillingObjectType killingObject)
    {   
        Debug.Log("ÉVÉNEMENT DE MORT REÇU ! Affichage de l'écran.");
        // Empêche l'affichage multiple
        if (isShowing) return;

        

        if (deathScreenPanel == null || panelAnimator == null || secondaryText == null)
        {
            Debug.LogError("Une référence est manquante sur le DeathScreenManager, impossible d'afficher l'écran.");
            return;
        }

        string enemyName = killingObject.ToString();
        secondaryText.text = "You got Killed but ...\n you got the " + enemyName;

        // Active le panel et démarre la désactivation
        deathScreenPanel.SetActive(true);
        isShowing = true;
        panelAnimator.SetTrigger("Show");

        // Démarrer la coroutine de désactivation
        StartCoroutine(DisableAfterDelay());
    }

    // Nouvelle coroutine pour désactiver le panel après délai
    private IEnumerator DisableAfterDelay()
    {
        // Attendre la durée spécifiée
        yield return new WaitForSeconds(screenDuration);

        // Désactiver complètement le panel
        deathScreenPanel.SetActive(false);
        isShowing = false;
    }
}