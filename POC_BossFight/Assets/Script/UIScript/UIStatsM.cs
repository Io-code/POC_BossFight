using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatsM : MonoBehaviour
{
    [Header("Pattern")]
    public int totalPattern;
    public int[] patternsC = new int[3];
    [Header("Display")]
    [Range(0, 1)]
    public float[] probaP = new float[3];
    public Image[] probaImg = new Image[3];
    public TextMeshProUGUI patternCount;

    private void Update()
    {
        for(int i = 0; i < probaImg.Length; i++)
        {
            float amount = 0;
            for(int j = 0; j < probaP.Length -i; j++)
                amount += probaP[j+i];

            probaImg[i].fillAmount = amount;
        }
    }

    public void ResetProba()
    {
        totalPattern = 0;
        for (int i = 0; i < probaP.Length; i++)
        {
            probaP[i] = 0;
            patternsC[i] = 0;
        }
        patternCount.text = totalPattern.ToString();
    }

    public void LoadStat(int patternIndex)
    {
        patternsC[patternIndex]++;
        totalPattern++;

        // update proba
        for (int i = 0; i < probaP.Length; i++)
        {
            probaP[i] = (float)patternsC[i] / totalPattern;
        }
        patternCount.text = totalPattern.ToString();

    }

}
