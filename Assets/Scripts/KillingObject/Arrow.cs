using UnityEngine;

public class Arrow : KillingObject
{
    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerState.Instance.playerState != killingObjectType)
            {
                PlayerEvents.Kill(killingObjectType);
                PlayerState.Instance.SwitchMovement();
            }
        }
    }

    public Vector3 direction = Vector3.right; // Défini par ArrowThrower
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void Start()
    {
        // Oriente vers la direction souhaitée
        transform.rotation = Quaternion.LookRotation(direction);

        Destroy(gameObject, 5f);
    }
}
