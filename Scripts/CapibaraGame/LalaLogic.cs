using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LalaLogic : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI lalaText;


    private void Awake()
    {
        lalaText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetQuestionResultText(bool positive)
    {
    }

    public void SetText(string text)
    {
        lalaText.text = text;
    }

    public void SetIntroText()
    {
 
    }

}
