using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatternK : MonoBehaviour
{
    public static UIPatternK instance;

    public RectTransform patternCanvas;
    Vector2 canvasSize = new Vector2(140, 60);
    public List<UIPatternM> patterns;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void DisplayPattern()
    {
        patternCanvas.sizeDelta = new Vector2(canvasSize.x, canvasSize.y * (patterns.Count));  

    }
    public void RemovePattern(UIPatternM pattern)
    {
        pattern.rt.parent = null;
        patterns.Remove(pattern);
        //debug 
        //pattern.gameObject.SetActive(false);
        DisplayPattern();
    }
    public void AddPattern(UIPatternM pattern)
    {
        pattern.rt.parent = patternCanvas;
        patterns.Add(pattern);
        DisplayPattern();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Mathf.Ceil(patternCanvas.sizeDelta.y/canvasSize.y); i++)
        {
            //patterns[i].rt.parent = patternCanvas;
            Vector3 pos = new Vector3(patternCanvas.position.x, patternCanvas.position.y- patternCanvas.sizeDelta.y, patternCanvas.position.z) ;
            Gizmos.DrawSphere(pos, 5f);
        }
    }
}
