using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement uiMainMenu;

    private Button playButton;
    private Button howToPlayButton;
    private Label howToPlayLabel;

    //maybe: use Resources.Load to show a popup.uxml
    //private UIDocument howToPlayDocument;//fill this in with the HowToPlay popup in Inspector?
    //private VisualElement uiHowToPlayPopup;

    private void Awake()
    {     
        uiMainMenu = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        //get references to the UI elements we are going to manipulate:
        playButton = uiMainMenu.Q<Button>("playButton");
        howToPlayButton = uiMainMenu.Q<Button>("howToPlayButton");
        howToPlayLabel = uiMainMenu.Q<Label>("howToPlayLabel");

        //assign logic to ui elements:
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        howToPlayButton.RegisterCallback<ClickEvent>(OnHowToPlayButtonClicked);
    }

    private void OnPlayButtonClicked(ClickEvent evt)
    {
        gameObject.SetActive(false);
    }

    private void OnHowToPlayButtonClicked(ClickEvent evt)
    {
        //todo
        howToPlayLabel.text = "this is how you play the game";
    }

    private void ShowHowToPlayPopup(ClickEvent evt)
    {
        
        //    howToPlayDocument = Resources.Load<UIDocument>("HowToPlayInstructions"); // Load popup document
        //    if (howToPlayDocument != null)
        //    {
        //        Debug.Log("HowToPlayInstructions.uxml loaded successfully!");
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to load HowToPlayInstructions.uxml!");
        //    }
        //    //uiHowToPlayPopup = howToPlayDocument.rootVisualElement;



        //    //how to play popup logic:
        //    howToPlayButton = uiMainMenu.Q<Button>("HowToPlayButton");
        //    howToPlayButton.RegisterCallback<ClickEvent>(ShowHowToPlayPopup);

        // Initially hide the popup
        //uiHowToPlayPopup.style.display = DisplayStyle.None;


        //uiHowToPlayPopup.style.display = DisplayStyle.Flex;
    }
}
