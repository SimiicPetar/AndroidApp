using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarNodeLogic : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject BackGroundBeforeAnswered;
    SpriteRenderer BackgroundWhenAnswered;
    Color correctColor;
    Color wrongColor;
    Animator nodeAnimator;



    private void Awake()
    {
        nodeAnimator = GetComponent<Animator>();
        BackgroundWhenAnswered = GetComponent<SpriteRenderer>();
        BackgroundWhenAnswered.color = correctColor;
    }

    private void Start()
    {
        
        correctColor = ProgressBarLogic.Instance.CorrectColor;
        Debug.Log(correctColor);
        wrongColor = ProgressBarLogic.Instance.WrongColor;
    }

    public void SetBackground(bool correct)
    {
        nodeAnimator.SetTrigger("ShowUp");
        //var temp = BackgroundWhenAnswered.color;
        //  temp.a = 1;
        // BackgroundWhenAnswered.color = temp;

        // BackGroundBeforeAnswered.SetActive(false);
        
        if (correct)
            BackgroundWhenAnswered.DOColor(correctColor, 0.5f);
        else
            BackgroundWhenAnswered.DOColor(wrongColor, 0.5f);
    }

    public void ResetBackground()
    {
        nodeAnimator.SetTrigger("Reset");
          BackgroundWhenAnswered.DOColor(new Color(0, 0, 0, 0), 0.5f);
    }
}
