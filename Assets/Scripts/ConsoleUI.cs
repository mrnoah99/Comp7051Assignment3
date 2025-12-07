using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class ConsoleUI : MonoBehaviour {
    public TMP_InputField inputField;
    public GameObject consolePanel;
    public ConsoleCommandsHandler consoleHandler;

    void Start()
    {
        consolePanel.SetActive(false);
    }

    // Constantly checks for keyboard input for opening/closing the console
    void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame && !inputField.isFocused)
        {
            consolePanel.SetActive(!consolePanel.activeSelf);
            Debug.Log("Console opened/closed");
        }
    }

    // Resets text and deactivates the console when a command is entered
    public void OnSubmit()
    {
        string command = inputField.text;
        consoleHandler.Execute(command);
        inputField.text = "";
        consolePanel.SetActive(false);
    }
}