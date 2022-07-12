
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterTracingAudioManager : AudioManagerBase
{
    private static LetterTracingAudioManager _instance = null;
    public static LetterTracingAudioManager Instance { get { return _instance; } }

    public delegate void introPart1Ended();
    public introPart1Ended onIntroPart1Ended;

    public List<AudioClip> sameForAllLetters;
    public List<AudioClip> letterpartSounds;

    LetterTracingManager gameManager;

    int introSoundIndex = 0;



    UnitManager unitManager;

    Task callBackFunc;

    Task tutorialRepeatTask;

    Task TutPart1;

    public ProgressBarLalaVisualBehaviour lala;

    public override void LoadSounds()// LoadSounds
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestionResultSound(bool correct)// PlayQuestionResultSound
    {
        if (correct)
            PlaySound(soundPack.correctAnswerSound);
        else
            PlaySound(soundPack.wrongAnswerSound);
    }

    public override void PlaySound(AudioClip clip)// play sound clip
    {
        //audioManager.PlayOneShot(clip);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }

    public override void PlayTutorialSounds() 
    {
       
        //callBackFunc = new Task(CallBackFunc(soundToPlay.length));
    }

    public void PlayTutorialSoundsPart1() // task TutorialSoundPart1 play
    {
        TutPart1 = new Task(TutorialSoundPart1());
    }


    IEnumerator TutorialSoundPart1(bool repeat = false)// AllowDrawing . lala  Idle and  interactable. WaitForSeconds NaratorWaitTimeWhenStartToTalk
    {
        if (!repeat)
            yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        AudioClip soundToPlay = sameForAllLetters[0];
        introSoundIndex++;
        PlaySound(soundToPlay);
        yield return new WaitForSeconds(soundToPlay.length);
       // gameManager.ShowFingerPrompt();
        //onIntroPart1Ended.Invoke();
        gameManager.AllowDrawing();
        lala.Idle();
        lala.interactable = true;
        TutPart1.Stop();
    }

 


    IEnumerator CallBackFunc(float duration)//CallBackFunc
    {
        yield return new WaitForSeconds(duration);
        introEnded.Invoke();
        callBackFunc.Stop();
    }


    private void Awake()// Awake
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()// start play sound
    {
        gameManager = LetterTracingManager.Instance;
        unitManager = UnitManager.Instance;
        
        PlayTutorialSoundsPart1();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckIfMusicPlaying() //CheckIfMusicPlaying
    {
        throw new System.NotImplementedException();
    }

    public void RepeatTutorial() //  RepeatTutorial  task CallbackHelperTutorialRepeat
    {
        lala.Talk();
        lala.interactable = false;
        tutorialRepeatTask = new Task(CallbackHelperTutorialRepeat());
    }

    IEnumerator CallbackHelperTutorialRepeat()// click on lala tutorialRepeat
    {
        //ovde negde da uglavim da moze da se ponavlja hmmm
        TutorialRepetitionIsPlaying = true;
        lala.interactable = false;
        Debug.Log("Kliknuo si na lalu da zapocnes ponavljanje tutoriala");
        PlayTutorialRepetition(sameForAllLetters[0]);
        yield return new WaitForSeconds(sameForAllLetters[0].length);
        TutorialRepetitionIsPlaying = false;
        lala.interactable = true;
        gameManager.AllowDrawing();
        introEnded.Invoke();
        tutorialRepeatTask.Stop();
    }

    void PlayTutorialRepetition(AudioClip clip) // play audio
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
       // currentTutorialRepetitionPrefab = pref;
        pref.Initialize(clip);
    }
}
