using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManateeVisualBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    readonly string SwimInCorrectTrigger = "SwimInCorrect";
    readonly string SwimInWrongTrigger = "SwimInWrong";
    readonly string SwimOutTrigger = "SwimOut";

    bool IsUnderWater = false;

    Animator manateeAnimator;
    private void Awake()
    {
        manateeAnimator = GetComponent<Animator>();
        manateeAnimator.enabled = false;
    }
    void Start()
    {
        StartCoroutine(AnimatorDesync());
    }

    private IEnumerator AnimatorDesync()
    {   
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.8f));
        manateeAnimator.enabled = true;
    }

    public void SwimOut()
   {
        if (IsUnderWater)
        {
            IsUnderWater = false;
            manateeAnimator.SetTrigger(SwimOutTrigger);
        }
        
   }


    public bool IsManateeUnderWater()
    {
        return IsUnderWater;
    }
   public void SwimIn(bool correct)
    {
        
        if (correct)
        {
            if (!IsUnderWater)
            {
                IsUnderWater = true;
                manateeAnimator.SetTrigger(SwimInCorrectTrigger);
            }
            
        }
        else
        {
            IsUnderWater = true;
            manateeAnimator.SetTrigger(SwimInWrongTrigger);
        }
            
        //manateeAnimator.SetTrigger(SwimInTrigger);
    } 
}
