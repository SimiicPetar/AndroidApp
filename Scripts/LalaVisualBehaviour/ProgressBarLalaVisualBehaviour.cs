using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressBarLalaVisualBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    readonly string IdleTrigger = "Idle";
    readonly string CorrectTrigger = "ThumbsUp";
    readonly string WrongTrigger = "No!";
    readonly string TalkingTrigger = "Talk";
    public bool interactable = false;
    public IGameManager gameManager;
    AudioManagerBase audioManager;
    Animator lalaAnimator;
    void Start()
    {

        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IGameManager>();
        foreach (var manager in ss)
            gameManager = manager;
        audioManager = FindObjectOfType<AudioManagerBase>();
        lalaAnimator = GetComponent<Animator>();
        //gameManager.wordSoundPlayed += Talk;
        audioManager.introEnded += Idle;
    }

    public void ShowThumbsUp()
    {
        lalaAnimator.SetTrigger(CorrectTrigger);
    }

    public void ShowWrongAnswer()
    {
        lalaAnimator.SetTrigger(WrongTrigger);
    }

    public void Talk()
    {
        lalaAnimator.ResetTrigger(IdleTrigger);
        lalaAnimator.SetTrigger(TalkingTrigger);
    }

    public void Idle()
    {
        
        if(SceneManager.GetActiveScene().name == "BlendingGame" || SceneManager.GetActiveScene().name == "ConsolidationGame")
        {
            lalaAnimator.Play("Idle");
        }else
            lalaAnimator.SetTrigger(IdleTrigger);
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && interactable)
        {
            interactable = false;
            gameManager.RepeatTutorial();
            //gameManager.TurnOffHandPointer();
        }
    }
}
