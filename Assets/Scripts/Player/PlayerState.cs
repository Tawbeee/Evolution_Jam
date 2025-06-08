using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] public KillingObjectType playerState;
    [SerializeField] private KillingObjectType startState;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private CapsuleCollider astronautCollider;

    private bool movementIsArrow = false;
    [SerializeField] private PlayerMovementTutorial playerMovementScript;
    [SerializeField] private PlayerArrowMovement arrowMovementScript;

    [SerializeField] private float invincibilityDuration = 2f;
    public bool isInvincible = false;


    // Wwise Game State switching
    private void SwitchWwiseGameState()
    {
        string gameStateName = "PlayerState";
        string stateValue;

        switch (playerState)
        {
            case KillingObjectType.None:
                stateValue = "Nothing";
                break;
            case KillingObjectType.Spike:
                stateValue = "Spike";
                break;
            case KillingObjectType.Fire:
                stateValue = "Fire";
                break;
            case KillingObjectType.Water:
                stateValue = "Water";
                break;
            case KillingObjectType.Gravity:
                stateValue = "Fall";
                break;
            case KillingObjectType.Poison:
                stateValue = "Poison";
                break;
            case KillingObjectType.Arrow:
                stateValue = "Arrow";
                break;
            case KillingObjectType.Electricity:
                stateValue = "Lightning";
                break;
            case KillingObjectType.Stomp:
                stateValue = "Stomp";
                break;
            default:
                stateValue = "Nothing"; // Fallback to normal state
                break;
        }

        AkUnitySoundEngine.SetState(gameStateName, stateValue);
        Debug.Log($"Wwise Game State set to: {gameStateName} - {stateValue}");
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerState = startState;
        
        SwitchWwiseGameState();
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
            // Si déjà en train de compter, on arrête pour redémarrer le timer
            if (poisonCoroutine != null)
                StopCoroutine(poisonCoroutine);

            poisonCoroutine = StartCoroutine(PoisonKillAfterDelay(30f));
            
            // Démarrer l'effet visuel rouge
            if (PoisonScreenEffect.Instance != null)
                PoisonScreenEffect.Instance.StartPoisonEffect(30f);
        }
        else
        {
            // Si on n'est plus en poison, on annule la coroutine
            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine = null;
            }
            
            // Arrêter l'effet visuel rouge
            if (PoisonScreenEffect.Instance != null)
                PoisonScreenEffect.Instance.StopPoisonEffect();
        }
    }

    private IEnumerator PoisonKillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Vérifier que le joueur est toujours en poison au moment de la mort
        if (playerState == KillingObjectType.Poison)
        {
            PlayerEvents.Kill(KillingObjectType.Poison);
        }
    }

    // Modifie ta méthode OnPlayerKilled pour appeler HandlePoisonEffect après changement d'état
    private void OnPlayerKilled(KillingObjectType type)
    {

        if (isInvincible) return; // Ignore si déjà invincible

        if (playerState == KillingObjectType.Arrow)
        {
            SwitchMovement();
        }
        playerState = type;

        // Active l'invincibilité
        StartCoroutine(ActivateInvincibility());



        this.transform.position = SpawnPoint.position;
        HandlePoisonEffect();
        HandleStompEffect();

        // Switch Wwise Game State
        SwitchWwiseGameState();
    }

    private IEnumerator ActivateInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
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


    private void HandleStompEffect()
    {
        bool isStomp = (playerState == KillingObjectType.Stomp);

        // Vitesse
        

        // Taille de l'apparence (astronaute)
        astronaut.transform.localScale = isStomp
            ? new Vector3(1f, 0.4f, 1f)  // garde une taille visible
            : new Vector3(1f, 1f, 1f);

        // Ajuste le CharacterController (évite qu'il flotte ou colle au sol)
        if (astronautCollider != null)
        {
            astronautCollider.height = isStomp ? 1f : 2f;
            astronautCollider.center = isStomp ? new Vector3(0f, -0.5f, 0f) : new Vector3(0f, 0f, 0f);
        }
    }






}
