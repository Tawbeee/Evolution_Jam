using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] public KillingObjectType playerState;
    [SerializeField] private KillingObjectType startState;

    private bool movementIsArrow = false;
    [SerializeField] private PlayerMovementTutorial playerMovementScript;
    [SerializeField] private PlayerArrowMovement arrowMovementScript;

  

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerState = startState;
    }

    private void OnEnable()
    {
        PlayerEvents.OnKilled += OnPlayerKilled;
    }

    private void OnDisable()
    {
        PlayerEvents.OnKilled -= OnPlayerKilled;
    }

    private Coroutine poisonCoroutine;


    private void HandlePoisonEffect()
    {
        // Si on est en poison, on lance la coroutine de mort
        if (playerState == KillingObjectType.Poison)
        {
            // Si d�j� en train de compter, on arr�te pour red�marrer le timer
            if (poisonCoroutine != null)
                StopCoroutine(poisonCoroutine);

            poisonCoroutine = StartCoroutine(PoisonKillAfterDelay(30f));
        }
        else
        {
            // Si on n'est plus en poison, on annule la coroutine
            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine = null;
            }
        }
    }

    private IEnumerator PoisonKillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // V�rifier que le joueur est toujours en poison au moment de la mort
        if (playerState == KillingObjectType.Poison)
        {
            PlayerEvents.Kill(KillingObjectType.Poison);
        }
    }

    // Modifie ta m�thode OnPlayerKilled pour appeler HandlePoisonEffect apr�s changement d'�tat
    private void OnPlayerKilled(KillingObjectType type)
    {
        if (playerState == KillingObjectType.Arrow)
        {
            SwitchMovement();
        }
        playerState = type;

        this.transform.position = SpawnPoint.position;
        HandlePoisonEffect();


    }

    public void SwitchMovement()
    {
        movementIsArrow = !movementIsArrow;
        ApplyMovementType();
    }

    private void ApplyMovementType()
    {
        if (arrowMovementScript != null)
            arrowMovementScript.enabled = movementIsArrow;

        if (playerMovementScript != null)
            playerMovementScript.enabled = !movementIsArrow;
    }

    




}
