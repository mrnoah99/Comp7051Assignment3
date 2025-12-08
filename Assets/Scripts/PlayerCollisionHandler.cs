using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource wallCollide;
    [SerializeField]
    private MazeCreator creator;
    
    private MazeGameScore gameScore;

    void Start()
    {
        gameScore = FindFirstObjectByType<MazeGameScore>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wallCollide.Play();
        } if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(creator.NewMaze());
            gameScore.ZeroScore();
        }
    }
}
