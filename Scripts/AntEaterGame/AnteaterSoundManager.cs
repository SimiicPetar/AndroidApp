
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnteaterSoundManager : AudioManagerBase
{
    // Start is called before the first frame update

    public List<AudioClip> tutorialSounds;


    AnteaterGameManager gameManager;

    static AnteaterSoundManager _instance = null;

    public static AnteaterSoundManager Instance { get { return _instance; } }

    int introSoundIndex = 0;

    private AudioSourcePrefab currentTutorialAudioPrefab;

    private AudioSourcePrefab currentTutorialRepetitionPrefab;

    public Task TutorialRepeatTask;

    private Task TutorialAudioTask;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {

        gameManager = AnteaterGameManager.Instance;
        PlayTutorialSounds();
        StartCoroutine(BeginningOfTheGameAntJumpIn(1f));
    }

    public override void LoadSounds()
    {
        foreach(var sound in tutorialSounds)
        {
        }
    }

    IEnumerator BeginningOfTheGameAntJumpIn(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.ActivateAntGameObjects();
        gameManager.SetMravi();
        yield return new WaitForSeconds(0.3f);
       
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
        yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        currentTutorialAudioPrefab =  PlayTutorialSound(tutorialSounds[introSoundIndex]);
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        HasListenedToTutorial = true;
        gameManager.progressBar.lala.interactable = true;
        gameManager.SetLala();
        introEnded.Invoke();
    }

    public void RepeatTutorial()
    {
        introSoundIndex = 0;
        gameManager.progressBar.lala.Talk();
        PlayTutorialSoundsRepeated();
    }

    public void PlayTutorialSoundsRepeated()
    {
        

        TutorialRepeatTask = new Task( CallbackHelperTutorialRepeat());
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
        if (currentTutorialAudioPrefab != null)
        {
            TutorialAudioTask.Stop();
            currentTutorialAudioPrefab.GetComponent<AudioSource>().mute = true;
            currentTutorialAudioPrefab = null;
            Destroy(currentTutorialAudioPrefab);
            introEnded.Invoke();
            gameManager.SetLala();
        }

        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.DisableMravi(true);
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
        PlayTutorialRepetition(tutorialSounds[introSoundIndex]);
        TutorialRepetitionIsPlaying = true;
        yield return new WaitForSeconds(tutorialSounds[introSoundIndex].length);
        TutorialRepetitionIsPlaying = false;
        gameManager.progressBar.lala.interactable = true;
        gameManager.progressBar.lala.Idle();
        gameManager.EnableClicking();
        TutorialRepeatTask.Stop();
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
        throw new System.NotImplementedException();
    }
}
