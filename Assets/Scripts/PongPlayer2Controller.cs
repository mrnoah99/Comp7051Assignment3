using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PongPlayer2Controller : MonoBehaviour
{ // Essentially the same as the PlayerController, just designed specifically
    // for Player2
    public int moveSpeed = 15;

    private Rigidbody rb;
    private float moveInputY;
    private PlayerInput input;

    void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        if (Keyboard.current != null)
        {
            input.SwitchCurrentControlScheme(Keyboard.current);
            if (Gamepad.all.Count > 1)
            {
                input.SwitchCurrentControlScheme(new InputDevice[] { Keyboard.current, Gamepad.all[1] });
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
