using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossM : MonoBehaviour
{
    public static UIBossM instance;
    public BossController bossC;
    public UIStatsM statsM;

    [Header("Pattern")]
    public RectTransform patternCanvas;
    [HideInInspector] public int maxPattern = 3;

    public List<UIPatternM> patterns;
    IDictionary<UIPatternM, BossPattern> patternD = new Dictionary<UIPatternM, BossPattern>();
    bool patternDetect;

    [Header("Settings")]
    public UISettingsM settingsG;
    public UIPatternM displayPattern;
    UIPatternM virtualDisplayP;
    #region Suscribe Event
    private void OnEnable()
    {
        BossSwitchState.OnEndAttack += ResetPatternCount;
    }

    private void OnDisable()
    {
        BossSwitchState.OnEndAttack -= ResetPatternCount;
    }
    #endregion

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    private void Update()
    {

        for(int i = 0; i < patterns.Count; i++)
        {
            // setup color
            Color newCol = patterns[i].hightLight.color;
            float alpha = patterns[i].hightLight.color.a;
            float speed = (Time.deltaTime / 0.5f);

            if (bossC.pIndex < bossC.bPatterns.Count && bossC.bPatterns[bossC.pIndex] == patternD[patterns[i]])
            {
                alpha = Mathf.Clamp01(alpha + speed);
                if(patternDetect)
                {
                    patternDetect = false;
                    Debug.Log("Trigger Pattern");
                    statsM.LoadStat(i);
                }
            }  
            else
                alpha = Mathf.Clamp01(alpha - speed);
            newCol = new Color(newCol.r, newCol.g, newCol.b, alpha);
            patterns[i].hightLight.color = newCol;
        }
        
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

        if (virtualDisplayP != null && patternD.ContainsKey(virtualDisplayP))
            settingsG.LoadVisual(patternD[virtualDisplayP]);

        // reset color
        Color newCol = pattern.hightLight.color;
        pattern.hightLight.color = new Color(newCol.r, newCol.g, newCol.b, 0);

        // reset stat
        statsM.ResetProba();
        
    }
    public void AddPattern(UIPatternM pattern)
    {
        pattern.rt.parent = patternCanvas;

        patterns.Add(pattern);
        BossPattern patternB = new BossPattern(pattern.patternScr);
        patternD.Add(pattern, patternB);

        bossC.bPatterns.Add(patternD[pattern]);
        bossC.UpdateNextPattern();

        if (virtualDisplayP && patternD.ContainsKey(virtualDisplayP))
            settingsG.LoadVisual(patternD[virtualDisplayP]);

        // reset stat
        statsM.ResetProba();
    }

    public void DisplayPatternSettings(UIPatternM pattern, bool display)
    {
        settingsG.gameObject.SetActive(display);
        if (!display)
        {
            virtualDisplayP.buttonIconSettings.color = virtualDisplayP.normalColor;
            return;
        }
            

        if (virtualDisplayP && virtualDisplayP != pattern)
        {
            virtualDisplayP.buttonIconSettings.color = virtualDisplayP.normalColor;
            virtualDisplayP.displayS = false;
        }
            

        virtualDisplayP = pattern;

        if (virtualDisplayP)
        {
            displayPattern.patternScr = virtualDisplayP.patternScr;
            displayPattern.UpdateVisual();
            settingsG.LoadVisual(patternD[virtualDisplayP]);
            virtualDisplayP.buttonIconSettings.color = virtualDisplayP.selectColor;
        } 
    }

    void ResetPatternCount( Animator animator)
    {
        if(animator == bossC.animator)
        {
            patternDetect = true;
        }
    }
}
