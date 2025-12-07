using UnityEngine;

public class BounceSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource hitSound;
    
    private MazeGameScore gameScore;
    private AudioSource enemyHitSound;
    private bool hasPlayed;

    void Start()
    {
        hasPlayed = false;
        enemyHitSound = GameObject.FindGameObjectWithTag("EnemyHitSound").GetComponent<AudioSource>();
        gameScore = GameObject.FindGameObjectWithTag("Score").GetComponent<MazeGameScore>();
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor")) && !hasPlayed)
        {
            hitSound.Play();
            hasPlayed = true;
        }
        if (other.gameObject.CompareTag("Enemy") && !hasPlayed)
        {
            var enemyAI = other.gameObject.GetComponent<EnemyAIController>();
            enemyAI.health--;
            enemyHitSound.Play();
            hasPlayed = true;
            gameScore.IncrementScore();
            if (enemyAI.health <= 0)
            {
                enemyAI.Dead();
            }
            Destroy(gameObject);
        }
    }
}
