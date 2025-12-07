using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConsoleCommandsHandler : MonoBehaviour
{
    // Prefab objects assigned in the editor for the AI and Player2
    public GameObject aiPrefab;
    public GameObject p2Prefab;

    public static bool AIEnabled;
    public GameObject ball;

    // Used to list available commands in the console
    private Dictionary<string, Action<string[]>> commands = new();

    void Awake()
    {
        Register("bg_colour", SetBackgroundColour);
        Register("toggle_ai", ToggleAI);
        Register("exit", ExitConsole);
    }

    // Adds new commands to the list of available options
    public void Register(string name, Action<string[]> callback)
    {
        commands[name.ToLower()] = callback;
    }

    // Runs a command when provided
    public void Execute(string input)
    {
        string[] parts = input.Split(' ');
        string cmd = parts[0].ToLower();
        string[] args = parts.Length > 1 ? parts[1..] : Array.Empty<string>();

        if (commands.TryGetValue(cmd, out var action))
        {
            action.Invoke(args);
        }
        else
        {
            Debug.LogWarning($"Unknown command: {cmd}");
        }
    }

    // Toggles between Player2 and an AI
    private void ToggleAI(string[] args)
    {
        if (AIEnabled)
        {
            // Switch to player
            Debug.Log("Switching to Player 2");
            GameObject obj = GameObject.Find("AI(Clone)");
            Vector3 pos = obj.transform.position;
            Destroy(obj);
            GameObject newObj = Instantiate(p2Prefab);
            newObj.transform.position = pos;
            AIEnabled = false;
        }
        else
        {
            // Switch to AI
            Debug.Log("Switching to AI");
            GameObject obj = GameObject.Find("Player2(Clone)");
            Vector3 pos = obj.transform.position;
            Destroy(obj);
            GameObject newObj = Instantiate(aiPrefab);
            newObj.transform.position = pos;
            newObj.GetComponent<CPUController>().ball = ball;
            AIEnabled = true;
        }
    }

    // Changes background colour. Hopefully this is enough options for now
    private void SetBackgroundColour(string[] args)
    {
        if (args.Length == 0) return;

        string colorName = args[0].ToLower();
        Color newColor;

        switch (colorName)
        {
            case "red": newColor = Color.red; break;
            case "green": newColor = Color.green; break;
            case "blue": newColor = Color.blue; break;
            case "black": newColor = Color.black; break;
            case "white": newColor = Color.white; break;
            case "yellow": newColor = Color.yellow; break;
            case "purple": newColor = Color.magenta; break;
            default:
                Debug.LogWarning($"Unknown color: {colorName}");
                return;
        }

        Camera.main.backgroundColor = newColor;
    }

    /* Technically doesn't have to do anything because the console exits on every
        command automatically, but I added it just in case an error would happen
        because it can't find the command provided.*/
    private void ExitConsole(string[] args)
    {
        Debug.Log("Console exited");
    }
}
