
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManagerCapibara : AudioManagerBase
{

    



    static AudioManagerCapibara _instance = null;

    public static AudioManagerCapibara Instance { get { return _instance; } }

    KapibaraGameManager gameManager;

    int introSoundIndex = 0;

    public List<AudioClip> tutorialSounds;

    AudioSourcePrefab currentTutorialRepetitionPrefab = null;

    AudioSourcePrefab currentTutorialAudioPrefab = null;

    public Task TutorialRepeatTask;

    public Task TutorialAudioTask;

    public override void LoadSounds()
    {   
    }

    private void Start()
    {
        gameManager = KapibaraGameManager.Instance;
        LoadSounds();
        PlayTutorialSounds();
        StartCoroutine(BeginningOfTheGameBabyJumpIn(0f));

    }

    

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    IEnumerator BeginningOfTheGameBabyJumpIn(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.ActivateCapiGameobjects();
        yield return new WaitForSeconds(0.7f);
        gameManager.SetKapibara();
    }

    public override void PlayTutorialSounds()
    {
        TutorialAudioTask = new Task(CallbackHelperTutorial()); 
    }

    IEnumerator CallbackHelperTutorial()
    {
        yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        currentTutorialAudioPrefab = PlayTutorialSound(tutorialSounds[introSoundIndex]);
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        gameManager.progressBar.lala.interactable = true;
        gameManager.SetLala();
        HasListenedToTutorial = true;
        introEnded.Invoke();
    }

    private void OnDisable()
    {
    }

    public void RepeatTutorial()
    {
        introSoundIndex = 0;
        gameManager.progressBar.lala.Talk();
        PlayTutorialSoundsRepeated();
    }

    private void PlayTutorialSoundsRepeated()
    {
       TutorialRepeatTask = new Task(CallbackHelperTutorialRepeat());
    }

    void PlayTutorialRepetition(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        currentTutorialRepetitionPrefab = pref;
        pref.Initialize(clip);
    }

    public void StopTutorialRepetition()
    {
        if(TutorialRepeatTask != null && currentTutorialRepetitionPrefab != null)
        {
            TutorialRepeatTask.Stop();
            currentTutorialRepetitionPrefab.GetComponent<AudioSource>().mute = true;
            Destroy(currentTutorialRepetitionPrefab);
        }
        if(currentTutorialAudioPrefab != null)
        {
            TutorialAudioTask.Stop();
            currentTutorialAudioPrefab.GetComponent<AudioSource>().mute = true;
            currentTutorialAudioPrefab = null;
            Destroy(currentTutorialAudioPrefab);
            introEnded.Invoke();
            gameManager.SetLala();
        }
        currentTutorialRepetitionPrefab = null;
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.EnableKapibare();
        introEnded.Invoke();
    }

    IEnumerator CallbackHelperTutorialRepeat()
    {
        //ovde negde da uglavim da moze da se ponavlja hmmm
        TutorialRepetitionIsPlaying = true;
        gameManager.progressBar.lala.interactable = false;
        Debug.Log("Kliknuo si na lalu da zapocnes ponavljanje tutoriala");
        PlayTutorialRepetition(tutorialSounds[introSoundIndex]);       
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.EnableKapibare();
        introEnded.Invoke();
        TutorialRepeatTask.Stop();
    }

    private AudioSourcePrefab PlayTutorialSound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
        return pref;
    }

    public override void PlaySound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }

    public override void PlayQuestionResultSound(bool correct)
    {
        if (correct)
            PlaySound(soundPack.correctAnswerSound);
        else
            PlaySound(soundPack.wrongAnswerSound);


    }

    public override bool CheckIfMusicPlaying()
    {
        return true;
    }
}
