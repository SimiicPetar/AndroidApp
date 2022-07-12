using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LalaControl : MonoBehaviour
{
	public Animator LalaAnimator;
	bool interactable = false;
	 readonly string idleTrigger = "Idle";
    readonly string HiTrigger = "Hi";
    // Start is called before the first frame update
    void Start()
    {
        
    }
	private void Awake()
    {
        LalaAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	 private void OnMouseOver()
    {
        if(LalaAnimator != null && Input.GetMouseButtonDown(0) && interactable)
        {
            StartCoroutine(LalaSpeech());
        }    
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
	void Talk()
    {
        LalaAnimator.SetTrigger("Talk");
    }
	 void Hi()
    {
        LalaAnimator.SetTrigger(HiTrigger);
    }

    public void Idle()
    {
        LalaAnimator.SetTrigger(idleTrigger);
    }
}
