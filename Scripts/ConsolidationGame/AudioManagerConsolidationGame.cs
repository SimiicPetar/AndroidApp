
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerConsolidationGame : AudioManagerBase
{
    static AudioManagerConsolidationGame _instance = null;
    public static AudioManagerConsolidationGame Instance { get { return _instance; } }

    public AudioClip TutorialSound;

    public AudioClip RepeatingPartOfTutorial;

    public LetterSoundPairs letterSoundPairs;

    ConsolidationGameManagerNew gameManager;


    public delegate void StartedLetterSound(bool start);
    public StartedLetterSound letterSoundStarted;

    Task TutorialRepeatTask;

    Task TutorialAudioTask;

    AudioSourcePrefab currentTutorialRepetitionPrefab;

    AudioSourcePrefab currentTutorialAudioPrefab;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = ConsolidationGameManagerNew.Instance;
        PlayTutorialSounds();
        StartCoroutine(LalaStartBan());
    }

    IEnumerator LalaStartBan()
    {
        gameManager.progressBar.lala.interactable = false;
        yield return new WaitForSeconds(TutorialSound.length + 1f);
        gameManager.progressBar.lala.interactable = true;
    }

    public void PlayLetterSound(char letter, bool reset = false)
    {
        StartCoroutine(PlayLetterSoundHelper(letter, reset));
    }

    IEnumerator PlayLetterSoundHelper(char letter, bool reset = false)
    {
        letterSoundStarted(true);
        LetterSoundPair pair = letterSoundPairs.GetLetterSoundPair(letter);
        PlaySound(pair.letterSound);
        yield return new WaitForSeconds(pair.letterSound.length);
        letterSoundStarted(false);
        gameManager.progressBar.lala.GetComponent<Collider>().enabled = true;
       // gameManager.ShowPosibilities(reset);
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
        if (correct)
            PlaySound(soundPack.correctAnswerSound);
        else
            PlaySound(soundPack.wrongAnswerSound);
    }

    IEnumerator LalaClickBan(AudioClip clip)
    {
        letterSoundStarted(true);
        yield return new WaitForSeconds(clip.length + 0.9f);
        letterSoundStarted(false);
    }

    public override void PlaySound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
        LalaClickBan(clip);
    }
    
    private AudioSourcePrefab PlayTutorialSound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
        return pref;
    }

    public override void PlayTutorialSounds()
    {
        TutorialAudioTask = new Task(CallbackTutorialHelper());
    }

    IEnumerator CallbackTutorialHelper()
    {
        yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        currentTutorialAudioPrefab = PlayTutorialSound(TutorialSound);
        yield return new WaitForSeconds(TutorialSound.length);
        HasListenedToTutorial = true;
        introEnded.Invoke();
        gameManager.progressBar.lala.interactable = true;
        gameManager.ShowPosibilities();
    }

    public void RepeatTutorial()
    {
        gameManager.progressBar.lala.Talk();
        PlayTutorialSoundsRepeated();
    }

    private void PlayTutorialSoundsRepeated()
    {
        TutorialRepeatTask = new Task(CallbackHelperTutorialRepeat());
    }

    public void StopTutorialRepetition()
    {
        if(TutorialRepeatTask != null && currentTutorialRepetitionPrefab != null)
        {
            TutorialRepeatTask.Stop();
            currentTutorialRepetitionPrefab.GetComponent<AudioSource>().mute = true;
            Destroy(currentTutorialRepetitionPrefab);
            currentTutorialRepetitionPrefab = null;
        }
        if(currentTutorialAudioPrefab != null)
        {
            TutorialAudioTask.Stop();
            currentTutorialAudioPrefab.GetComponent<AudioSource>().mute = true;
            currentTutorialAudioPrefab = null;
            Destroy(currentTutorialAudioPrefab);
            introEnded.Invoke();
            gameManager.ShowPosibilities();
        }
      
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.progressBar.lala.Idle();
        gameManager.AllowClickingRocks(true);
        introEnded.Invoke();
    }

    void PlayTutorialRepetition(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        currentTutorialRepetitionPrefab = pref;
        pref.Initialize(clip);
    }

    IEnumerator CallbackHelperTutorialRepeat()
    {
        TutorialRepetitionIsPlaying = true;
        PlayTutorialRepetition(RepeatingPartOfTutorial);
        yield return new WaitForSeconds(RepeatingPartOfTutorial.length);
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.progressBar.lala.Idle();
        gameManager.AllowClickingRocks(true);
        TutorialRepeatTask.Stop();
    }

}
