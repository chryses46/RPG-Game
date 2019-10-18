using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerTesting : MonoBehaviour
{
    EventSystem eventSystem;

    void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }
    void Update()
    {
        //ProcessControllerInput();
    }

    private void ProcessControllerInput()
    {
        string name = eventSystem.currentInputModule.name;
        
        Debug.Log(name);
    }
}
