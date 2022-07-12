using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerWelcomeScreen1 : AudioManagerBase
{
    // Start is called before the first frame update
  

    public AudioClip WelcomeScreenSoundtrack;

     AudioClip WelcomeLalaSpeech;

    AudioSourcePrefab _bgMusicPrefab;

    AudioSourcePrefab _lalaTalkPrefab;

    Task SpeechTask;

    public override bool CheckIfMusicPlaying()
    {
        throw new System.NotImplementedException();
    }

    public override void LoadSounds()
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestionResultSound(bool correct)
    {
        throw new System.NotImplementedException();
    }

    public override void PlaySound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }

    public override void PlayTutorialSounds()
    {
        throw new System.NotImplementedException();
    }

    #region Singleton
    private static AudioManagerWelcomeScreen1 instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    public static AudioManagerWelcomeScreen1 Instance
    {
        get => instance;

    }
    #endregion
    void Start()
    {
        if(_bgMusicPrefab == null)
        {
            _bgMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
            _bgMusicPrefab.Initialize(WelcomeScreenSoundtrack, true);
            
        }
    }


    public void StartLalaSpeech()
    {
        SpeechTask = new Task(WaitForEndOfLalaSpeechToIncreaseBgVolume());
        _lalaTalkPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
        _lalaTalkPrefab.Initialize(WelcomeLalaSpeech);
    }

    public void StopLalaSpeech()
    {
        if ( _lalaTalkPrefab != null )
        {
            SpeechTask.Stop();
            _lalaTalkPrefab.GetComponent<AudioSource>().mute = true;
            _lalaTalkPrefab = null;
            Destroy(_lalaTalkPrefab);
            var audioSource = _bgMusicPrefab.GetComponent<AudioSource>();
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 1f, 0.8f);
        }
    }
	
    IEnumerator WaitForEndOfLalaSpeechToIncreaseBgVolume()
    {
        var audioSource = _bgMusicPrefab.GetComponent<AudioSource>();
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0.1f, 0.8f);
        //PlaySound(WelcomeLalaSpeech);
        yield return new WaitForSeconds(WelcomeLalaSpeech.length);
        DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 1f, 0.8f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
