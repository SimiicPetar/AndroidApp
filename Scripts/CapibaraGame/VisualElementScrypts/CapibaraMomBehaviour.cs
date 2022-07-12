using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapibaraMomBehaviour : MonoBehaviour
{
    readonly string LookLeftTrigger = "Turn Left";

    readonly string LookRightTrigger = "Turn Right";

    readonly string LookDownTrigger = "Look Down";

    Animator capibaraMomAnimator;

    AudioManagerCapibara audioManager;

    KapibaraGameManager gameManager;

    private void Start()
    {
        gameManager = KapibaraGameManager.Instance;
        audioManager = AudioManagerCapibara.Instance;
        capibaraMomAnimator = GetComponent<Animator>();
    }

    public void LookInADirection(CapibaraBase capibara)
    {
        /* if (capibara.GetType().Equals(typeof(CapibaraBackBehaviour))){
             LookDown();
         }else if (capibara.GetType().Equals(typeof(CapibaraSideBehaviour)))
         {
             if (((CapibaraSideBehaviour)capibara).orientation == CapibaraOrientation.LEFT)
                 LookLeft();
             else
                 LookRight();
         }*/
        LookDown();
    }

    void LookDown()
    {
    
        capibaraMomAnimator.SetTrigger(LookDownTrigger);
    }

    void LookLeft()
    {
        capibaraMomAnimator.SetTrigger(LookLeftTrigger);
    }

    void LookRight()
    {
        capibaraMomAnimator.SetTrigger(LookRightTrigger);
    }
}
