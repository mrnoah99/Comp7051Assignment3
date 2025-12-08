using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MazeGameController : MonoBehaviour
{
    [SerializeField]
    private EnemyAIController enemyPrefab;
    [SerializeField]
    private AudioSource[] deathSounds;
    [SerializeField]
    private AudioSource respawnSound;
    [SerializeField]
    private Transform deathSoundsPos;
    [SerializeField]
    private Transform respawnSoundPos;

    private float time = 0;
    private bool enemyDead = false;
    private int enemyDeathCount = 0;
    private EnemyAIController enemy;
    private InputActions inputActions;
    private InputAction pausePlay;
    private InputAction day;
    private InputAction night;
    private bool isPlaying = true;
    private MazeCreator creator;

    void Start()
    {
        inputActions = new();
        pausePlay = inputActions.Player.PausePlayMusic;
        day = inputActions.Player.DayMusic;
        night = inputActions.Player.NightMusic;

        pausePlay.performed += PausePlay;
        pausePlay.Enable();

        day.performed += DayMusic;
        day.Enable();

        night.performed += NightMusic;
        night.Enable();

        creator = gameObject.GetComponent<MazeCreator>();
    }

    private void DayMusic(InputAction.CallbackContext context)
    {
        enemy.ChangeToDay();
    }

    private void NightMusic(InputAction.CallbackContext context)
    {
        enemy.ChangeToNight();
    }

    private void PausePlay(InputAction.CallbackContext context)
    {
        if (isPlaying)
        {
            enemy.Pause();
        } else
        {
            enemy.Play();
        }
        isPlaying = !isPlaying;
    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = FindFirstObjectByType<EnemyAIController>();
        }

        if (enemyDead)
        {
            time += Time.deltaTime;
        }
        if (time >= 5)
        {
            enemy = Instantiate(enemyPrefab, new Vector3(UnityEngine.Random.Range(0, 5), 1, UnityEngine.Random.Range(0, 5)), Quaternion.identity);
            enemy.gameController = this;
            enemy.walkPointRangeX = creator.mazeDepth;
            enemy.walkPointRangeZ = creator.mazeWidth;
            respawnSoundPos.position = enemy.transform.position;
            respawnSound.Play();
            enemyDead = false;
            time = 0;
        }
    }

    public void EnemyDeath()
    {
        deathSoundsPos.position = enemy.transform.position;
        deathSounds[Math.Min(enemyDeathCount, deathSounds.Length - 1)].Play();
        enemyDeathCount++;
        enemyDead = true;
    }


}
