using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnimationState = Spine.AnimationState;

public class UnitLalaController : MonoBehaviour
{
    // Start is called before the first frame update\

    public delegate void NaratorIsTalking(bool isTalking);
    public static NaratorIsTalking naratorIsTalking;
    private SkeletonGraphic skeletonGraphic;
    AudioClip LastClipSpoken;

    [SpineAnimation]
    public string TalkingAnim;

    [SpineAnimation]
    public string IdleAnim;

    AnimationState state;

    bool interactable = true;

    Skeleton skeleton;

    public Task TalkingTask;
    private void Awake()
    {
        LalaFriendClickable.OnLalaFriendTalking += SetInteractable;
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        skeleton = skeletonGraphic.Skeleton;
    }

    
    void Start()
    {

        UIMapManager.OnWindowClosed += StopTalking;
        if (UnitStatisticsBase.Instance.CheckIfFinishedGame())
            GetComponent<Button>().enabled = false;

        SetMix();
    }

    public void SetInteractable(float waitTime)
    {
        StartCoroutine(InteractabilityReset(waitTime));
    }

    IEnumerator InteractabilityReset(float waitTime)
    {
        interactable = false;
        yield return new WaitForSeconds(waitTime);
        interactable = true;
    }

    void OnEnable()
    {
        
        AudioManagerMap.OnStartedTalking += SetTalking;
        LalaFriendClickable.OnLalaFriendTalking += SetInteractable;

        if (UnitStatisticsBase.Instance.CheckIfUnitIsFinished(GameManager.Instance.currentUnit.unitId))
            GetComponent<Button>().enabled = false;
        else
            GetComponent<Button>().enabled = true;  
        SetIdle();
    }

    private void StopTalking(UIWindow window)
    {
        if(TalkingTask != null)
        {
            TalkingTask.Stop();
            SetIdle();
        }
    }

    private void OnDestroy()
    {
        UIMapManager.OnWindowClosed -= StopTalking;
    }
    void OnDisable()
    {
        AudioManagerMap.OnStartedTalking -= SetTalking;
        
        LalaFriendClickable.OnLalaFriendTalking -= SetInteractable;
        interactable = true;
    }

    void SetMix()
    {
        AnimationStateData stateData = new AnimationStateData(skeletonGraphic.SkeletonData);
        skeletonGraphic.AnimationState.Data.SetMix(TalkingAnim, IdleAnim, 0.8f);
        skeletonGraphic.AnimationState.Data.SetMix(IdleAnim, TalkingAnim, 0.8f);
    }

    public void SetIdle()
    {
        GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, IdleAnim, true);
    }

    public void RepeatLastClipSpoken()
    {
        if (!AudioManagerMap.Instance.CheckIfLalaNaratorIsTalking())
        {
            
            AudioManagerMap.Instance.PlayLastClipSpoken(LastClipSpoken);
        }
            
    }

    public void SetTalking(AudioClip Clip)
    {
        LastClipSpoken = Clip;
        TalkingTask = new Task(TalkingCoroutine(Clip.length));
    }

    IEnumerator TalkingCoroutine(float duration)
    {
        interactable = false;
        GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, TalkingAnim, true);
        yield return new WaitForSeconds(duration);
        SetIdle();
        interactable = true;
        UIManager.Instance.ActiveFriend.AllowClick(true);
    }
}
