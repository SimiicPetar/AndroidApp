using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayActive : MonoBehaviour
{
    public GameObject asd1;
	public float broj;
	
	
	
	/*public GameObject text2;
	public Text txt2;*/
	//private int yourNumberVariable;
	//public Text txt;
    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(StartCountdown());
    }

    
	 float currCountdownValue;
 private IEnumerator StartCountdown(float countdownValue = 0)
 {
     currCountdownValue = countdownValue;
     if (currCountdownValue < 300)
     {
         //Debug.Log("Countdown: " + currCountdownValue);
		
         yield return new WaitForSeconds(1.0f);
         currCountdownValue++;
		  
     }
	 
 }
 void Update()
    {
        
		
		if( currCountdownValue == broj ){
			
			if( asd1 != null){
				asd1.gameObject.SetActive(true);
			}
			
		}
		
		
	}
	
	
}
