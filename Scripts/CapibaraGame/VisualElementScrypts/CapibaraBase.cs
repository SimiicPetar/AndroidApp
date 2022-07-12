using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CapibaraBase : MonoBehaviour
{
    protected readonly string JumpTrigger = "Jump";

    protected readonly string JumpToBushTrigger = "JumpToBush";

    protected readonly string JumpBackInTrigger = "JumpBackIn";

    protected readonly string IdleTrigger = "Idle";

    protected readonly string IdleFarTrigger = "IdleFar";

    protected readonly string FearTrigger = "Fear";

    protected readonly string ResetTrigger = "ResetForNewQuestion";

    protected bool isFeared = false;

    public abstract void Jump();

    public abstract void ResetForNewQuestion();

    public abstract void IdleFar();

    public abstract void JumpTobush();

    public abstract void JumpBackIn();

    public abstract void Idle();

    public abstract void Fear();

    public abstract bool IsFeared();
}
