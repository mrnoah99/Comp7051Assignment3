using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private AudioSource wallCollide;
    [SerializeField]
    private MazeCreator creator;
    
    private MazeGameScore gameScore;
    private MazeGameController gameController;

    void Start()
    {
        gameScore = FindFirstObjectByType<MazeGameScore>();
        gameController = FindFirstObjectByType<MazeGameController>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wallCollide.Play();
        } if (other.gameObject.CompareTag("Enemy"))
        {
            gameScore.ZeroScore();
            gameController.ResetKills();
            StartCoroutine(creator.NewMaze());
        }
    }
}
