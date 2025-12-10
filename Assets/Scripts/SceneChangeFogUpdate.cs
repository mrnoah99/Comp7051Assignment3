using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class SceneChangeFogUpdate : MonoBehaviour
{
    [SerializeField]
    private FullScreenPassRendererFeature fog;
    [SerializeField] 
    private GameObject fogObj;
    
    private InputActions inputActions;
    private InputAction fogToggle;
    
    void Awake()
    {
        DisableFog();
    }

    void Start()
    {
        inputActions = new();
        fogToggle = inputActions.Player.FogToggle;
        fogToggle.performed += ToggleFog;
        fogToggle.Enable();

    }
    
    private void ToggleFog(InputAction.CallbackContext context)
    {
        fog.SetActive(true); //change to handle based on whether we actually want the fog enabled or not
        fogObj.SetActive(!fogObj.activeInHierarchy);
        Debug.Log("Fog enabled");
    }

    private void DisableFog()
    {
        fog.SetActive(false);
        fogObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Fog disabled");
    }
    
}
