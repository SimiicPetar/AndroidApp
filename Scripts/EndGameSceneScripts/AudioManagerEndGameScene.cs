using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerEndGameScene : AudioManagerBase
{

    public AudioClip EndGameSoundtrack;
    public static AudioManagerEndGameScene _instance = null;

    public static AudioManagerEndGameScene Instace { get { return _instance; } }


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    #region not needed for this one
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

    

    public override void PlayTutorialSounds()
    {
        throw new System.NotImplementedException();
    }


    #endregion

    public void PlayBackgroundMusic()
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(EndGameSoundtrack, true);
        pref.ChangeMute(GameManager.Instance.MuteBGMusicBool);
    }

    public override void PlaySound(AudioClip clip)
    {
        AudioSourcePrefab pref = Instantiate(GameManager.Instance.audioPrefab, null);
        pref.Initialize(clip);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
