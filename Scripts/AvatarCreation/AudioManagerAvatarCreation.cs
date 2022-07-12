using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerAvatarCreation : Singleton<AudioManagerAvatarCreation>
{
    public AudioClip AvatarCreationPart1;
    public AudioClip AvatarCreationChooseLetterType;
    public AudioClip AvatarCreationChooseANameClip;
    public AudioClip AvatarCreationSaveAvatarClip;
    public AudioClip BackgroundMusic;


    AudioSourcePrefab AvatarCreationPart1Object;
    AudioSourcePrefab AvatarCreationChooseLetterTypeObject;
    AudioSourcePrefab AvatarCreationChooseAName;
    AudioSourcePrefab AvatarCreationSaveAvatar;
    AudioSourcePrefab BGMusicAudioPrefab;

    private void Start()// start
    {
        BGMusicAudioPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
        BGMusicAudioPrefab.Initialize(BackgroundMusic, true);
    }

    public void PlayAvatarCustomizationPanelSound()// Destroy AvatarCreationChooseLetterTypeObject 
    {
        if (AvatarCreationChooseLetterTypeObject != null)
            Destroy(AvatarCreationChooseLetterTypeObject.gameObject);
        AvatarCreationPart1Object = Instantiate(GameManager.Instance.audioPrefab, null);
        AvatarCreationPart1Object.Initialize(AvatarCreationPart1, false, UIManagerAvatarCreation.Instance.AvatarCustomizationPanel.GetComponent<UIWindow>());
    } 

    public void PlayChooseLetterTypeSound()// Destroy AvatarCreationPart1Object
    {
        if (AvatarCreationPart1Object != null)
            Destroy(AvatarCreationPart1Object.gameObject);
        AvatarCreationChooseLetterTypeObject = Instantiate(GameManager.Instance.audioPrefab, null);
        //AvatarCreationChooseLetterTypeObject.Initialize(AvatarCreationChooseLetterType, false, UIManagerAvatarCreation.Instance.TypeOfFontSelectionPanel.GetComponent<UIWindow>());
    }

    public void PlayChooseANameClip()// Destroy AvatarCreationChooseAName
    {
        if (AvatarCreationChooseAName != null)
            Destroy(AvatarCreationChooseAName.gameObject);
        AvatarCreationChooseAName = Instantiate(GameManager.Instance.audioPrefab, null);
        AvatarCreationChooseAName.Initialize(AvatarCreationChooseANameClip, false, UIManagerAvatarCreation.Instance.InputAvatarNamePanel.GetComponent<UIWindow>());
    }

    public void PlaySaveAvatarclip()// Destroy AvatarCreationSaveAvatar
    {
        if (AvatarCreationSaveAvatar != null)
            Destroy(AvatarCreationSaveAvatar.gameObject);
        AudioManagerMap.OnStartedTalking?.Invoke(AvatarCreationSaveAvatarClip);
        AvatarCreationSaveAvatar = Instantiate(GameManager.Instance.audioPrefab, null);
        AvatarCreationSaveAvatar.Initialize(AvatarCreationSaveAvatarClip, false, UIManagerAvatarCreation.Instance.ConfirmAvatarOverlayCanvas.GetComponent<UIWindow>());
    }

}
