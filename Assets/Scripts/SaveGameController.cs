using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SaveGameController : MonoBehaviour
{
    public int score = 0; //use persistentDataPath to save high score in custom GameData object
    public Transform player;
    public Transform enemy;
    const string fileName = "savegame.json";
    public static SaveGameController sGCtrl;
    public GameData savedData;

    [SerializeField]
    private GameObject menuObj;

    private InputActions inputActions;
    private InputAction menu;
    
    
    private void Awake()
    {
        if (sGCtrl == null)
        {
            DontDestroyOnLoad(gameObject);
            sGCtrl = this;
            LoadGame();
        }

        inputActions = new();
        menu = inputActions.Player.OpenMenu;
        menu.performed += OpenMenu;
        menu.Enable();
    }

    //Small custom class to hold high score that will be saved and serialized using persistentDataPath
    //Serialization: automatic process of transforming data structures or object states into a format that Unity can store and reconstruct later
    [Serializable]
    public class GameData
    {
        public float playerX, playerY, playerZ;
        public float enemyX, enemyY, enemyZ;
        public int savedScore;
        
    }

    private void OpenMenu(InputAction.CallbackContext context)
    {
        menuObj.SetActive(!menuObj.activeInHierarchy);
        Cursor.lockState = menuObj.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CloseMenu()
    {
        menuObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            string json = File.ReadAllText(Application.persistentDataPath + fileName);
            savedData = JsonUtility.FromJson<GameData>(json);

            if (savedData == null)
            {
                Debug.Log("Save not found!");
                return;
            }
            
            // Restore score, enemy location, player location, and maze layout
            sGCtrl.score = savedData.savedScore;
            player.position = new Vector3(savedData.playerX, savedData.playerY, savedData.playerZ);
            enemy.position = new Vector3(savedData.enemyX, savedData.enemyY, savedData.enemyZ);
            
            Debug.Log($"Game loaded! Player loc: {player.position} || Enemy loc: {enemy.position} || Score: {sGCtrl.score}");
            
        }
    }
    
    public static void SaveData(Vector3 playerPos, Vector3 enemyPos, int score)
    {
        GameData data = new GameData
        {
            playerX = playerPos.x,
            playerY = playerPos.y,
            playerZ = playerPos.z,

            enemyX = enemyPos.x,
            enemyY = enemyPos.y,
            enemyZ = enemyPos.z,

            savedScore = score
        };
        
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + fileName, json);
        Debug.Log("Game saved to  " + Application.persistentDataPath + fileName);

    }

    public void SaveGame()
    {
        SaveData(player.position, enemy.position, score);
        Debug.Log($"Game saved! Player loc: {player.position} || Enemy loc: {enemy.position} || Score: {sGCtrl.score}");
    }

    public void ExitGame()
    {
        SaveGame();
        Application.Quit();
        Debug.Log("Bye bye!!!!!!");
    }
}

