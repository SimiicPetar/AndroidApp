using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LalaAvatarCreationController : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update\

    public delegate void NaratorIsTalking(bool isTalking);
    public static NaratorIsTalking naratorIsTalking;
    private SkeletonGraphic skeletonGraphic;
    bool interactable = false;

    [SpineAnimation]
    public string TalkingAnim;

    [SpineAnimation]
    public string IdleAnim;

    Spine.AnimationState state;

    Skeleton skeleton;

    AudioClip LastClipSpoken;
    void Start()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        skeleton = skeletonGraphic.Skeleton;

        SetMix();
    }
    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
    }
    void OnEnable()
    {
       // AudioManagerMap.OnStartedTalking += SetTalking;
        SetIdle();
    }

    void OnDestroy()
    {

    }

    void SetMix()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        skeleton = skeletonGraphic.Skeleton;

        AnimationStateData stateData = new AnimationStateData(skeletonGraphic.SkeletonData);
        skeletonGraphic.AnimationState.Data.SetMix(TalkingAnim, IdleAnim, 0.4f);
        skeletonGraphic.AnimationState.Data.SetMix(IdleAnim, TalkingAnim, 0.4f);
    }

    public void SetIdle()
    {
        SetMix();
        GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, IdleAnim, true);
    }

    public void RepeatInstruction()
    {
        AudioManagerAvatarCreation.Instance.PlaySaveAvatarclip();
    }

    IEnumerator TalkingCoroutine(float duration)
    {
        interactable = false;
        GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, TalkingAnim, true);
        yield return new WaitForSeconds(duration);
        SetIdle();
        interactable = true;
    }
}
