using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsM : MonoBehaviour
{
    public UIPatternS[] patternSettings;
    [HideInInspector] public BossPattern bossP;

    public void LoadVisual( BossPattern bossP)
    {
        this.bossP = bossP;
        for(int i = 0; i < patternSettings.Length; i++)
        {
            if (i < bossP.nextPatterns.Count)
            {
                patternSettings[i].slider.value = bossP.nextPatterns[i].weight;
                patternSettings[i].gameObject.SetActive(true);
            }
            else
                patternSettings[i].gameObject.SetActive(false);
        }
    }
}
