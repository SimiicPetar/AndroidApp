using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerPlayerSelection : AudioManagerBase
{
    static AudioManagerPlayerSelection _instance;
    public static AudioManagerPlayerSelection Instance { get { return _instance; } }

    public AudioClip BackgroundMusic;

    AudioSourcePrefab BGMusicPrefab;
    private void Awake()
    {
        _instance = this;
    }
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
        throw new System.NotImplementedException();
    }

    public override void PlayTutorialSounds()
    {
        throw new System.NotImplementedException();
    }

    public AudioClip PlayerSelectionSoundPart1;
    public AudioClip PlayerSelectionSoundPart2;

    public ProgressBarLalaVisualBehaviour lalaNarator;
    void Start()
    {
        BGMusicPrefab = Instantiate(GameManager.Instance.audioPrefab, null);
        BGMusicPrefab.Initialize(BackgroundMusic, true);
        BGMusicPrefab.ChangeMute(GameManager.Instance.MuteBGMusicBool);
        StartCoroutine(LalaControllCoroutine(false));
    }

    IEnumerator LalaControllCoroutine(bool repeat)
    {
        
        if(!repeat)
            yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        BGMusicPrefab.MuteGradually(true);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(PlayerSelectionSoundPart1);
        yield return new WaitForSeconds(PlayerSelectionSoundPart1.length);
        AudioSourcePrefab pref2 = Instantiate(GameManager.Instance.audioPrefab, null);
        pref2.Initialize(PlayerSelectionSoundPart2);
        yield return new WaitForSeconds(PlayerSelectionSoundPart2.length);
        lalaNarator.Idle();
        ChooseAvatarLalaController.interactable = true;
        BGMusicPrefab.MuteGradually(false);
    }


    public void RepeatLalaInstruction() {
        lalaNarator.Talk();
        StartCoroutine(LalaControllCoroutine(true));
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}

// Start is called before the first frame update


