using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    readonly string TalkTrigger = "Talk";

    Animator turtleAnimator;

    private void Start()
    {
        turtleAnimator = GetComponent<Animator>();
    }
    public void Talk()
    {
        turtleAnimator.SetTrigger(TalkTrigger);
    }
}
