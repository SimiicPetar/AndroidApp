using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationButton : MonoBehaviour
{
    // Start is called before the first frame update
     // readonly string idleTrigger = "lalaMove";
    //readonly string HiTrigger = "Hi";
    public Animator LalaAnimator;
	// public GameObject asd1;
	//public float broj;
	
	
	//const string IdleTrigger = "Idle";
	//const string TalkTrigger = "Talking";
	
	
    public void Start()
    {
		LalaAnimator.Play("New Animation");
		//StartCoroutine(StartCountdown());
		//LalaAnimator. SetBool("Talking", false); 
		//avatarAnimator.SetTrigger(IdleTrigger);
		//LalaAnimator.SetTrigger(idleTrigger);
		//LalaAnimator.Play("Talking");
		//LalaAnimator.Play("Idle");
       // StartFlying();Idle
	   
    }
	/*public void talk()
    {
        LalaAnimator.Play("Talking");
    }
	public void idle()
    {
       LalaAnimator.Play("Idle");
    }
	
	
	/*
	
	public void ShowThumbsUp()
    {
        lalaAnimator.SetTrigger(CorrectTrigger);
    }
	
	m_Animator.Play("FullGunFireSequence", -1, 0f);
	
	 m_Animator.gameObject.SetActive(false);
 m_Animator.gameObject.SetActive(true);
	*/
	/* float currCountdownValue;
 public IEnumerator StartCountdown(float countdownValue = 0)
 {
     currCountdownValue = countdownValue;
     while (currCountdownValue < 300)
     {
         //Debug.Log("Countdown: " + currCountdownValue);
		
         yield return new WaitForSeconds(1.0f);
         currCountdownValue++;
		  
     }
 }
 void Update()
    {
        
		
		if( currCountdownValue == 6 ){
			
			//LalaAnimator.Play("Talking", -1, 0f);
			//LalaAnimator.gameObject.SetActive(false);
			 LalaAnimator.Play("Idle");
			
		}
		
		
	}
    }*/
}
