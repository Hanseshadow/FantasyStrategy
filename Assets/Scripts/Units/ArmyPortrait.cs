using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyPortrait : MonoBehaviour
{
    public Image Portrait;
    public GameObject PortraitFrame;

    void Start()
    {
    }

    public void OnClicked()
    {
        Debug.Log("Button clicked.");
    }

    public void ChangeFrameColor(ArmyData.ArmyAlignments portraitColor)
    {
        Color color = Color.gray;

        switch(portraitColor)
        {
            case ArmyData.ArmyAlignments.None:
                color = Color.gray;
                break;
            case ArmyData.ArmyAlignments.Friend:
                color = Color.green;
                break;
            case ArmyData.ArmyAlignments.Enemy:
                color = Color.red;
                break;
        }

        Image image = PortraitFrame.GetComponent<Image>();

        if(image != null)
        {
            image.color = color;
        }
    }
}
