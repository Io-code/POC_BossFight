using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPatternM : MonoBehaviour
{
    public RectTransform rt;
    public Image hightLight;
    public Image buttonIconSettings;
    [HideInInspector] public float intensity = 0;
    [Header("Pattern")]
    public ScriptablePattern patternScr;

    public TextMeshProUGUI nameT;
    public Image icon;
    public bool attribute;

    public Color selectColor, normalColor;
    [HideInInspector]public bool displayS;
    private void Awake()
    {
        if(patternScr)
            UpdateVisual();
    }
    public void SwitchPattern()
    {
        attribute = !attribute;
        if (attribute)
        {
            if(UIBossM.instance.patterns.Count < UIBossM.instance.maxPattern)
            {
                buttonIconSettings.gameObject.SetActive(true);
                UIBossM.instance.AddPattern(this);
                UIPatternK.instance.RemovePattern(this);
            }
            
        }
        else 
        {
            displayS = false;
            buttonIconSettings.gameObject. SetActive(true);
            UIBossM.instance.DisplayPatternSettings(this, false);

            UIBossM.instance.RemovePattern(this);
            UIPatternK.instance.AddPattern(this);
        }
    }
    public void DisplaySettings()
    {
        if (attribute)
        {
            displayS = !displayS;
            UIBossM.instance.DisplayPatternSettings(this,displayS);
        }
            
    }
    public void UpdateVisual()
    {
        nameT.text = (" " + patternScr.name);
        icon.sprite = patternScr.icon;
    }
}
