using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public int moveSpeed = 15;

    private Rigidbody rb;
    private float moveInputY;
    private PlayerInput input;

    void Start()
    {
        /* Assigns input of keyboard by default, and also supports a controller
            if it is plugged in. */
        input = gameObject.GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        if (Keyboard.current != null)
        {
            input.SwitchCurrentControlScheme(Keyboard.current);
            if (Gamepad.all.Count > 0)
            {
                input.SwitchCurrentControlScheme(new InputDevice[] { Keyboard.current, Gamepad.all[0] });
            }
        }
        input.ActivateInput();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInputY = input.y;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(0, 0, moveInputY * moveSpeed);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }
}
