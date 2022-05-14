using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPatternS : MonoBehaviour
{
    int maxValue = 4;
    public Slider slider;
    public TextMeshProUGUI valueT;
    public Image icon;
    char[] valueCharr;
    private void Awake()
    {
        slider.maxValue = maxValue;
    }
    private void Update()
    {   
        valueCharr = (slider.value.ToString()).ToCharArray(0, Mathf.Clamp( slider.value.ToString().Length,0,3));
        valueT.text = (valueCharr.Length == 3 )?((valueCharr[0] +"."+ valueCharr[2]).ToString()) : valueCharr[0].ToString();
    }
}
