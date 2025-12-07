using UnityEngine;

public class PongDoor : MonoBehaviour
{
    [SerializeField]
    private SceneChanger sceneChanger;

    public void StartPong()
    {
        sceneChanger.ChangeScene("Menu");
    }
}
