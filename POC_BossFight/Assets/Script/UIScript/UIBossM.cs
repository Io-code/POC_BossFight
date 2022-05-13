using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossM : MonoBehaviour
{
    public static UIBossM instance;
    public int patternNb;
    int currPNb;
    public GameObject uiPatternPrefab;
    public List<UIPatternM> patternsM;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void RemovePattern(UIPatternM pattern)
    {

    }
    public void AddPattern(UIPatternM pattern)
    {

    }
}
