using UnityEngine;

public class BounceSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource hitSound;
    private bool hasPlayed;

    void Start()
    {
        hasPlayed = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor")) && !hasPlayed)
        {
            hitSound.Play();
            hasPlayed = true;
        }
    }
}
