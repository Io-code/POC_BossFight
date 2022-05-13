using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPatternM : MonoBehaviour
{
    public RectTransform rt;
    

    public bool attribute;
    public void SwitchPattern()
    {
        attribute = !attribute;
        if (attribute)
        {
            UIBossM.instance.AddPattern(this);
            UIPatternK.instance.RemovePattern(this);
        }
        else
        {
           UIBossM.instance.RemovePattern(this);
           UIPatternK.instance.AddPattern(this);
        }
    }
}
