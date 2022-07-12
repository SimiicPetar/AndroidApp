using DG.Tweening;
using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerMap : Singleton<AudioManagerMap>
{
    // Start is called before the first frame update
    public delegate void BanUIWhenTalking(float duration);
    public static BanUIWhenTalking OnBanUIWhenTalking;

    public delegate void StartedTalking( AudioClip TalkedClip);
    public static StartedTalking OnStartedTalking;

    public SoundPackObject soundPack;

    public AudioClip MapSoundtrack;

    public AudioClip TouchTheLetter;

    public AudioClip PlayCapybara;

    public AudioClip PlayAnteater;

    public AudioClip PlayBlending;

    public AudioClip RewardAfterManatee;

    public AudioClip RewardAfterAnaconda;

    public AudioClip EndGameMapLalaSpeech;

    public AudioClip LalaAlbumPageSpeech;

    public AudioClip LalaRecommendsToPlayAgainSpeech;

    public AudioClip AvatarJumpingSoundEffect;

    public AudioClip PageFlippingSoundStickerAlbum;


    public List<AudioClip> WelcomeToTheHouse;

    AudioSourcePrefab bgMusicPrefab;

    AudioSourcePrefab LalaUnitNaratorCurrentSpeech;

    AudioSourcePrefab LalaFriendSpeech;

    Task TouchTheLetterTask;

    Task UnmuteTask;

    public AudioSourcePrefab BgMusicPrefab { get => bgMusicPrefab; set => bgMusicPrefab = value; }
    public bool LalaIsTalkingBeforeAlbum { get => _lalaIsTalkingBeforeAlbum; set => _lalaIsTalkingBeforeAlbum = value; }
    public bool LalaNaratorIsTalking { get => _lalaNaratorIsTalking; set => _lalaNaratorIsTalking = value; }

    bool _lalaIsTalkingBeforeAlbum = false;

    bool _lalaNaratorIsTalking = false;


    private void OnEnable()
    {
        GameManager.Instance.onMutedBGMusic += MuteBGMusicPrefab;
        UIMapManager.OnWindowClosed += UnmuteBGMusic;
    }

    private void UnmuteBGMusic(UIWindow window)
    {
        if (window == UIMapManager.Instance.UnitView)
        {
            bgMusicPrefab.ChangeMute(GameManager.Instance.MuteBGMusicBool);
            bgMusicPrefab.MuteGradually(GameManager.Instance.MuteBGMusicBool);
        }
           
    }

    private void OnDisable()
    {
        UIMapManager.OnWindowClosed -= StopTheSoundCoroutine;
        GameManager.Instance.onMutedBGMusic -= MuteBGMusicPrefab;
        UIMapManager.OnWindowClosed -= UnmuteBGMusic;
    }



    void Start()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
            bgMusicPrefab.ChangeMute(GameManager.Instance.MuteBGMusicBool);
        }
        

        UIMapManager.OnWindowClosed += StopTheSoundCoroutine;

    }

    void MuteBGMusicPrefab(bool muteState)
    {
     


        bgMusicPrefab.ChangeMute(muteState);
    }

    public void PlayBGMusic()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }else
            BgMusicPrefab.ChangeMute(false);
    }

    public void PlayAvatarJumpingSoundEffect()
    {
        PlaySound(AvatarJumpingSoundEffect);

        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
    }

   

    void StopTheSoundCoroutine(UIWindow win)
    {
        if (win == UIMapManager.Instance.UnitView && TouchTheLetterTask != null)
        {
            TouchTheLetterTask.Stop();
            TouchTheLetterTask = null;
        }
            
    }

    public void PlayBinSound()
    {
        PlaySound(soundPack.correctAnswerSound);
    }

    public void PlayLastClipSpoken(AudioClip clip)
    {   
        if(clip == null && !UnitProgressManager.Instance.CheckIfAllLettersAreFinished())// da ponovi "dodirni slovo"
        {
            PlayTouchTheLetter();
        } 
        else if (WelcomeToTheHouse.Contains(clip))
        {
            PlayWelcomeToTheHouse(GameManager.Instance.currentUnit.unitId);
        }
        else if(clip == TouchTheLetter || clip == PlayCapybara || clip == PlayAnteater || clip == PlayBlending)
        {
            if (BgMusicPrefab == null)
            {
                BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
                BgMusicPrefab.Initialize(MapSoundtrack, true);
            }
            
            BgMusicPrefab.MuteGradually(true);
            AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
            LalaUnitNaratorCurrentSpeech = pref;
            pref.Initialize(clip);
            OnStartedTalking?.Invoke(clip);
            UnmuteTask = new Task(WaitForUnmute(clip.length));
            StopLalaFriendSpeech();
        }

    }

    public void PlayLalaRecommendToPlayAgain()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(LalaRecommendsToPlayAgainSpeech);
        OnStartedTalking?.Invoke(LalaRecommendsToPlayAgainSpeech);
        UnmuteTask = new Task(WaitForUnmute(LalaRecommendsToPlayAgainSpeech.length));
    }

    public void PlayLalaAlbumPageSpeech()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(LalaAlbumPageSpeech, false, UIMapManager.Instance.StickerAlbum);
        UnmuteTask = new Task(WaitForUnmute(LalaAlbumPageSpeech.length));
    }

    public void PlayEndGameLalaSpeech()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(EndGameMapLalaSpeech);
        OnStartedTalking?.Invoke(EndGameMapLalaSpeech);
        // treba reci ovoj lali na mapi da pocne da prica ili stagod
        UnmuteTask = new Task(WaitForUnmute(EndGameMapLalaSpeech.length));
    }

    private bool HaveSameLetters(string a, string b)
    {
        return a.All(x => b.Contains(x)) && b.All(x => a.Contains(x));
    }

    public void PlayWelcomeToTheHouse(string unitID)
    {
        _lalaNaratorIsTalking = true;
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        //var audioClipToPlay = WelcomeToTheHouse.FirstOrDefault(s => HaveSameLetters(s.name, GameManager.Instance.currentUnit.unitId));
        var audioClipToPlay = WelcomeToTheHouse[UnitManager.Instance.AllUnitsList.IndexOf(GameManager.Instance.currentUnit)];
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        if (audioClipToPlay == null)
            audioClipToPlay = WelcomeToTheHouse[1];
        pref.Initialize(audioClipToPlay, false, UIMapManager.Instance.UnitView);
        LalaUnitNaratorCurrentSpeech = pref;
        OnStartedTalking?.Invoke(audioClipToPlay);
        TouchTheLetterTask = new Task(TouchTheLetterCoroutine(audioClipToPlay.length));
        UnmuteTask = new Task(WaitForUnmute(TouchTheLetter.length + audioClipToPlay.length));
        StopLalaFriendSpeech();
    }


    public void StopNaratorSpeech()
    {
        if(LalaUnitNaratorCurrentSpeech != null)
        {
            UIManager.Instance.lalaController.SetIdle();
            if(UIManager.Instance.lalaController.TalkingTask != null)
                UIManager.Instance.lalaController.TalkingTask.Stop();
            LalaUnitNaratorCurrentSpeech.ChangeMute(true);
            Destroy(LalaUnitNaratorCurrentSpeech.gameObject);
            
            if(UnmuteTask != null)
                UnmuteTask.Stop();
            if (TouchTheLetterTask != null)
                TouchTheLetterTask.Stop();
        }
    }

    public bool CheckIfLalaNaratorIsTalking()
    {
        return LalaUnitNaratorCurrentSpeech != null;
    }

    public bool CheckIfFriendIsTalking()
    {
        return LalaFriendSpeech != null;
    }

    public void StopLalaFriendSpeech()
    {
        if(LalaFriendSpeech != null)
        {
            UnmuteTask.Stop();
            UIManager.Instance.ActiveFriend.SetIdle();
            if (UIManager.Instance.ActiveFriend.TalkingTask != null)
                UIManager.Instance.ActiveFriend.TalkingTask.Stop();
            LalaFriendSpeech.ChangeMute(true);
            Destroy(LalaFriendSpeech.gameObject);
        }
    }

    IEnumerator TouchTheLetterCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayTouchTheLetter(true);
    }

    public void PlayAfterAnacondaReward()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(RewardAfterAnaconda, false, UIMapManager.Instance.StickerAlbum);
        OnStartedTalking?.Invoke(RewardAfterAnaconda);
        OnBanUIWhenTalking?.Invoke(RewardAfterAnaconda.length);
        StartCoroutine(LalaIsTalkingForUIBanCoroutine(RewardAfterAnaconda.length));
        UnmuteTask = new Task(WaitForUnmute(RewardAfterAnaconda.length));
    }

    IEnumerator LalaIsTalkingForUIBanCoroutine(float duration)
    {
        foreach(var obj in FindObjectsOfType<UIElementClickabilityControl>().ToList())
        {
            obj.GetComponent<Button>().enabled = false;
        }
        _lalaIsTalkingBeforeAlbum = true;
        yield return new WaitForSeconds(duration);
        _lalaIsTalkingBeforeAlbum = false;
        foreach (var obj in FindObjectsOfType<UIElementClickabilityControl>().ToList())
        {
            obj.GetComponent<Button>().enabled = true;
        }
    }

    public void PlayAfterManateeReward()
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(RewardAfterManatee, false, UIMapManager.Instance.StickerAlbum);
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }



    public void PlaySpeechSound(AudioClip speech)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        LalaFriendSpeech = pref;
        pref.Initialize(speech, false, UIMapManager.Instance.UnitView);
        StopNaratorSpeech();
       
    }

    public void PlayTouchTheLetter(bool isItIntro = false)
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        if (LalaUnitNaratorCurrentSpeech == null && !isItIntro)
        {
            AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
            LalaUnitNaratorCurrentSpeech = pref;
            pref.Initialize(TouchTheLetter, false, UIMapManager.Instance.UnitView);
            OnStartedTalking?.Invoke(TouchTheLetter);
        }
        else if(LalaUnitNaratorCurrentSpeech != null || isItIntro)
        {
            
            if(!isItIntro)
                BgMusicPrefab.MuteGradually(true);
            AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
            LalaUnitNaratorCurrentSpeech = pref;
            pref.Initialize(TouchTheLetter, false, UIMapManager.Instance.UnitView);
            OnStartedTalking?.Invoke(TouchTheLetter);
            

        }
    }

    IEnumerator WaitForUnmute(float soundLen)
    {

        yield return new WaitForSeconds(soundLen + 0.4f);
        BgMusicPrefab.MuteGradually(GameManager.Instance.MuteBGMusicBool);
        _lalaNaratorIsTalking = false;
    }

    public void PlayPlayCapybara()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        LalaUnitNaratorCurrentSpeech = pref;
        pref.Initialize(PlayCapybara, false, UIMapManager.Instance.UnitView);
        OnStartedTalking?.Invoke(PlayCapybara);
        StartCoroutine(WaitForUnmute(PlayCapybara.length));
    }

    public void PlayPlayAnteater()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        LalaUnitNaratorCurrentSpeech = pref;
        pref.Initialize(PlayAnteater, false, UIMapManager.Instance.UnitView);
        OnStartedTalking?.Invoke(PlayAnteater);
        UnmuteTask = new Task(WaitForUnmute(PlayAnteater.length));
    }

    public void PlayPlayBlending()
    {
        if (BgMusicPrefab == null)
        {
            BgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            BgMusicPrefab.Initialize(MapSoundtrack, true);
        }
        BgMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        LalaUnitNaratorCurrentSpeech = pref;
        pref.Initialize(PlayBlending, false, UIMapManager.Instance.UnitView);
        OnStartedTalking?.Invoke(PlayBlending);
        UnmuteTask = new Task(WaitForUnmute(PlayBlending.length));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
