using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement ui;
    // References to main menu UI elements:
    public Button playButton;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        playButton = ui.Q<Button>("PlayButton");
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
    }

    private void OnPlayButtonClicked(ClickEvent evt)
    {
        Debug.Log("OnPlayButtonClicked");
        gameObject.SetActive(false);
    }
}
