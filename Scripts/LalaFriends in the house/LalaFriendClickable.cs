using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LalaFriendClickable : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void LalaFriendTalking(float WaitTime);
    public static LalaFriendTalking OnLalaFriendTalking;
    public string unitID;
    public AudioClip speechSound;
    bool clickable = true;
    Button button;


    private SkeletonGraphic skeletonGraphic;

    [SpineAnimation]
    public string TalkingAnim;

    [SpineAnimation]
    public string IdleAnim;

    Spine.AnimationState state;

    Skeleton skeleton;


    public Task TalkingTask;

    Task TalkBanTask;

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        skeleton = skeletonGraphic.Skeleton;
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        UIMapManager.OnWindowOpened += SetIdleWhenReopeningWindow;
        AudioManagerMap.OnBanUIWhenTalking += SetUninteractable;

    }

    private void SetUninteractable(float duration)
    {
        TalkBanTask = new Task(BanWhenLalaTalksAfterAnacondaCoroutine(duration));
    }

    IEnumerator BanWhenLalaTalksAfterAnacondaCoroutine(float duration)
    {
        button.enabled = false;
        yield return new WaitForSeconds(duration);
        button.enabled = true;
    }

    private void OnDisable()
    {
        UIMapManager.OnWindowOpened -= SetIdleWhenReopeningWindow;
        AudioManagerMap.OnBanUIWhenTalking -= SetUninteractable;
    }

    void Start()
    {
        SetMix();
    }

    void SetMix()
    {
        AnimationStateData stateData = new AnimationStateData(skeletonGraphic.SkeletonData);
        skeletonGraphic.AnimationState.Data.SetMix(TalkingAnim, IdleAnim, 0.4f);
        skeletonGraphic.AnimationState.Data.SetMix(IdleAnim, TalkingAnim, 0.4f);
    }

    void SetIdleWhenReopeningWindow(UIWindow win)
    {
        if (win == UIMapManager.Instance.UnitView)
            SetIdle();
    }

    void StopTalking()
    {
        SetIdle();
    }

    public void SetIdle()
    {
        skeletonGraphic.AnimationState.SetAnimation(0, IdleAnim, true);
    }

    public void SetTalking(float duration)
    {
        TalkingTask = new Task(TalkingCoroutine(duration));
    }

    IEnumerator TalkingCoroutine(float duration)
    {
        UIManager.Instance.lalaController.SetInteractable(duration);
        AudioManagerMap.Instance.BgMusicPrefab.MuteGradually(true);
        skeletonGraphic.AnimationState.SetAnimation(0, TalkingAnim, true);
        yield return new WaitForSeconds(duration);
        AudioManagerMap.Instance.BgMusicPrefab.MuteGradually(false);
        SetIdle();
        clickable = true;
    }

    public void AllowClick (bool naratorIsTalking)
    {
        clickable = naratorIsTalking;
    }

    public void ClickToSpeak()
    {
        if(!AudioManagerMap.Instance.CheckIfFriendIsTalking())
        {
            clickable = false;
            AudioManagerMap.Instance.PlaySpeechSound(speechSound);
            TalkingTask = new Task(TalkingCoroutine(speechSound.length));
        }
            
    }


}
