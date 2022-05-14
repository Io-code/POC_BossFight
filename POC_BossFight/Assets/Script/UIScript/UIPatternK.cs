using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatternK : MonoBehaviour
{
    public static UIPatternK instance;

    public RectTransform patternCanvas;
    public List<UIPatternM> patterns;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void RemovePattern(UIPatternM pattern)
    {
        //pattern.rt.parent = null;
        patterns.Remove(pattern);
        
    }
    public void AddPattern(UIPatternM pattern)
    {
        pattern.rt.parent = patternCanvas;
        patterns.Add(pattern);
        
    }

}
