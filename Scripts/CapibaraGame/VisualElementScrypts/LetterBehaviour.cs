using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Animator LetterAnimator;

    readonly string FadeTrigger = "Fade";
    readonly string IdleTrigger = "Idle";

    private void Awake()
    {
        LetterAnimator = GetComponent<Animator>();
    }

    IEnumerator FadeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LetterAnimator.SetTrigger(FadeTrigger);
    }

    IEnumerator IdleDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        LetterAnimator.SetTrigger(IdleTrigger);
    }


    public void Fade()
    {
        Debug.Log("fejdovanje slova");
        LetterAnimator.ResetTrigger(IdleTrigger);
        if (!SceneManager.GetActiveScene().name.Equals("ManateeGame"))
            StartCoroutine(FadeDelay(0.6f));
        else
            StartCoroutine(FadeDelay(0f));
    }

    public void ResetToIdle()
    {
        Debug.Log("Vracanje slova");
        LetterAnimator.ResetTrigger(FadeTrigger);
        if (SceneManager.GetActiveScene().name.Equals("AnteaterGame"))
            StartCoroutine(IdleDelay(AnteaterGameManager.Instance.LETTERSHOWUPDELAY));
        else if (SceneManager.GetActiveScene().name.Equals("ManateeGame"))
        {
            StartCoroutine(IdleDelay(ManateeGameManager.Instance.LETTERSHOWUPDELAY));
        }
        else
        {
            LetterAnimator.SetTrigger(IdleTrigger);
        }
            
    }

}
