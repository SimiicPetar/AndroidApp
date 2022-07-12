using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ManateeAudioManager : AudioManagerBase {


    static ManateeAudioManager _instance = null;
    public static ManateeAudioManager Instance { get { return _instance; } }

    public delegate void WordSoundsOver(bool over);
    public WordSoundsOver wordSoundsOverEvent;

    public AudioClip defaultIntroAudio;
    Action fcallback;

    ManateeGameManager gameManager;
    private int maxIntroSoundIndex = 0;
    readonly string ManateeGameSoundsPath = "Sounds/Manatee Game Sounds";
    int introSoundIndex = 0;
    int WordIndex = 0;
    float delay = 2.333f;
    List<AudioClip> tutorialSounds;
    public AudioClip DefaultManateeIntro;

    private AudioSourcePrefab currentTutorialRepetitionPrefab;

    private AudioSourcePrefab currentTutorialAudioPrefab;

    private Task TutorialRepeatTask;

    private Task TutorialAudioTask;

    private Coroutine PlayWordSoundsTask;

    // Start is called before the first frame update
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        tutorialSounds = new List<AudioClip>();
        gameManager = ManateeGameManager.Instance;
        LoadSounds();
        
    }

    public void PlayWordSounds(List<AudioClip> clips, bool introduction = false)
    {
        PlayWordSoundsTask = StartCoroutine(PlayWordSoundsCoroutine(clips, introduction));
    

    }

    // Update is called once per frame
    IEnumerator PlayWordSoundsCoroutine(List<AudioClip> clips, bool introduction = false)
    {
        gameManager.AllowClicking(true);
        if (WordIndex < clips.Count) {
            gameManager.manateeList[WordIndex].ShowWordArrow(clips[WordIndex]);
        }          
        else
        {
            WordIndex = 0;
            wordSoundsOverEvent(true);
            gameManager.AllowClicking(true);
            gameManager.progressBar.lala.interactable = true;
            gameManager.progressBar.lala.GetComponent<Collider>().enabled = true;
            yield break;
        }

        if (!gameManager.manateeList[WordIndex].manateeVisual.IsManateeUnderWater())
        {
            StartCoroutine(DelayBetweenWords(0.8f, clips));
        }
        else
        {
            WordIndex++;
            StartCoroutine(PlayWordSoundsCoroutine(clips));
        }
        yield return new WaitForSeconds(0f);
    }

    public void StopWordSoundTask()
    {
        if (PlayWordSoundsTask != null)
        {
            StopAllCoroutines();
            WordIndex = 0;
            wordSoundsOverEvent(true);
            gameManager.AllowClicking(true);

        }
            
    }

    IEnumerator DelayBetweenWords(float duration, List<AudioClip> clips)
    {
        PlaySound(clips[WordIndex]);
        yield return new WaitForSeconds(clips[WordIndex].length + 0.8f);
        WordIndex++;
        PlayWordSounds(clips);
    }

    public override void LoadSounds()
    {
        string letter;
        if (UnitManager.Instance != null)
            letter = UnitManager.Instance.GetCurrentLetter().ToString().ToUpper();
        else
            letter = "a";
        foreach (var sound in Resources.LoadAll<AudioClip>(ManateeGameSoundsPath + $"/Letter{letter}/"))
        {
            tutorialSounds.Add(sound);
            Debug.Log("Loadovao sam " + sound.name);
           
        }
        if (tutorialSounds.Count == 0)
            tutorialSounds.Add(defaultIntroAudio);

        StartCoroutine(WaveDelay());
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

    public override void PlayTutorialSounds()
    {
        TutorialAudioTask = new Task(CallbackHelperTutorial());
    }


    IEnumerator CallbackHelperTutorial()
    {
        HasListenedToTutorial = true;
        currentTutorialAudioPrefab = PlayTutorialSound(tutorialSounds[introSoundIndex]);
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        gameManager.progressBar.lala.interactable = true;
        gameManager.ManateeInit();
        //wordSoundsOverEvent(true);
        WoodenSignManatee.Instance.SetInteractable(true);
        gameManager.narator.interactable = true;

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
    //asdsa
 

    public void PlayTutorialSoundsRepeated()
    {
        TutorialRepeatTask = new Task(CallbackHelperTutorialRepeat());
    }


    public void StopTutorialRepetition()
    {
        if (TutorialRepeatTask != null && currentTutorialRepetitionPrefab != null)
        {
            TutorialRepeatTask.Stop();
            currentTutorialRepetitionPrefab.GetComponent<AudioSource>().mute = true;
            Destroy(currentTutorialRepetitionPrefab);
            currentTutorialRepetitionPrefab = null;
        }if(currentTutorialAudioPrefab != null)
        {
            TutorialAudioTask.Stop();
            currentTutorialAudioPrefab.GetComponent<AudioSource>().mute = true;
            currentTutorialAudioPrefab = null;
            Destroy(currentTutorialAudioPrefab);
            introEnded.Invoke();
            gameManager.ManateeInit();
        }
        
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.progressBar.lala.Idle();
        gameManager.EnableClicking();
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
        PlayTutorialRepetition(tutorialSounds[introSoundIndex]);
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.progressBar.lala.Idle();
        gameManager.EnableClicking();
        TutorialRepeatTask.Stop();
    }
    
    void OnEnable()
    {
        
    }

    public void PlayTargetWord(AudioClip tSound)
    {

        StartCoroutine(PlayTargetWordSound(tSound));
    }

    IEnumerator PlayTargetWordSound(AudioClip targetWordSound)
    {
        yield return new WaitForSeconds(soundPack.correctAnswerSound.length / 2);
        PlaySound(targetWordSound);
    }

    public override void PlayQuestionResultSound(bool correct)
    {
        if (correct) { PlaySound(soundPack.correctAnswerSound); }
            
        else
            PlaySound(soundPack.wrongAnswerSound);
    }
   

    IEnumerator WaveDelay()
    {
        yield return new WaitForSeconds(0f);
        PlayTutorialSounds();
    }

    public override bool CheckIfMusicPlaying()
    {
        throw new NotImplementedException();
    }
}
