using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _instance = null;
    public static UIManager Instance { get { return _instance; } }

    public LalaFriendClickable ActiveFriend { get  { return lalaFriendsInTheHouse.FirstOrDefault(x => x.unitID == GameManager.Instance.currentUnit.unitId); } set { activeFriend = value; } }


    public List<SelectableUnitLetter> selectableUnitLetters;

    public List<UnlockableUnitGame> unlockableUnitGames;

    public List<LalaFriendClickable> lalaFriendsInTheHouse;

    private LalaFriendClickable activeFriend;

    GameManager gameManager;
    UnitProgressManager unitProgressManager;

    public UnitLalaController lalaController;

    public GameObject mapView;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        UIMapManager.OnWindowClosed += ResetFriend;
    }

    private void OnDisable()
    {
        UIMapManager.OnWindowClosed -= ResetFriend;
    }

    void ResetFriend(UIWindow win)
    {
        if (win == UIMapManager.Instance.UnitView)
            ActiveFriend.gameObject.SetActive(false);

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        unitProgressManager = UnitProgressManager.Instance;
    }

    public void UnitViewSetup()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            unitProgressManager = UnitProgressManager.Instance;
        }
        var currentProgress = UnitStatisticsBase.Instance.GetCurrentUnitProgress();
        if (!currentProgress.CheckIfUnitIsPlayed())
        {
            AudioManagerMap.Instance.PlayWelcomeToTheHouse(gameManager.currentUnit.unitId);
        }else if (currentProgress.CheckIfUnitIsPlayed() && !unitProgressManager.CheckIfAllLettersAreFinished())
        {
            AudioManagerMap.Instance.PlayTouchTheLetter();
        }else if (!currentProgress.CapibaraCompleted)
        {
            AudioManagerMap.Instance.PlayPlayCapybara();
        }else if (!currentProgress.AnteaterCompleted)
        {
            AudioManagerMap.Instance.PlayPlayAnteater();
        }else if (!currentProgress.BlendingCompleted)
        {
            AudioManagerMap.Instance.PlayPlayBlending();
        }

        //LalaFriendImage.sprite = gameManager.currentUnit.LalaFriendSprite;
        ActiveFriend = lalaFriendsInTheHouse.FirstOrDefault(x => x.unitID == GameManager.Instance.currentUnit.unitId);
        ActiveFriend.gameObject.SetActive(true);

        if (!gameManager.allowAllGames)
        {
            var list = GameManager.Instance.currentUnit.unitLetters;
            if (GameManager.Instance.currentUnit.unitNumber == 5)
                list = new List<char> { 'l', 'c', 'j' };
            else if (GameManager.Instance.currentUnit.unitNumber == 6)
                list = new List<char> { 't', 'n', 'b' };

            for (int i = 0; i < selectableUnitLetters.Count; i++)
            {
                selectableUnitLetters[i].SetText(list[i ]);
                selectableUnitLetters[i].SetTextColor(UnitProgressManager.Instance.CheckIfLetterIsFinished(selectableUnitLetters[i].letter, UnitManager.Instance.AllUnitsList.IndexOf(GameManager.Instance.currentUnit)));
            }

            for (int i = 0; i < unlockableUnitGames.Count; i++)
            {
                unlockableUnitGames[i].SetButtonInteractable(unitProgressManager.CheckIfAllLettersAreFinished());
            }
        }
        
    }

    public void LoadManateeGame()
    {
        SceneManager.LoadScene("ManateeGame");
    }
}
