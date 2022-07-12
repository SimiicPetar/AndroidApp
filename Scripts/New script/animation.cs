using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    //public float FlyTime;
    //public Vector3 EndLocation;

    readonly string idleTrigger = "lalaMove";
    //readonly string HiTrigger = "Hi";
    public Animator LalaAnimator;
	
	

    //bool interactable = false;

    //Task LalaActionsTask;
    
    // Start is called before the first frame update
    /*private void Awake()
    {
        LalaAnimator = GetComponent<Animator>();
        interactable = false;
    }*/

    private void Start()
    {
		LalaAnimator.SetTrigger(idleTrigger);
		LalaAnimator.Play("lalaMove");
       // StartFlying();
    }

   /* void StartFlying()
    {
        transform.DOMove(EndLocation, FlyTime).SetEase(Ease.Linear).OnComplete(() => {
            LalaActionsTask = new Task(LalaActions());
        });
        
    }

    public void StopLalaActionsTask()
    {
        if(LalaActionsTask != null)
        {
            LalaActionsTask.Stop();
            Idle();
        }
    }

    IEnumerator LalaActions()
    {
        
        Hi();
        yield return new WaitForSeconds(LalaAnimator.GetCurrentAnimatorClipInfo(0).Length + 0.6f);
        AudioManagerWelcomeScreen.Instance.StartLalaSpeech();
        yield return new WaitForSeconds(AudioManagerWelcomeScreen.Instance.WelcomeLalaSpeech.length);
        Idle();
        interactable = true;
    }

    private void OnMouseOver()
    {
        if(LalaAnimator != null && Input.GetMouseButtonDown(0) && interactable)
        {
            StartCoroutine(LalaSpeech());
			
			
        }    
    }

    void Talk()
    {
        LalaAnimator.SetTrigger("Talk");
    }


    IEnumerator LalaSpeech()
    {
		Talk();
        interactable = false;
        Talk();
        AudioManagerWelcomeScreen.Instance.StartLalaSpeech();
        yield return new WaitForSeconds(AudioManagerWelcomeScreen.Instance.WelcomeLalaSpeech.length);
        Idle();
        interactable = true;
    }
    void Hi()
    {
        LalaAnimator.SetTrigger(HiTrigger);
    }

    public void Idle()
    {
        LalaAnimator.SetTrigger(idleTrigger);
    }*/

}
