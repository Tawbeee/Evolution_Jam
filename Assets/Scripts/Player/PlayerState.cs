using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; private set; }
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] public KillingObjectType playerState;
    [SerializeField] private KillingObjectType startState;


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

    private void OnPlayerKilled(KillingObjectType type)
    {
        playerState = type; 
        this.transform.position = SpawnPoint.position;

    }

}
