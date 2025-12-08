using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class SaveGameController : MonoBehaviour
{
    public int score = 0; //use persistentDataPath to save high score in custom GameData object
    public Transform player;
    public Transform enemy;
    const string fileName = "/savegame.dat";
    public static SaveGameController sGCtrl;

    private void Awake()
    {
        if (sGCtrl == null)
        {
            DontDestroyOnLoad(gameObject);
            sGCtrl = this;
            LoadGame();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SaveGame();
            Application.Quit();
            Debug.Log("Quitting game . . .");
        }
    }

    //Small custom class to hold high score that will be saved and serialized using persistentDataPath
    //Serialization: automatic process of transforming data structures or object states into a format that Unity can store and reconstruct later
    [Serializable]
    public class GameData
    {
        public float playerX, playerY, playerZ;
        public float enemyX, enemyY, enemyZ;
        public int savedScore;

        public GameData(Vector3 playerPos, Vector3 enemyPos, int score)
        {
            playerX = playerPos.x;
            playerY = playerPos.y;
            playerZ = playerPos.z;

            enemyX = enemyPos.x;
            enemyY = enemyPos.y;
            enemyZ = enemyPos.z;

            savedScore = score;
        }
    }

    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read);
            GameData data = (GameData)bf.Deserialize(fs);
            fs.Close();
            // Restore score
            sGCtrl.score = data.savedScore;
            // Restore player location
            player.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            // Restore enemy location
            enemy.position = new Vector3(data.enemyX, data.enemyY, data.enemyZ);
            
        }
    }

    private void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); //class to help serialize and deserialize data
        FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate); //open file path for writing

        GameData data = new GameData(player.position, enemy.position, score);

        bf.Serialize(fs, data); //use binary formatter to serialize data at filepath
        fs.Close();
    }
}

