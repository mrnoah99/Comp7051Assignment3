using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{ // Handles scene change between the menu and the gameplay
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Loaded Scene: " + sceneName);
    }

    public void PickCharacter(string character)
    {
        if (character == "Player 2")
        {
            GameController.spawnPlayer = true;
            ConsoleCommandsHandler.AIEnabled = false;
            Debug.Log("Spawning Player 2");
        }
        else
        {
            GameController.spawnPlayer = false;
            ConsoleCommandsHandler.AIEnabled = true;
            Debug.Log("Spawning AI");
        }
    }
}
