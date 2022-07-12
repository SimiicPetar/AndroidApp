using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilloVisualBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate void ArmadilloClicked();
    public ArmadilloClicked onArmadilloClicked;
    public bool interactable = false;

    public bool clicked = false;

    Animator armadilloAnimator;

    LalaLetterMachineManager gameManager;

    LetterMachineAudioManager audioManager;

    readonly string StartShakeTrigger = "StartShake";
    readonly string StopShakeTrigger = "StopShake";

    Task ShakeAnimationTask;
    void Subscribing()
    {
        gameManager.onWordDisplayDone += SetInteractable;
        audioManager.introEnded += SetInteractable;
        audioManager.onTargetWordSaid += DeactivateShakeAnimation;
        onArmadilloClicked += gameManager.StopHandPointerTask;
    }

    private void Start()
    {
        audioManager = LetterMachineAudioManager.Instance;
        armadilloAnimator = GetComponent<Animator>();
        armadilloAnimator.speed = 1f;
        gameManager = LalaLetterMachineManager.Instance; 
        interactable = true;
        Subscribing();
    }

    public void DeactivateShakeAnimation()
    {
        armadilloAnimator.SetTrigger(StopShakeTrigger);
    }

    public void ActivateShake()
    {
        ShakeAnimationTask = new Task(ActivateShakeAnimation());
        armadilloAnimator.SetTrigger(StartShakeTrigger);
    }
    IEnumerator ActivateShakeAnimation()
    {

        armadilloAnimator.SetTrigger(StartShakeTrigger);
        yield return new WaitForSeconds(0f);
        armadilloAnimator.SetTrigger(StopShakeTrigger);
        ShakeAnimationTask.Stop();
    }

    void SetInteractable()
    {
        interactable = true;
    }

    private void OnMouseOver()
    {
        //ovde ide logika za ostatak
        if(Input.GetMouseButtonDown(0) && interactable) {
            audioManager.StopTutorialRepetition();
            gameManager.StartFallingLetters();
            interactable = false;
            clicked = true;
            onArmadilloClicked.Invoke();
            gameManager.DisableHandPointer(false);

        }
    }
}
