using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneChangeFogUpdate : MonoBehaviour
{
    [SerializeField]
    private FullScreenPassRendererFeature fog;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Maze")
        {
            fog.SetActive(false);
        } else
        {
            fog.SetActive(true); //change to handle based on whether we actually want the fog enabled or not
        }
    }
}
