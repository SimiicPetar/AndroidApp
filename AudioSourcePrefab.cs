using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePrefab : MonoBehaviour
{
    UIWindow _uiEventInvokingWindow;
    AudioSource audioSource;
    // Start is called before the first frame update
    public void Initialize(AudioClip clipToPlay, bool loop = false, UIWindow uiThatShutsMeDown = null)
    {
        _uiEventInvokingWindow = uiThatShutsMeDown;
        if(_uiEventInvokingWindow != null)
        {
            UIMapManager.OnWindowClosed += DestroyAudioPrefab;
        }
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = loop;

        if (!loop)
        {
            audioSource.PlayOneShot(clipToPlay);
            Invoke("DestroyObject", clipToPlay.length + 0.1f);
        }
        else
        {
            audioSource.loop = loop;
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
            
    }

    private void OnDisable()
    {
        UIMapManager.OnWindowClosed -= DestroyAudioPrefab;
    }

    void DestroyAudioPrefab(UIWindow win)
    {
        if (win == _uiEventInvokingWindow)
            Destroy(gameObject);
    }

    public void MuteGradually(bool muteState)
    {
        if (GameManager.Instance.MuteBGMusicBool)
        {
            audioSource.mute = true;
            return;
        }
            
        if (muteState)
        {
            
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0f, 1f).OnComplete(() => audioSource.mute = muteState);
        }
        else
        {
            audioSource.mute = muteState;
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 1f, 1f);
        }
            


    }

    public void ChangeMute(bool mute)
    {
        if (mute)
            audioSource.mute = mute;
        else
            audioSource.mute = GameManager.Instance.MuteBGMusicBool;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
	
}
