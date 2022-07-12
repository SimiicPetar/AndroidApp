using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapibaraBackBehaviour : CapibaraBase
{

    Animator capibaraAnimator;
    public CapibaraMomBehaviour capiMom;
    KapibaraGameManager gameManager;

    void Start()
    {
        gameManager = KapibaraGameManager.Instance;
        capibaraAnimator = GetComponent<Animator>();
    }

    public override void Jump()
    {
        capibaraAnimator.SetTrigger(JumpTrigger);
        gameManager.StartReset(this);
    }

    public override void IdleFar()
    {
        capiMom.LookInADirection(this);
        capibaraAnimator.SetTrigger(IdleFarTrigger);
    }

    public override void JumpTobush()
    {
        Debug.Log("Jumptobush event se okinuo");
        capibaraAnimator.SetTrigger(JumpToBushTrigger);
      
    }

    public override void ResetForNewQuestion()
    {
        if (!gameManager.lastTrial)
            capibaraAnimator.SetTrigger(ResetTrigger);
        else
        {
            StartCoroutine(TurnOffCapibaraAtTheEnd());
        }
    }

    IEnumerator TurnOffCapibaraAtTheEnd()
    {
        capibaraAnimator.SetTrigger(ResetTrigger);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }


    IEnumerator SmallDelay(float delay)
    {
        yield return new WaitForSeconds(0);
        if (!gameManager.lastTrial)
        {
            capibaraAnimator.SetTrigger(JumpBackInTrigger);
            gameManager.StartNewWordCapibara();
        }
        else
        {
            gameManager.StartNewWordCapibara();
            gameObject.SetActive(false);
            yield return null;
        }
    }

    public override void JumpBackIn()
    {
        isFeared = false;
        StartCoroutine(SmallDelay(1f));
    }

    public override void Idle()
    {
        capibaraAnimator.SetTrigger(IdleTrigger);
    }

    public override void Fear()
    {
        capibaraAnimator.SetTrigger(FearTrigger);
        isFeared = true;
    }

    public override bool IsFeared()
    {
        return isFeared;
    }
}
