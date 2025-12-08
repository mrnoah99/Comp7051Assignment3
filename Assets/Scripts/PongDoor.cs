using UnityEngine;

public class PongDoor : MonoBehaviour
{
    [SerializeField]
    private SceneChanger sceneChanger;

    public void StartPong()
    {
        Cursor.lockState = CursorLockMode.None;
        sceneChanger.ChangeScene("Menu");
    }
}
