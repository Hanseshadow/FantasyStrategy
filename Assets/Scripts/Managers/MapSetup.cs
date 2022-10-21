using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    private GameManager GM;

    public void MapSetupInitialize(GameManager gm)
    {
        if(gm == null)
            return;

        GM = gm;
    }

    public void StartGame()
    {
        GM.PlayGame();
    }

    public void CancelSetup()
    {
        GM.ShowScreen(GameManager.UIScreens.MainMenu);
    }
}
