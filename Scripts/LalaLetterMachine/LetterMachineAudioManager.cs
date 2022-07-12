
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterMachineAudioManager : AudioManagerBase
{
    public delegate void TargetWordSoundPlaying();
    public TargetWordSoundPlaying onTargetWordSaid;

    static LetterMachineAudioManager _instance = null;
    public static LetterMachineAudioManager Instance { get { return _instance; } }


    
    public AudioClip LetterMachineIntroduction;

    public AudioClip spinningSound;

    LalaLetterMachineManager gameManager;

    Task callBackFunc;

    Task TutorialRepeatTask;

    Task TutorialSoundTask;

    AudioSourcePrefab currentTutorialRepetitionPrefab;

    AudioSourcePrefab currentTutorialSoundPrefab;

    public AudioSourcePrefab audioPref;


    public override void LoadSounds()//LoadSounds
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestionResultSound(bool correct)
    {
       
    }

    public AudioClip PlayBinSound()//correctAnswerSound
    {
        PlaySound(soundPack.correctAnswerSound);
        return soundPack.correctAnswerSound;
    }

    public override void PlaySound(AudioClip clip)//PlaySound
    {
        AudioSourcePrefab pref = Instantiate(audioPref, null);
        pref.Initialize(clip);
    }

    private AudioSourcePrefab PlayTutorialSound(AudioClip clip)//PlayTutorialSound
    {
        AudioSourcePrefab pref = Instantiate(audioPref, null);
        pref.Initialize(clip);
        return pref;
    }
   

    public void PlayTargetWordSound(string name)//  play sound
    {
        if (UnitManager.Instance != null)
        {
            var sound = gameManager.gameInfo.FindAudioClip(name);
            onTargetWordSaid.Invoke();
            PlaySound(sound);

        }
        else {
            var sound = gameManager.gameInfo.FindAudioClip(name);
            onTargetWordSaid.Invoke();
            PlaySound(sound);
        }
    }

    public void PlayLetterSound(char letter, string word)// play audio GetLetterAudio(letter, word)
    {
        PlaySound(GetLetterAudio(letter, word));
    }

    AudioClip GetTargetWordSound(char letter) // play sound Paths.WordSoundsPath + letter [0]
    {
        //ovde treba logika za pronalazenje zvuka konkretne reci 
        var sound = Resources.LoadAll<AudioClip>(Paths.WordSoundsPath + letter)[0];
        return sound;
    }

    AudioClip GetLetterAudio(char letter, string word)//GetLetterAudio
    {
        return Resources.Load<AudioClip>($"Sounds/LalaLetterMachine/LetterSounds/{word}{letter}");
    }

    AudioClip GetLetterSound(char letter, string specificWord = null) {// GetLetterSound logic 
        if(specificWord != null)
        {
            Debug.Log($"Sounds/specific letter sounds/{specificWord}{letter}");
            var sound = Resources.Load<AudioClip>($"Sounds/specific letter sounds/{specificWord}{letter}");
            if (sound == null)
            {
                var sound2 = Resources.Load<AudioClip>(Paths.LetterSoundsPath + letter);
                return sound2;
            }
            else return sound;
        }
        else
        {
            var sound = Resources.Load<AudioClip>(Paths.LetterSoundsPath + letter);
            if (sound == null)
            {
                return Resources.Load<AudioClip>(Paths.LetterSoundsPath + 'f');
            }
            else
                return sound;
        }

    }

    

    public override void PlayTutorialSounds() // play audio 
    {
        
        HasListenedToTutorial = true;
        //fallingLettersTask = new Task(InstantiateRandomFallingLetter());
        TutorialSoundTask = new Task(CallBackFunc(LetterMachineIntroduction.length));
    }

    IEnumerator CallBackFunc(float duration) //  WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk); WaitForSeconds(duration)
    {
        yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        currentTutorialSoundPrefab = PlayTutorialSound(LetterMachineIntroduction);
        yield return new WaitForSeconds(duration);
        gameManager.lalaNarator.interactable = true;
        //gameManager.StartFallingLetters();
        introEnded.Invoke();
        gameManager.StartHandPointerTask();
        TutorialSoundTask.Stop();
    }

    private void Awake()//Awake
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()//start game play audio
    {

        gameManager = LalaLetterMachineManager.Instance;
        PlayTutorialSounds();
    }

    public void RepeatTutorial()// play audio PlayTutorialSoundsRepeated
    {
     
        gameManager.lalaNarator.Talk();
        PlayTutorialSoundsRepeated();
    }

    void PlayTutorialRepetition(AudioClip clip)// play audio
    {
        AudioSourcePrefab pref = Instantiate(audioPref, null);
        currentTutorialRepetitionPrefab = pref;
        pref.Initialize(clip);
    }

    public void StopTutorialRepetition()//StopTutorialRepetition
    {
        if(TutorialRepeatTask != null && currentTutorialRepetitionPrefab != null)
        {
            TutorialRepeatTask.Stop();
            currentTutorialRepetitionPrefab.GetComponent<AudioSource>().mute = true;
            Destroy(currentTutorialRepetitionPrefab);
            currentTutorialRepetitionPrefab = null;
        }
        if (currentTutorialSoundPrefab != null) // if currentTutorialSoundPrefab is not null
        {
            TutorialSoundTask.Stop();
            currentTutorialSoundPrefab.GetComponent<AudioSource>().mute = true;
            currentTutorialSoundPrefab = null;
            Destroy(currentTutorialSoundPrefab);
            gameManager.StartHandPointerTask();
            introEnded.Invoke();
        }

        TutorialRepetitionIsPlaying = false;
        gameManager.lalaNarator.interactable = true;
        gameManager.lalaNarator.Idle();
        gameManager.EnableClicking();
        introEnded.Invoke();
    }

    public void PlayTutorialSoundsRepeated()// PlaySoundCoroutine  LetterMachineIntroduction
    {
        TutorialRepeatTask = new Task(PlaySoundCoroutine(LetterMachineIntroduction));
    }

    IEnumerator PlaySoundCoroutine(AudioClip clip)// play audio LetterMachineIntroduction
    {
        TutorialRepetitionIsPlaying = true;
        PlayTutorialRepetition(LetterMachineIntroduction);
        yield return new WaitForSeconds(LetterMachineIntroduction.length);
        TutorialRepetitionIsPlaying = false;
        gameManager.EnableClicking();
        gameManager.lalaNarator.Idle();
        TutorialRepeatTask.Stop();
    }



    public override bool CheckIfMusicPlaying()// CheckIfMusicPlaying  true or false
    {
        throw new System.NotImplementedException();
    }
}
