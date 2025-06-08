using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] GameObject arrowPrefab;   // Le prefab de la flèche
    [SerializeField] float fireRate = 1f;       // Intervalle de tir
    [SerializeField] float arrowForce = 10f;    // Vitesse de la flèche

    [Header("Direction Settings")]
    [SerializeField, Tooltip("Angle de tir en degrés (0 = droite, 90 = haut, etc.)")]
    float shootAngle = 0f;

    [SerializeField] float angleX = 0f;         // Rotation autour de l'axe X
    [SerializeField] float angleY = 0f;         // Rotation autour de l'axe Y
    [SerializeField] float angleZ = 0f;         // Rotation autour de l'axe Z
    [SerializeField] Transform Spawn; // Utiliser la rotation locale
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
        // Calcule la direction à partir de l’angle
        float angleRad = shootAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

        // Crée une rotation alignée sur la direction
        Quaternion rotation = Quaternion.Euler(angleX, angleY, angleZ);

        // Instancie la flèche avec cette rotation
        GameObject arrow = Instantiate(arrowPrefab, Spawn.position, rotation);

        // Applique la vélocité
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * arrowForce;
        }
    }
}
