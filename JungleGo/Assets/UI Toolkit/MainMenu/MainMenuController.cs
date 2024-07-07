using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//todo: a label which shows your score (accuracy)
//todo: endscreen which has a label that tells you whether you've won or lost.
//todo: a button for next, which opens the main menu again (assumption: the game is paused in the background and doesn't start).
//todo: improve flavor text.

//this current implementation of main menu relies on clearing and adding ui elements as needed, depending on if we're currently in the main menu or how to play page.

public class MainMenuController : MonoBehaviour
{
    //stuff for mainMenu:
    private VisualElement mainMenu;
    private VisualElement mainMenuDynamicSection;
    private Button playButton;
    private Button howToPlayButton;

    //todo: mainMenuTemplate using the below technique, which will be called when starting a new level.
    
    //stuff for howToPlay:
    [SerializeField] private VisualTreeAsset howToPlayTemplate;//drag & drop in inspector
    private VisualElement howToPlay;//create based on the file above
    private Button backButton;

    //stuff for endScreenTemplate:
    [SerializeField] private VisualTreeAsset endScreenTemplate;//drag & drop in inspector
    //private VisualElement howToPlay;//create based on the file above
    //private Button backButton;
    
    private void Awake()
    {
        mainMenu = GetComponent<UIDocument>().rootVisualElement;
        mainMenuDynamicSection = mainMenu.Q<VisualElement>("mainMenuDynamicSection");
        SetupMainMenu();
        SetupHowToPlayMenu();
    }
    //GameManager will call this method when the game ends:
    public void ShowEndScreen()
    {

    }
    private void SetupMainMenu()
    {
        playButton = mainMenuDynamicSection.Q<Button>("playButton");
        playButton.clicked += PlayButtonOnClicked;
        howToPlayButton = mainMenuDynamicSection.Q<Button>("howToPlayButton");
        howToPlayButton.clicked += HowToPlayButtonOnClicked;
    }
    private void SetupHowToPlayMenu()
    {
        howToPlay = howToPlayTemplate.CloneTree();
        backButton = howToPlay.Q<Button>("backButton");
        backButton.clicked += BackButtonOnClicked;
    }
    private void PlayButtonOnClicked()
    {
        mainMenu.Clear();
        //Alternate approaches:
        //gameObject.SetActive(false);
        //SceneManager.LoadScene("FirstLevel");
    }
    private void HowToPlayButtonOnClicked()
    {
        mainMenuDynamicSection.Clear();
        mainMenuDynamicSection.Add(howToPlay);
    }
    private void BackButtonOnClicked()
    {
        mainMenuDynamicSection.Clear();
        mainMenuDynamicSection.Add(playButton);
        mainMenuDynamicSection.Add(howToPlayButton);
    }

    //testing endscreen logic. end game -> end screen -> loop back to start screen
}
