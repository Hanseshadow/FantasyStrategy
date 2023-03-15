using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueRelay : MonoBehaviour
{
    public Slider SourceSlider;
    public TextMeshProUGUI TargetLabel;

    public void OnLabelChange()
    {
        if(SourceSlider == null || TargetLabel == null)
            return;

        TargetLabel.text = SourceSlider.value.ToString();
    }
}
