using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArrowMovement : MonoBehaviour
{
    [Header("Arrow Movement Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public float minHeight = 1f; // Hauteur minimale de la flèche
    public float fallSpeed = 2f; // Vitesse de descente de la flèche

    public Transform orientation;
    public PlayerInput playerInput;

    private InputAction turnAction;
    private float turnInput;
    private Rigidbody rb;
    private bool hadGravity;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        turnAction = playerInput.actions["Turn"];
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Quand ce script s'active, désactive l'autre script de mouvement
        PlayerMovementTutorial otherMovement = GetComponent<PlayerMovementTutorial>();
        if (otherMovement != null)
            otherMovement.enabled = false;

        // Si on a un Rigidbody, configurer pour le mouvement flèche
        if (rb != null)
        {
            hadGravity = rb.useGravity;
            rb.useGravity = false; // Désactiver la gravité pour la flèche
            rb.linearDamping = 0.2f; // Peu de damping pour garder la réactivité
            rb.angularDamping = 0.5f; // Damping angulaire réduit
        }
    }

    private void OnDisable()
    {
        // Quand ce script se désactive, reactive l'autre script de mouvement
        PlayerMovementTutorial otherMovement = GetComponent<PlayerMovementTutorial>();
        if (otherMovement != null)
            otherMovement.enabled = true;

        // Restaurer les paramètres du Rigidbody
        if (rb != null)
        {
            rb.useGravity = hadGravity;
            rb.linearDamping = 0f; // Restaurer le damping original
            rb.angularDamping = 0.05f; // Restaurer le damping angulaire original
        }
    }

    private void FixedUpdate()
    {
        HandleArrowMovement();
        EnforceMinHeight();
    }

    private void HandleArrowMovement()
    {
        // Lire l'input de rotation (gauche/droite)
        turnInput = turnAction.ReadValue<float>();

        // TOUJOURS utiliser la rotation directe pour garder le contrôle
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime, Space.Self);

        if (rb != null)
        {
            // Utiliser le Rigidbody pour le mouvement
            Vector3 forwardVelocity = transform.forward * moveSpeed;
            
            // Ajouter une descente légère si on est au-dessus de la hauteur minimale
            float verticalVelocity = transform.position.y > minHeight ? -fallSpeed : 0f;
            
            rb.linearVelocity = new Vector3(forwardVelocity.x, verticalVelocity, forwardVelocity.z);
        }
        else
        {
            // Fallback sans Rigidbody
            Vector3 movement = transform.forward * moveSpeed * Time.fixedDeltaTime;
            
            // Ajouter la descente si on est au-dessus de la hauteur minimale
            if (transform.position.y > minHeight)
            {
                movement.y = -fallSpeed * Time.fixedDeltaTime;
            }
            
            transform.position += movement;
        }
    }

    private void EnforceMinHeight()
    {
        Vector3 pos = transform.position;
        if (pos.y < minHeight)
        {
            pos.y = minHeight;
            
            if (rb != null)
            {
                // Utiliser MovePosition pour éviter les conflits avec la physique
                rb.MovePosition(pos);
                // Annuler la vélocité verticale quand on atteint la hauteur minimale
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            }
            else
            {
                transform.position = pos;
            }
        }
    }
}
