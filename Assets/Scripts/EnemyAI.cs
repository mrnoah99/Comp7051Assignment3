using UnityEngine;
using UnityEngine.AI;

// Credit: I followed this tutorial for the enemy AI movement here https://www.youtube.com/watch?v=UjkSFoLxesw
public class EnemyAIController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    private Animator animator;
    private bool day;

    public Vector3 walkPoint;
    bool walkPointSet; // Target point for enemy to move towards
    public float walkPointRangeX;
    public float walkPointRangeZ;
    public int health = 3;
    public AudioSource musicDay;    
    public AudioSource musicNight;
    public MazeGameController gameController;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController == null) gameController = FindFirstObjectByType<MazeGameController>();

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        // Walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(0.1f, walkPointRangeX);
        float randomZ = Random.Range(0.1f, walkPointRangeZ);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        // Check if point is actually on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2.0f))
        {
            walkPointSet = true;
        }
        
        // animator.SetFloat("Move", walkPoint.x, walkPoint.y, walkPoint.z);
        

    }

    public void Dead()
    {
        gameController.EnemyDeath();
        Destroy(gameObject);
    }

    public void ChangeToDay()
    {
        musicNight.Stop();
        musicDay.Play();
        day = true;
    }

    public void ChangeToNight()
    {
        musicDay.Stop();
        musicNight.Play();
        day = false;
    }
    
    public void Pause()
    {
        if (day)
        {
            musicDay.Pause();
        } else
        {
            musicNight.Pause();
        }
    }

    public void Play()
    {
        if (day)
        {
            musicDay.UnPause();
        } else
        {
            musicNight.UnPause();
        }
    }

    public void MusicOnRespawn()
    {
        if (day)
        {
            ChangeToDay();
        } else
        {
            ChangeToNight();
        }
    }

    public void UpdateVolume(bool fog)
    {
        if (fog)
        {
            musicDay.volume /= 2;
            musicNight.volume /=2;
        } else
        {
            musicDay.volume *= 2;
            musicNight.volume *= 2;
        }
    }
}
