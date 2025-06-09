using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float arrowForce = 10f;

    [SerializeField] Transform spawn; // Point d'apparition de la flèche

    private float timer;

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= fireRate)
        {
            FireArrow();
            timer = 0f;
        }
    }

    void FireArrow()
    {
        // Utilise la direction locale X de l’ArrowThrower
        Vector3 direction = transform.right;

        // Instancie la flèche
        GameObject arrow = Instantiate(arrowPrefab, spawn.position, Quaternion.identity);

        // Donne la direction à la flèche
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.direction = direction.normalized;
        }
    }
}
