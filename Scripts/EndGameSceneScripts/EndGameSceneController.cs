using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneController : Singleton<EndGameSceneController>
{
    // Start is called before the first frame update

    public AvatarSpineDressUp avatar;
    public Animator AvatarAnimator;

    public TextMeshProUGUI AvatarNameOnLogText;
    void Start()
    {
        avatar.Dressup(AvatarBase.AvatarSpineDictionary[AvatarBase.Instance.ActiveAvatarKey]);
        AudioManagerEndGameScene.Instace.PlayBackgroundMusic();
        AvatarAnimator.SetTrigger("Dance");
        AvatarNameOnLogText.text = AvatarBase.Instance.GetActiveAvatar().AvatarName;
    }

    public void ReturnToMap()
    {
        UnitStatisticsBase.Instance.UpdateFinishedGameStatus();
        GameManager.Instance.WatchedEndGame = true; //znaci da smo odgledali endgame
        SceneManager.LoadScene("MapScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
