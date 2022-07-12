using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueMacaoVisualBehaviour : MonoBehaviour
{
    readonly string IdleTrigger = "IdleTrigger";
    readonly string ThumbsUpTrigger = "ThumbsUpTrigger";
    readonly string WrongTrigger = "No";
    readonly string HiTrigger = "Hi";
    readonly string PointingTrigger = "Pointing";

    public bool interactable = true;
    public bool clicked = false;
    Animator macaoAnimator;
    ManateeGameManager gameManager;
    ManateeAudioManager audioManager;

    private void Start()
    {
        gameManager = ManateeGameManager.Instance;
        audioManager = ManateeAudioManager.Instance;
        macaoAnimator = GetComponent<Animator>();
        audioManager.wordSoundsOverEvent += SetInteractable;
        interactable = true;
    }

    void SetInteractable(bool enable)
    {
        interactable = true;
    }

    public void ResetToIdle()
    {
        macaoAnimator.SetTrigger(IdleTrigger);
    }

    public void ShowThumbsUp()
    {
        macaoAnimator.SetTrigger(ThumbsUpTrigger);
    }

    public void Wrong()
    {
        macaoAnimator.SetTrigger(WrongTrigger);
    }

    public void SayHi()
    {
        macaoAnimator.SetTrigger(HiTrigger);
    }

    public void Point()
    {
        macaoAnimator.SetTrigger(PointingTrigger);
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && interactable){

                audioManager.StopTutorialRepetition();
                gameManager.sign.SetInteractable(false);
                gameManager.RepeatWordSounds();
                gameManager.sign.SetInteractable(false);
                clicked = true;
                interactable = false;           
        }
    }
}
