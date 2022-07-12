using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationLala : MonoBehaviour
{
     // readonly string idleTrigger = "lalaMove";
    //readonly string HiTrigger = "Hi";
    public Animator LalaAnimator;
	//const string IdleTrigger = "Idle";
	//const string TalkTrigger = "Talking";
	
	
    public void Start()
    {
		LalaAnimator.Play("Talking");
		
		LalaAnimator.Play("Idle");
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
    }*/
	
	
	/*
	
	public void ShowThumbsUp()
    {
        lalaAnimator.SetTrigger(CorrectTrigger);
    }
	
	Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && anim.isPlaying)
        {
            anim.Stop();
        }
	*/
}

