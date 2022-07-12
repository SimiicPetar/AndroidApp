
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetterSoundAudioManager : AudioManagerBase
{



    LetterSoundGameManager gameManager;

    static LetterSoundAudioManager _instance = null;

    public static LetterSoundAudioManager Instance { get{ return _instance; } }

    public List<AudioClip> tutorialSounds;

    List<AudioClip> introSounds;

    public ProgressBarLalaVisualBehaviour lala;

    Task TargetWordIllustrationShowUp;

    Task TargetWordLetterShowUp;

    Task callBackFunc;

    Task playSoundDelay;

    private void Awake() //awake
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

    }

    private void Start()// start load intro sound
    {
        gameManager = LetterSoundGameManager.Instance;

        if(UnitManager.Instance != null)
        {
    
            introSounds = Resources.Load<LetterSoundIntros>($"ScriptableObjects/LetterSoundGameIntros/FinalIntros/{UnitManager.Instance.GetCurrentLetter().ToString().ToLower()}new").introSounds;

        }
            
        else
            introSounds = Resources.Load<LetterSoundIntros>($"ScriptableObjects/LetterSoundGameIntros/a").introSounds;
        PlayTutorialSounds();
    }

    public override void LoadSounds()
    {
        
    }
    //3 pa 5
    public override void PlaySound(AudioClip clip)// audioPrefab
    {
        //audioManager.PlayOneShot(clip);
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }

    public override void PlayTutorialSounds()// playSoundDelay
    {
        playSoundDelay = new Task(PlaySoundDelay());
    }

    public void ReplayTutorial() // ReplayTutorial  lala
    {
        lala.Talk();
        playSoundDelay = new Task(PlaySoundDelay(true));
    }

    IEnumerator PlaySoundDelay(bool repeat = false)//  PlaySoundDelay bool false
    {
        //dodato ovo ispod radi cekanja dok lala mahne na pocetku
        if(!repeat)
            yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
        int i = 0;
        var clipToRepeat = introSounds[1];
        foreach(var clip in introSounds)
        {
            PlaySound(clip);
            if (i == 0)
            {
                yield return new WaitForSeconds(2f);
                gameManager.ActivateLetterPopUp();
                yield return new WaitForSeconds(clip.length - 2f);
            }
            else if(i == 1)
            {
                yield return new WaitForSeconds(2.8f);
                gameManager.ActivateIllustrationPopUp();
                PlaySound(soundPack.correctAnswerSound);
                yield return new WaitForSeconds(clip.length - 2.8f);
            }
            else
            {
                yield return new WaitForSeconds(clip.length);
            }

            i++;
        }

        PlaySound(clipToRepeat);// play sound  WaitForSeconds  clipToRepeat length
        yield return new WaitForSeconds(clipToRepeat.length);
        introEnded.Invoke();
        gameManager.lalaNarator.interactable = true;
        playSoundDelay.Stop();
       
        
    }

    IEnumerator IllustrationPopUp(float delay)//ActivateIllustrationPopUp
    {
        yield return new WaitForSeconds(delay);
        gameManager.ActivateIllustrationPopUp();
        TargetWordIllustrationShowUp.Stop();
    }

    IEnumerator EndOfIntroDelay(float delay)// delay introEnded
    {
        yield return new WaitForSeconds(delay);
        introEnded.Invoke();
    }

    public override void PlayQuestionResultSound(bool correct) //PlayQuestionResultSound
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckIfMusicPlaying() //CheckIfMusicPlaying
    {
        throw new System.NotImplementedException();
    }
}
