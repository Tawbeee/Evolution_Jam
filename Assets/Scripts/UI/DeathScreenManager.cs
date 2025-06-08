using UnityEngine;
using TMPro;
using System.Collections;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private TextMeshProUGUI secondaryText;
    [SerializeField] private float screenDuration = 3f; // Dur�e d'affichage de l'�cran

    private Animator panelAnimator;
    private bool isShowing = false; // Nouveau flag pour emp�cher les d�clenchements multiples

    void Awake()
    {
        if (deathScreenPanel != null)
        {
            panelAnimator = deathScreenPanel.GetComponent<Animator>();
            deathScreenPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Le Death Screen Panel n'est pas assign� dans le DeathScreenManager !");
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
        // Emp�che l'affichage multiple
        if (isShowing) return;

        Debug.Log("�V�NEMENT DE MORT RE�U ! Affichage de l'�cran.");

        if (deathScreenPanel == null || panelAnimator == null || secondaryText == null)
        {
            Debug.LogError("Une r�f�rence est manquante sur le DeathScreenManager, impossible d'afficher l'�cran.");
            return;
        }

        string enemyName = killingObject.ToString();
        secondaryText.text = "But you got the " + enemyName;

        // Active le panel et d�marre la d�sactivation
        deathScreenPanel.SetActive(true);
        isShowing = true;
        panelAnimator.SetTrigger("Show");

        // D�marrer la coroutine de d�sactivation
        StartCoroutine(DisableAfterDelay());
    }

    // Nouvelle coroutine pour d�sactiver le panel apr�s d�lai
    private IEnumerator DisableAfterDelay()
    {
        // Attendre la dur�e sp�cifi�e
        yield return new WaitForSeconds(screenDuration);

        // D�sactiver compl�tement le panel
        deathScreenPanel.SetActive(false);
        isShowing = false;
    }
}