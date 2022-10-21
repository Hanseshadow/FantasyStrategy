using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsGameStarted = false;
    public bool IsInUI = true;

    public GameObject MainMenu;
    public GameObject LoadingScreen;
    public GameObject GameScreen;
    public GameObject UnitUI;
    public GameObject MapCreation;

    public MapGrid MapGenerator;

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

    void Start()
    {
        if(MainMenu != null)
            ShowScreen(UIScreens.MainMenu);
    }

    void Update()
    {
    }

    public void ShowScreen(UIScreens screen)
    {
        HideScreens();

        switch(screen)
        {
            case UIScreens.MainMenu:
                MainMenu.SetActive(true);
                IsInUI = true;
                break;
            case UIScreens.LoadingScreen:
                LoadingScreen.SetActive(true);
                IsInUI = true;
                break;
            case UIScreens.GameScreen:
                GameScreen.SetActive(true);
                UnitUI.SetActive(true);
                IsInUI = false;
                break;
            case UIScreens.MapCreation:
                MapCreation.SetActive(true);
                IsInUI = true;
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

        FindObjectOfType<CameraPanning>()?.Initialize();
    }
}
