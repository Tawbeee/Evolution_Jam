using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }

    [SerializeField] public Transform SpawnPoint;
    [SerializeField] public KillingObjectType playerState;
    [SerializeField] private KillingObjectType startState;
    [SerializeField] private GameObject astronaut;
    [SerializeField] private CapsuleCollider astronautCollider;

    [SerializeField] private PlayerMovementTutorial playerMovementScript;
    [SerializeField] private PlayerArrowMovement arrowMovementScript;

    [SerializeField] private float invincibilityDuration = 2f;
    public bool isInvincible = false;

    [SerializeField] private Rigidbody rb;

    private bool movementIsArrow = false;

    private Coroutine poisonCoroutine;

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
                stateValue = "Nothing";
                break;
        }

        AkUnitySoundEngine.SetState(gameStateName, stateValue);
        Debug.Log($"Wwise Game State set to: {gameStateName} - {stateValue}");
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (rb == null)
            rb = GetComponent<Rigidbody>();
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

    private void HandlePoisonEffect()
    {
        if (playerState == KillingObjectType.Poison)
        {
            if (poisonCoroutine != null)
                StopCoroutine(poisonCoroutine);

            poisonCoroutine = StartCoroutine(PoisonKillAfterDelay(30f));

            if (PoisonScreenEffect.Instance != null)
                PoisonScreenEffect.Instance.StartPoisonEffect(30f);
        }
        else
        {
            if (poisonCoroutine != null)
            {
                StopCoroutine(poisonCoroutine);
                poisonCoroutine = null;
            }

            if (PoisonScreenEffect.Instance != null)
                PoisonScreenEffect.Instance.StopPoisonEffect();
        }
    }

    private IEnumerator PoisonKillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerState == KillingObjectType.Poison)
        {
            PlayerEvents.Kill(KillingObjectType.Poison);
        }
    }

    private void OnPlayerKilled(KillingObjectType type)
    {
        if (isInvincible) return;

        if (playerState == KillingObjectType.Arrow)
        {
            SwitchMovement();
        }

        playerState = type;

        // Reset rotation
        transform.rotation = Quaternion.identity;

        // Reset Rigidbody velocity and angular velocity
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        StartCoroutine(ActivateInvincibility());

        transform.position = SpawnPoint.position;

        HandlePoisonEffect();
        HandleStompEffect();

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

        astronaut.transform.localScale = isStomp
            ? new Vector3(1f, 0.4f, 1f)
            : new Vector3(1f, 1f, 1f);

        if (astronautCollider != null)
        {
            astronautCollider.height = isStomp ? 1f : 2f;
            astronautCollider.center = isStomp ? new Vector3(0f, -0.5f, 0f) : Vector3.zero;
        }
    }
}
