using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CPUController : MonoBehaviour
{
    // Used to track the ball so the AI knows where to go
    public GameObject ball;
    // AI moves slower to give the player a decent chance
    public int moveSpeed = 5;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = 15;
    }

    void FixedUpdate()
    {
        // Moves the AI using forces to simulate the AI being dumb and not able to
        // reach where it's trying to go correctly
        if (ball.transform.position.z < transform.position.z)
        {
            rb.AddForce(new Vector3(0, 0, moveSpeed * Random.Range((float)-0.5, -1)), ForceMode.Impulse);
        }
        else if (ball.transform.position.z > transform.position.z)
        {
            rb.AddForce(new Vector3(0, 0, moveSpeed * Random.Range((float)0.5, 1)), ForceMode.Impulse);
        }
        else // Also tries to slow itself down if it is level with the ball
        {
            Vector3 velocity = rb.linearVelocity;
            if (velocity.z < 0)
            {
                rb.AddForce(new Vector3(0, 0, moveSpeed * (float)-0.5), ForceMode.Impulse);
            }
            else if (velocity.z > 0)
            {
                rb.AddForce(new Vector3(0, 0, moveSpeed * (float)-0.5), ForceMode.Impulse);
            }
        }
    }

    // All paddles, when hitting a wall, are forced to stop in case they are
    // moving with enough speed to pass the mesh of the wall
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }
}
