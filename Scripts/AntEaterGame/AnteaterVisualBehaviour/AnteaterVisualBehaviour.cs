using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnteaterVisualBehaviour : MonoBehaviour
{
    // Start is called before the first frame update


    readonly string EatLeftLeafTrigger = "EatLeft";
    readonly string EatMiddleLeafTrigger = "EatMiddle";
    readonly string EatRightLeafTrigger = "EatRight";

    readonly string SpitLeftLeafTrigger = "SpitLeft";
    readonly string SpitMiddleLeafTrigger = "SpitMiddle";
    readonly string SpitRightLeafTrigger = "SpitRight";

    AnteaterGameManager gameManager;

    Animator anteaterAnimator;
    void Start()
    {
        gameManager = AnteaterGameManager.Instance;
        anteaterAnimator = GetComponent<Animator>();
    }

    public void DisableAnts()
    {
        gameManager.DisableMravi(false);
    }

    public void EnableAnts()
    {
        gameManager.DisableMravi(true);
    }

    public void SpitLeaf(AntVisualBehaviour ant)
    {
        var antOrientation = ant.Orientation;
        if (antOrientation == AntOrientation.LEFT)
            anteaterAnimator.SetTrigger(SpitLeftLeafTrigger);
        else if (antOrientation == AntOrientation.MIDDLE)
            anteaterAnimator.SetTrigger(SpitMiddleLeafTrigger);
        else
            anteaterAnimator.SetTrigger(SpitRightLeafTrigger);
    }

    public void EatLeaf(AntVisualBehaviour ant)
    {
        var antOrientation = ant.Orientation;
        if (antOrientation == AntOrientation.LEFT)
            anteaterAnimator.SetTrigger(EatLeftLeafTrigger);
        else if (antOrientation == AntOrientation.MIDDLE)
            anteaterAnimator.SetTrigger(EatMiddleLeafTrigger);
        else
            anteaterAnimator.SetTrigger(EatRightLeafTrigger);
    }
}
