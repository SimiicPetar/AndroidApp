using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CapibaraOrientation { LEFT, RIGHT};
public class CapibaraSideBehaviour : CapibaraBase
{
    Animator capibaraAnimator;

    public CapibaraOrientation orientation;

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
        capibaraAnimator.SetTrigger(JumpToBushTrigger);
        
    }

    IEnumerator SmallDelay(float delay)
    {
       // transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
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
    
    



