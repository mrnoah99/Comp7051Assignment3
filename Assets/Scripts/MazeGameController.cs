using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    [SerializeField]
    private Material east;
    [SerializeField]
    private Material west;
    [SerializeField]
    private Material north;
    [SerializeField]
    private Material south;
    [SerializeField]
    private Material floor;
    [SerializeField]
    private Material door;
    [SerializeField]
    private Material doorTrim;
    [SerializeField]
    private Light sceneLight;

    private float time = 0;
    private bool enemyDead = false;
    private int enemyDeathCount = 0;
    private EnemyAIController enemy;
    private InputActions inputActions;
    private InputAction pausePlay;
    private InputAction day;
    private InputAction night;
    private InputAction flashlight;
    private InputAction fogToggle;
    private bool flashlightEnabled = false;
    private bool isPlaying = true;
    private bool fogEnabled = false;
    private MazeCreator creator;

    void Start()
    {
        inputActions = new();
        pausePlay = inputActions.Player.PausePlayMusic;
        day = inputActions.Player.DayMusic;
        night = inputActions.Player.NightMusic;
        flashlight = inputActions.Player.Flashlight;
        fogToggle = inputActions.Player.FogToggle;

        pausePlay.performed += PausePlay;
        pausePlay.Enable();

        day.performed += Day;
        day.Enable();

        night.performed += Night;
        night.Enable();

        flashlight.performed += FlashLight;
        flashlight.Enable();

        fogToggle.performed += ToggleFog;
        fogToggle.Enable();

        creator = gameObject.GetComponent<MazeCreator>();
        if (enemy == null)
        {
            enemy = FindFirstObjectByType<EnemyAIController>();
            StartDay();
        }
    }

    private void Day(InputAction.CallbackContext context)
    {
        enemy.ChangeToDay();
        east.SetFloat("_Day", 1);
        west.SetFloat("_Day", 1);
        north.SetFloat("_Day", 1);
        south.SetFloat("_Day", 1);
        floor.SetFloat("_Day", 1);
        door.SetFloat("_Day", 1);
        doorTrim.SetFloat("_Day", 1);
        sceneLight.intensity = 1;
        sceneLight.colorTemperature = 5000;
        sceneLight.color = Color.white;
        UnityEngine.RenderSettings.skybox.SetFloat("_Exposure", 1f);
    }

    private void StartDay()
    {
        enemy.ChangeToDay();
        east.SetFloat("_Day", 1);
        west.SetFloat("_Day", 1);
        north.SetFloat("_Day", 1);
        south.SetFloat("_Day", 1);
        floor.SetFloat("_Day", 1);
        door.SetFloat("_Day", 1);
        doorTrim.SetFloat("_Day", 1);
        sceneLight.intensity = 1;
        sceneLight.colorTemperature = 5000;
        sceneLight.color = Color.white;
        UnityEngine.RenderSettings.skybox.SetFloat("_Exposure", 1f);
    }

    private void Night(InputAction.CallbackContext context)
    {
        enemy.ChangeToNight();
        east.SetFloat("_Day", 0);
        west.SetFloat("_Day", 0);
        north.SetFloat("_Day", 0);
        south.SetFloat("_Day", 0);
        floor.SetFloat("_Day", 0);
        door.SetFloat("_Day", 0);
        doorTrim.SetFloat("_Day", 0);
        sceneLight.intensity = 0.025f;
        sceneLight.colorTemperature = 10000;
        sceneLight.color = Color.gray;
        UnityEngine.RenderSettings.skybox.SetFloat("_Exposure", 0.2f);
    }

    private void FlashLight(InputAction.CallbackContext context)
    {
        if (flashlightEnabled)
        {
            east.SetFloat("_Flashlight", 0);
            west.SetFloat("_Flashlight", 0);
            north.SetFloat("_Flashlight", 0);
            south.SetFloat("_Flashlight", 0);
            floor.SetFloat("_Flashlight", 0);
            door.SetFloat("_Flashlight", 0);
            doorTrim.SetFloat("_Flashlight", 0);
        } else
        {
            east.SetFloat("_Flashlight", 1);
            west.SetFloat("_Flashlight", 1);
            north.SetFloat("_Flashlight", 1);
            south.SetFloat("_Flashlight", 1);
            floor.SetFloat("_Flashlight", 1);
            door.SetFloat("_Flashlight", 1);
            doorTrim.SetFloat("_Flashlight", 1);
        }
        flashlightEnabled = !flashlightEnabled;
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

    private void ToggleFog(InputAction.CallbackContext context)
    {
        if (fogEnabled)
        {
            // disable fog? dunno if that's handled here
            enemy.musicDay.volume *= 2;
            enemy.musicNight.volume *= 2;
        } else
        {
            // enable fog? dunno if that's handled here
            enemy.musicDay.volume /= 2;
            enemy.musicNight.volume /= 2;
        }
        fogEnabled = !fogEnabled;
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
            enemy.walkPointRangeX = creator.mazeDepth;
            enemy.walkPointRangeZ = creator.mazeWidth;
            enemy.MusicOnRespawn();
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

    public void ResetKills()
    {
        enemyDeathCount = 0;
    } 
}
