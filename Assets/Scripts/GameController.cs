using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GameController : MonoBehaviour
{
    // Ball moveSpeed is very high because it has more mass to make it slide better
    public int moveSpeed = 250;
    public TextMeshProUGUI score;
    public TextMeshProUGUI menu;
    public GameObject player2;
    public GameObject ai;
    public static bool spawnPlayer = false;

    [SerializeField]
    private SceneChanger sceneChanger;

    private Rigidbody rb;
    private int leftright; // Used to determine the launch's x direction
    private float updown; // Used to determine the launch's z direction
    private float currentTime = 0;
    private float launch = 3;
    // Prevents the ball from launching itself more than once per round
    private bool launched = false;
    private int p1Score = 0;
    private int p2Score = 0;
    private bool gameOver = false;

    void Awake()
    {
        // Handles instantiating the inital Player2/AI
        if (!spawnPlayer)
        {
            GameObject character = Instantiate(ai);
            character.transform.position = new Vector3(30, 1, 0);
            CPUController script = character.GetComponent<CPUController>();
            script.ball = gameObject;
        }
        else
        {
            GameObject character = Instantiate(player2);
            character.transform.position = new Vector3(30, 1, 0);
        }
    }

    void Start()
    {
        // Randomises left/right and up/down for the first launch
        // and sets max speed
        rb = GetComponent<Rigidbody>();
        leftright = Random.Range(-1, 2);
        updown = Random.Range((float)-0.5, (float)0.5);
        if (leftright == 0) leftright = -1;
        rb.maxLinearVelocity = 25;
    }

    void FixedUpdate()
    {
        // Handles launching and round resets
        if (!launched && currentTime >= launch && !gameOver)
        {
            rb.AddForce(new Vector3(leftright * moveSpeed, 0, updown * moveSpeed), ForceMode.Impulse);
            currentTime = 0;
            launched = true;
        }
        else if (!launched && !gameOver)
        {
            currentTime += Time.fixedDeltaTime;
        }
        if (currentTime >= 1 && !gameOver)
        {
            menu.text = "";
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // Handles collisions with all 3 objects the ball might collide with
        Vector3 velocity = rb.linearVelocity;
        /* Collisions with the Players/AI only invert the x value of the ball's
            velocity. If the Player/AI is moving it will also slightly boost the
            ball's velocity in the z direction. */
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("AI"))
        {
            if (other.gameObject.GetComponent<Rigidbody>().linearVelocity.z > 0)
            {
                rb.linearVelocity = new Vector3((float)(velocity.x * -7.5), 0, velocity.z + Random.Range(3, 5));
            }
            else if (other.gameObject.GetComponent<Rigidbody>().linearVelocity.z < 0)
            {
                rb.linearVelocity = new Vector3((float)(velocity.x * -7.5), 0, velocity.z + Random.Range(-5, -3));
            }
            else
            {
                rb.linearVelocity = new Vector3((float)(velocity.x * -8), 0, velocity.z);
            }
        }
        /* Collisions with the wall will invert the z value of the ball's velocity.
            They will also give a boost to the ball's x velocity if it's moving
            too slowly. */
        else if (other.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = new Vector3(velocity.x, 0, velocity.z * (float)1.1);
            if (velocity.x < 1 && velocity.x > 0)
            {
                rb.AddForce(new Vector3(moveSpeed, 0, 0), ForceMode.Impulse);
            }
            else if (velocity.x > -1 && velocity.x < 0)
            {
                rb.AddForce(new Vector3(-moveSpeed, 0, 0), ForceMode.Impulse);
            }
        }
        /* A killbox was implemented to make sure that if the ball gets out of
            bounds, it gets returned with no Player earning a point. */
        else if (other.gameObject.CompareTag("KillBox"))
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            transform.position = new Vector3(0, 1, 0);
            launched = false;
            currentTime = 0;
            leftright = Random.Range(-1, 2);
            updown = Random.Range((float)-0.5, (float)0.5);
            if (leftright == 0) leftright = -1;
            menu.text = "Out of Bounds!";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        /* The only triggers present in this project are the point counters, so it
            will reset the ball's position and give points to the appropriate player */
        rb.linearVelocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 1, 0);
        launched = false;
        currentTime = 0;
        leftright = Random.Range(-1, 2);
        updown = Random.Range((float)-0.5, (float)0.5);
        if (leftright == 0) leftright = -1;
        if (other.gameObject.tag == "p2Gate") p1Score++;
        if (other.gameObject.tag == "p1Gate") p2Score++;
        score.text = p1Score + " - " + p2Score;
        if (p1Score == 5)
        {
            menu.text = "Player 1 wins!";
            gameOver = true;
            StartCoroutine(nameof(GameOver));
        }
        else if (p2Score == 5)
        {
            menu.text = "Player 2 wins!";
            gameOver = true;
            StartCoroutine(nameof(GameOver));
        }
        else
        {
            menu.text = "Score!";
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        if (gameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            sceneChanger.ChangeScene("Maze");
        }
    }
}
