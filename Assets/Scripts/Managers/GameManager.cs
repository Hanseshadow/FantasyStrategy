using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsGameStarted = false;
    public bool IsInUI = true;
    public bool IsGamePaused = false;

    // Load all the UI.  We have plenty of memory to spare.  If we don't, then this can be offloaded easily.
    public GameObject MainMenu;
    public GameObject LoadingScreen;
    public GameObject GameScreen;
    public GameObject UnitUI;
    public GameObject MapCreation;
    public GameObject Settings;

    public MapGrid MapGenerator;
    public ArmyManager Armies;

    public enum UIScreens
    {
        None,
        MainMenu,
        LoadingScreen,
        GameScreen,
        Settings,
        City,
        Spellbook,
        Statistics,
        UnitUI,
        MapCreation,
    }

    public UIScreens ShownScreen = UIScreens.MainMenu;
    public UIScreens LastScreen = UIScreens.MainMenu;

    void Start()
    {
        if(MainMenu != null)
            ShowMainMenu();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !IsInUI)
        {
            ShowSettings();
        }
    }

    public void ShowMainMenu()
    {
        ShowScreen(UIScreens.MainMenu);
    }

    public void ShowScreen(UIScreens screen)
    {
        HideScreens();

        LastScreen = ShownScreen;

        switch(screen)
        {
            case UIScreens.MainMenu:
                MainMenu.SetActive(true);
                IsInUI = true;
                Time.timeScale = 0f;
                break;
            case UIScreens.LoadingScreen:
                LoadingScreen.SetActive(true);
                IsInUI = true;
                Time.timeScale = 0f;
                break;
            case UIScreens.GameScreen:
                GameScreen.SetActive(true);
                UnitUI.SetActive(true);
                IsInUI = false;
                Time.timeScale = 1f;
                break;
            case UIScreens.MapCreation:
                MapCreation.SetActive(true);
                IsInUI = true;
                Time.timeScale = 0f;
                break;
            case UIScreens.Settings:
                Settings.SetActive(true);
                IsInUI = true;
                Time.timeScale = 0f;
                break;
        }
    }

    private void HideScreens()
    {
        LoadingScreen.SetActive(false);
        MainMenu.SetActive(false);
        GameScreen.SetActive(false);
        UnitUI.SetActive(false);
        MapCreation.SetActive(false);
        Settings.SetActive(false);
    }

    public void CreateMap()
    {
        ShowScreen(UIScreens.MapCreation);
    }

    public void PlayGame()
    {
        ShowScreen(UIScreens.LoadingScreen);

        if(MapGenerator != null)
        {
            MapGenerator.BeginGeneration(this);
        }
    }

    public void ShowSettings()
    {
        ShowScreen(UIScreens.Settings);
    }

    public void CloseSettings()
    {
        ShowScreen(UIScreens.GameScreen);
    }

    public void LoadGame()
    {
        // Load stuff
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
    }

    public void LoadingFinished()
    {
        ShowScreen(UIScreens.GameScreen);
        IsGameStarted = true;
        IsInUI = false;

        FindObjectOfType<CameraMapMovement>()?.Initialize();
    }
}
