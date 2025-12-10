using TMPro;
using UnityEngine;

public class MazeGameScore : MonoBehaviour
{
    public int score;

    void Start()
    {
        // put loading from save in here
        score = SaveGameController.sGCtrl.score;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    void Update()
    {
        SaveGameController.sGCtrl.score = score;
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

    public void ZeroScore()
    {
        score = 0;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }
}
