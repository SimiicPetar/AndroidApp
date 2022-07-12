using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTheGameLala : MonoBehaviour
{
    Animator LalaAnimator;

    const string TalkTrigger = "Talk";
    const string IdleTrigger = "Idle";
    // Start is called before the first frame update
    void Start()
    {
        AudioManagerMap.OnStartedTalking += SetTalking;
        LalaAnimator = GetComponent<Animator>();
    }

    void SetIdle()
    {
        LalaAnimator.SetTrigger(IdleTrigger);
    }

    void SetTalking(AudioClip Clip)
    {
        if (!UIMapManager.Instance.UIOpen)
            StartCoroutine(TalkReset(Clip.length));
    }

    private void OnDisable()
    {
        AudioManagerMap.OnStartedTalking -= SetTalking;
    }


    IEnumerator TalkReset(float duration)
    {
        LalaAnimator.SetTrigger(TalkTrigger);
        yield return new WaitForSeconds(duration);
        LalaAnimator.SetTrigger(IdleTrigger);
    }

}
