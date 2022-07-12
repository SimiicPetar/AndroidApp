using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileBehaviour : MonoBehaviour
{
    Animator crocodileAnimator;

    readonly string OpenMouthTrigger = "OpenMouth";

    private void Awake()
    {
        StartCoroutine(AnimatorDisableRandomDelay());
    }

    IEnumerator AnimatorDisableRandomDelay()
    {
        GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
        GetComponent<Animator>().enabled = true;
    }

    private void Start()
    {
        crocodileAnimator = GetComponent<Animator>();
    }

    public void OpenMouth()
    {
        crocodileAnimator.SetTrigger(OpenMouthTrigger);
    }
}
