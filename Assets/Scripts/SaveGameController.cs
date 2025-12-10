using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class SaveGameController : MonoBehaviour
{
    public int score = 0; //use persistentDataPath to save high score in custom GameData object
    public Transform player;
    public Transform enemy;
    const string fileName = "/savegame.json";
    public static SaveGameController sGCtrl;
    public GameData data;

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
        //public string mazeSeed;


        public GameData(Vector3 playerPos, Vector3 enemyPos, int score, string seed)
        {
            playerX = playerPos.x;
            playerY = playerPos.y;
            playerZ = playerPos.z;

            enemyX = enemyPos.x;
            enemyY = enemyPos.y;
            enemyZ = enemyPos.z;

            savedScore = score;
            //mazeSeed = seed;
        }
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
            data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game loaded!");
            // Restore score, enemy location, player location, and maze layout
            sGCtrl.score = data.savedScore;
            player.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            enemy.position = new Vector3(data.enemyX, data.enemyY, data.enemyZ);
            
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + fileName, json); //open file path for writing
        Debug.Log("Game saved!");

    }

    public void ExitGame()
    {
        SaveGame();
        Application.Quit();
    }
}

