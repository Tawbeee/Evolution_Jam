using UnityEngine;

public class StompKillingObject : KillingObject
{
    [SerializeField] private float stompDelay = 1f;
    [SerializeField] private float resetDelay = 2f;
    [SerializeField] private float riseSpeed = 5f;
    private Vector3 initialPosition;
    private Rigidbody rb;
    private bool hasStomped = false;

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // attend avant de tomber
        initialPosition = transform.position;

        Invoke(nameof(StartStomp), stompDelay);
    }

    private void StartStomp()
    {
        rb.isKinematic = false;
        Invoke(nameof(ResetStomp), resetDelay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasStomped) return;


        // G�re le kill aussi via la collision (en plus du trigger)
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerEvents.Kill(killingObjectType);
        }
    }

    private void ResetStomp()
    {
        
        rb.isKinematic = true;
        StartCoroutine(RiseBackUp());
    }

    private System.Collections.IEnumerator RiseBackUp()
    {
        
        while (Vector3.Distance(transform.position, initialPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, riseSpeed * Time.deltaTime);
            yield return null;
        }

        
        hasStomped = false;
        Invoke(nameof(StartStomp), stompDelay);
    }


    // Optionnel : le collider doit rester non-trigger pour utiliser OnCollisionEnter
    

    // Si tu veux aussi garder le comportement trigger pour tuer au contact :
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
