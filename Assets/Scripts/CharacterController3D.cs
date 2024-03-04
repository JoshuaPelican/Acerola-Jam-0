using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class CharacterController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float maxVelocity = 1.25f;
    [SerializeField] float jumpHeight = 4;

    Rigidbody rb;
    Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
            Move(((moveInput.x * transform.right) + (moveInput.y * transform.forward)).normalized, moveSpeed);
    }

    public void InputMove(InputAction.CallbackContext callback)
    {
        if (!callback.performed)
            return;

        moveInput = Vector2.ClampMagnitude(callback.ReadValue<Vector2>(), 1);
    }

    public void InputJump(InputAction.CallbackContext callback)
    {
        if (!callback.performed)
            return;

        Move(Vector3.up, jumpHeight);
    }

    public void Move(Vector3 direction, float speed)
    {
        rb.AddForce(speed * direction, ForceMode.VelocityChange);
        Vector3 clampedVelocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        rb.velocity = new Vector3(clampedVelocity.x, rb.velocity.y, clampedVelocity.z);
    }
}
