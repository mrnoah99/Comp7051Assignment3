using System;
using UnityEngine;

public class MazeGameController : MonoBehaviour
{
    [SerializeField]
    private EnemyAIController enemyPrefab;
    [SerializeField]
    private AudioSource[] deathSounds;
    [SerializeField]
    private AudioSource respawnSound;

    private float time = 0;
    private bool enemyDead = false;
    private int enemyDeathCount = 0;
    private EnemyAIController enemy;

    void Start()
    {
        // enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(0, 5), 1, Random.Range(0, 5)), Quaternion.identity);
        // enemy.gameController = this;
    }

    void Update()
    {
        if (enemyDead)
        {
            time += Time.deltaTime;
        }
        if (time >= 5)
        {
            enemy = Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(0, 5), 1, UnityEngine.Random.Range(0, 5)), Quaternion.identity);
            enemy.gameController = this;
            respawnSound.Play();
            enemyDead = false;
            time = 0;
        }
    }

    public void EnemyDeath()
    {
        deathSounds[Math.Min(enemyDeathCount, deathSounds.Length - 1)].Play();
        enemyDeathCount++;
        enemyDead = true;
    }
}
