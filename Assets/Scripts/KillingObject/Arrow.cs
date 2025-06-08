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

    public Vector2 direction = Vector2.right; // Défini par ArrowThrower
    public float speed = 10f;

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    void Start()
    {
        Destroy(gameObject, 10f); // détruit l'objet après 10 secondes
    }

}