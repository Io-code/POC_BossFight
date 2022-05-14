using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossM : MonoBehaviour
{
    public static UIBossM instance;
    public BossController bossC;

    [Header("Pattern")]
    public RectTransform patternCanvas;
    [HideInInspector] public int maxPattern = 3;

    public List<UIPatternM> patterns;
    IDictionary<UIPatternM, BossPattern> patternD = new Dictionary<UIPatternM, BossPattern>();
    

    [Header("Settings")]
    public UISettingsM settingsG;
    public UIPatternM displayPattern;
    UIPatternM virtualDisplayP;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void RemovePattern(UIPatternM pattern)
    {
        patterns.Remove(pattern);

        bossC.bPatterns.Remove(patternD[pattern]);
        bossC.UpdateNextPattern();
        patternD.Remove(pattern);

        // display
        if (patterns.Count <= 0)
            DisplayPatternSettings(null, false);

        if(virtualDisplayP)
            settingsG.LoadVisual(patternD[virtualDisplayP]);
    }
    public void AddPattern(UIPatternM pattern)
    {
        pattern.rt.parent = patternCanvas;

        patterns.Add(pattern);
        BossPattern patternB = new BossPattern(pattern.patternScr);
        patternD.Add(pattern, patternB);

        bossC.bPatterns.Add(patternD[pattern]);
        bossC.UpdateNextPattern();

        if (virtualDisplayP)
            settingsG.LoadVisual(patternD[virtualDisplayP]);
    }

    public void DisplayPatternSettings(UIPatternM pattern, bool display)
    {
        if (virtualDisplayP && virtualDisplayP != pattern)
            virtualDisplayP.displayS = false;

        settingsG.gameObject.SetActive(display);
        virtualDisplayP = pattern;

        if (virtualDisplayP)
        {
            displayPattern.patternScr = virtualDisplayP.patternScr;
            displayPattern.UpdateVisual();
            settingsG.LoadVisual(patternD[virtualDisplayP]);
        } 
    }
}
