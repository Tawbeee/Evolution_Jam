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

    private float initialY;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        turnAction = playerInput.actions["Turn"];
    }

    private void Start()
    {
        // Enregistre la position Y de départ
        initialY = transform.position.y;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        EnforceMinY();
    }

    private void HandleMovement()
    {
        turnInput = turnAction.ReadValue<float>();

        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.fixedDeltaTime, Space.Self);
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }

    private void EnforceMinY()
    {
        Vector3 pos = transform.position;
        if (pos.y < initialY)
        {
            pos.y = initialY;
            transform.position = pos;
        }
    }
}
