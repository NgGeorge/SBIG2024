using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

//this current implementation of main menu relies on clearing and adding ui elements as needed, depending on if we're currently in the main menu or how to play page.
public class MainMenuController : MonoBehaviour
{
    //stuff for mainMenu:
    private VisualElement mainMenu;
    private Button playButton;
    private Button howToPlayButton;
    
    //stuff for howToPlay:
    [SerializeField] private VisualTreeAsset howToPlayTemplate;//drag & drop in inspector
    private VisualElement howToPlay;//create based on the file above
    private Button backButton;

    private void Awake()
    {
        mainMenu = GetComponent<UIDocument>().rootVisualElement;
        SetupMainMenu();
        SetupHowToPlayMenu();
    }
    private void SetupHowToPlayMenu()
    {
        howToPlay = howToPlayTemplate.CloneTree();
        backButton = howToPlay.Q<Button>("backButton");
        backButton.clicked += BackButtonOnClicked;
    }
    private void SetupMainMenu()
    {
        playButton = mainMenu.Q<Button>("playButton");
        playButton.clicked += PlayButtonOnClicked;
        howToPlayButton = mainMenu.Q<Button>("howToPlayButton");
        howToPlayButton.clicked += HowToPlayButtonOnClicked;
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
        mainMenu.Clear();
        mainMenu.Add(howToPlay);
    }
    private void BackButtonOnClicked()
    {
        mainMenu.Clear();
        mainMenu.Add(playButton);
        mainMenu.Add(howToPlayButton);
    }
}
