using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AntOrientation { LEFT, MIDDLE, RIGHT};
public class AntVisualBehaviour : MonoBehaviour
{
    public AntOrientation Orientation;

    Animator antAnimator;

    readonly string IdleTrigger = "Idle";
    readonly string WrongAnswerTrigger = "Wrong";
    readonly string CorrectAnswerTrigger = "Right";
    readonly string JumpOutTrigger = "JumpOut";

    float jumpOutLength = 0.733f;
    AnteaterGameManager gameManager;

    bool IsUnderGround = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = AnteaterGameManager.Instance;
        antAnimator = GetComponent<Animator>();
    }

    public void SetIdleTrigger()
    {
        antAnimator.SetTrigger(IdleTrigger);
    }

    public void WrongAnswer()
    {
        antAnimator.ResetTrigger(JumpOutTrigger);
        IsUnderGround = true;
        antAnimator.SetTrigger(WrongAnswerTrigger);
    }

    public void CorrectAnswer(bool reset = false)
    {
        if (!reset)
        {
            IsUnderGround = true;
            antAnimator.SetTrigger(CorrectAnswerTrigger);
            gameManager.ResetAntsAfterQuestion(this);
        }
        else
        {
            antAnimator.SetTrigger(CorrectAnswerTrigger);
            IsUnderGround = true;
        }
        
    }

    public void JumpOut()
    {
        if (IsUnderGround)
        {
            antAnimator.SetTrigger(JumpOutTrigger);
            StartCoroutine(WaitForSettingIsUnderGroundBool());
        }
            
    }

    IEnumerator WaitForSettingIsUnderGroundBool()
    {
        yield return new WaitForSeconds(jumpOutLength);
        IsUnderGround = false;
    }

    public bool IsAntUnderground()
    {
        return IsUnderGround;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
