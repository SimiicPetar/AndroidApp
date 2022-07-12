using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitWoodPrefabScript :  Waypoint
{
    // Start is called before the first frame update
    public delegate void onWoodClicked(UnitInfo info);
    public onWoodClicked OnWoodClicked;

    public UnitInfo unitInfo;

    public SpriteRenderer Lock;
    public GameObject letterSpritesParent;

    public string LastUnitID;

    GameManager gameManager;
    UIMapManager uiMapManager;
    AvatarBase avatarBase;
    bool interactable = true;

    bool HandPointerWasActive = false;

    public GameObject StickerImageWhenUnitIsFinished;
    public GameObject HandPointerAnimator;
    public Image GlowEffect;
    Task StartAnimatorTask;
    [Space]
    public int HoleBeforeThisUnitID;



    private void OnEnable()
    {
        UIMapManager.OnWindowOpened += HandPointerHide;
        UIMapManager.OnWindowClosed += HandPointerShow;
    }



    private void OnDisable()
    {
        UIMapManager.OnWindowOpened -= HandPointerHide;
        UIMapManager.OnWindowClosed -= HandPointerShow;
    }

    private void Start()
    {
        avatarBase = AvatarBase.Instance;
        gameManager = GameManager.Instance;
        uiMapManager = UIMapManager.Instance;
        InitializeWoodPrefab();
    }
    
    void HandPointerHide(UIWindow win)
    {
        if (HandPointerAnimator.activeSelf)
        {
            HandPointerAnimator.SetActive(false);
            HandPointerWasActive = true;
        }
    }

    void HandPointerShow(UIWindow win)
    {
        if (HandPointerWasActive)
        {
            HandPointerAnimator.SetActive(true);
            HandPointerWasActive = false;
        }
    }

    IEnumerator DelayInitialization()
    {

        yield return new WaitUntil(() => UnitStatisticsBase.Instance != null);

        if (!UnitStatisticsBase.Instance.CheckIfHoleIsFinished(HoleBeforeThisUnitID))
        {
            interactable = false;
            yield return null;
        }
        else if (unitInfo.unitId == "afo" || UnitStatisticsBase.Instance.CheckIfUnitIsFinished(LastUnitID))
        {
            Lock.enabled = false;
            interactable = true;
            if (UnitStatisticsBase.Instance.CheckIfUnitIsFinished(unitInfo.unitId))
            {
                StickerImageWhenUnitIsFinished.SetActive(true);
                letterSpritesParent.SetActive(true);
                if (avatarBase.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
                    letterSpritesParent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"SLOVA ZA PANJEVE/{unitInfo.unitId} malo");
                else
                    letterSpritesParent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"SLOVA ZA PANJEVE/{unitInfo.unitId}");
            }

        }
        else
        {
            interactable = false;
        }
    }

    private void InitializeWoodPrefab()
    {
        StartCoroutine(DelayInitialization());
        //ako je holeid == -1 vratimo true svakako
       
       /* if (unitInfo.unitId == UnitStatisticsBase.Instance.GetMostCurrentUnitID())
            StartAnimatorTask = new Task(StartHandPointerIfNotClicking());*/
    }

    IEnumerator StartHandPointerIfNotClicking()
    {
        yield return new WaitForSeconds(5f);
        HandPointerAnimator.SetActive(true);

    }
    // Update is called once per frame
    private void OnMouseOver()
    {
        if(interactable && Input.GetMouseButtonDown(0) && UnitStatisticsBase.Instance.CheckIfHoleIsFinished(HoleBeforeThisUnitID))
        {
            // gameManager.SetCurrentUnit(unitInfo);
            //uiMapManager.OpenUnitWindow(unitInfo);
            // MapAvatarController.Instance.WalkToAUnit(unitInfo);
            if (HandPointerAnimator.activeSelf)
                NextUnitIndicatorController.Instance.ClickedOnUnitWithHandPointer();
            MapAvatarController.Instance.MoveAvatarToTheWaypoint(this);
            
        }
            
    }


    public override bool CheckIfWalkable()
    {
        Debug.Log($"Instanca UnitStatisticBase je {UnitStatisticsBase.Instance}");
        if (UnitStatisticsBase.Instance.CheckIfUnitIsFinished(LastUnitID) && UnitStatisticsBase.Instance.CheckIfHoleIsFinished(HoleBeforeThisUnitID))
            return true;
        else
            return false;
    }
}
