using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowBall : MonoBehaviour
{
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject player;
    private InputActions inputActions;
    private InputAction ballThrow;
    private Transform spawn;

    void Start()
    {
        inputActions = new();
        ballThrow = inputActions.Player.Attack;
        ballThrow.performed += Throw;
        ballThrow.Enable();
        spawn = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
    }

    private void Throw(InputAction.CallbackContext context)
    {
        var camera = player.GetComponentInChildren<Camera>().gameObject;
        var obj = Instantiate(ball);
        var ballRB = obj.GetComponent<Rigidbody>();
        
        obj.transform.position = spawn.position;
        ballRB.linearVelocity = camera.transform.forward * 10;
        Debug.Log("Ball Thrown");
    }
}
