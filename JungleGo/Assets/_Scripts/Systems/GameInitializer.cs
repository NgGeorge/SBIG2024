using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Pre-Start Initialization");
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Menu Stuff
        Debug.Log("Game Started");
        InitializeGame();
    }

    void InitializeGame()
    {
        // TODO: Do stuff with game manager
        // A lot of the singletons will be loaded with Unity game startup
        // not sure what we need to do here. Menu stuff?
    }
}
