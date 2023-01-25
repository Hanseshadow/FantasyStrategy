using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    GameManager GM;

    void Start()
    {
        GM = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(GM == null)
            return;

        if(Input.GetKeyDown(KeyCode.Escape) && GM.IsInUI && GM.IsGameStarted)
        {
            GM.CloseSettings();
        }
    }
}
