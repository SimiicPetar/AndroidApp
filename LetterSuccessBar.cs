using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterSuccessBar : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LetterText;
    public List<GameObject> SuccessIndicatorList;

    public void FillBar(float numberOfSuccess, char letter)
    {
        ResetBar();
        LetterText.text = letter.ToString();
        if (numberOfSuccess > 8)
            numberOfSuccess = 8;
        for(int i = 0; i < numberOfSuccess; i++)
        {
            SuccessIndicatorList[i].SetActive(true);
        }
        ScoreText.text = $"{numberOfSuccess}/8";
    }

    private void ResetBar()
    {
        for (int i = 0; i < SuccessIndicatorList.Count; i++)
        {
            SuccessIndicatorList[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
