using TMPro;
using UnityEngine;

public class MazeGameScore : MonoBehaviour
{
    public int score;

    void Start()
    {
        // put loading from save in here
        score = 0;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    public void IncrementScore()
    {
        score++;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    public void DecrementScore()
    {
        score--;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }
}
