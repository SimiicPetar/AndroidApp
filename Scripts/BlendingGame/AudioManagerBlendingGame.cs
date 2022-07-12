
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerBlendingGame : AudioManagerBase
{
    static AudioManagerBlendingGame _instance = null;
    public static AudioManagerBlendingGame Instance { get { return _instance; } }
    BlendingGameManager gameManager;

    public AudioClip CubeFitSound;

    public AudioClip tutorialSoundLala;

    AudioSourcePrefab currentTutorialRepetitionPrefab;

    AudioSourcePrefab currentTutorialAudioPrefab;

    private Task TutorialRepeatTask;

    private Task TutorialAudioTask;

    bool DontStartTutorial = false;

    public override void LoadSounds()
    {
        throw new System.NotImplementedException();
    }

    public override void PlayQuestionResultSound(bool correct)
    {
        throw new System.NotImplementedException();
    }

    public AudioSourcePrefab PlayTutorialSound(AudioClip clip)
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

    public void PlayFitSound()
    {
        PlaySound(CubeFitSound);
    }

    public override void PlayTutorialSounds()
    {
        if(!DontStartTutorial)
            TutorialAudioTask = new Task(IntroCoroutine());
    }

    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        if (!DontStartTutorial)
        {
            currentTutorialAudioPrefab = PlayTutorialSound(tutorialSoundLala);

            yield return new WaitForSeconds(tutorialSoundLala.length);
            HasListenedToTutorial = true;
            gameManager.lala.interactable = true;
            gameManager.BlendingGameInit();
            gameManager.EnableClicking();
            introEnded.Invoke();
        }
        
    }

    

    public void PlaySecondPartOfLetter(AudioClip clip)
    {

        PlaySound(clip);
    }

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

        gameManager = BlendingGameManager.Instance;
        PlayTutorialSounds();

    }

    public void RepeatTutorial()
    {
      
        gameManager.lala.Talk();
        PlayTutorialSoundsRepeated();
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
            gameManager.EnableClicking();
            gameManager.BlendingGameInit();
        }else
        {
            DontStartTutorial = true;
            introEnded.Invoke();
            gameManager.EnableClicking();
            gameManager.BlendingGameInit();
        }
        
        TutorialRepetitionIsPlaying = false;
        gameManager.lala.interactable = true;
        gameManager.lala.Idle();
        gameManager.EnableClicking();
        introEnded.Invoke();
    }

    public void PlayTutorialSoundsRepeated()
    {
        TutorialRepeatTask = new Task(CallbackHelperTutorialRepeat(tutorialSoundLala));
    }

    void PlayTutorialRepetition(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        currentTutorialRepetitionPrefab = pref;
        pref.Initialize(clip);
    }

    IEnumerator CallbackHelperTutorialRepeat(AudioClip tutorialRepeat)
    {
        TutorialRepetitionIsPlaying = true;
        PlayTutorialRepetition(tutorialRepeat);
        yield return new WaitForSeconds(tutorialRepeat.length);
        TutorialRepetitionIsPlaying = false;
        gameManager.lala.Idle();
    }
    IEnumerator PlaySoundCoroutine(AudioClip clip)
    {
        PlaySound(tutorialSoundLala);
        yield return new WaitForSeconds(tutorialSoundLala.length);
        gameManager.EnableClicking();
        gameManager.lala.Idle();
    }
    public override bool CheckIfMusicPlaying()
    {
        throw new System.NotImplementedException();
    }
}
