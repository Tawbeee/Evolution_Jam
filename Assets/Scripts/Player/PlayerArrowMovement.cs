using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArrowMovement : MonoBehaviour
{
    [Header("Arrow Movement Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    public Transform orientation;
    public PlayerInput playerInput;

    private InputAction turnAction;
    private float turnInput;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        turnAction = playerInput.actions["Turn"];
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Lire l'entrée de rotation
        turnInput = turnAction.ReadValue<float>();

        // Rotation uniquement autour de l'axe Y (lacets)
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime, Space.Self);

        // Avance constante dans la direction que pointe la flèche
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }
}
