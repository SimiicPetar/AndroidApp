using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioManagerBase : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void IntroEnded();
    public IntroEnded introEnded;

    public bool HasListenedToTutorial = false;

    public bool TutorialRepetitionIsPlaying = false;

    public SoundPackObject soundPack;

    public abstract void PlayTutorialSounds();

    public abstract void LoadSounds();

    public abstract void PlaySound(AudioClip clip);

    public abstract void PlayQuestionResultSound(bool correct);

    public abstract bool CheckIfMusicPlaying();

}
