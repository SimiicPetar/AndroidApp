using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Patterns.Singleton;
using DG.Tweening;

public class StickerAlbumManager : Singleton<StickerAlbumManager> {

    public delegate void onNewCardReceived(char letter);
    public static onNewCardReceived OnNewCardReceived; 

    public List<StickerAlbumInfo> unitAlbumInfoList;

    Dictionary<DragableCard, CardSlot> cardPairs;


    public List<CardSlot> RewardCardSlots;

    public List<DragableCard> RewardCardsUpsideDown;

    public GameObject StickerAlbumParent;

    public GameObject AlbumBeforeButton;

    public GameObject NextAlbumButton;

    public GameObject BackToMapButton;

    public GameObject GoToMapAfterStickingButton;

    int currentOpenedAlbumIndex = 0;

     bool isAnacondaReward = false;

    [Space]
    [Header("leve i desne strane contenti")]
    public LeftPageLogic LeftPage1;
    public LeftPageLogic LeftPage2;
    public RightPageLogic RightPage1;
    public RightPageLogic RightPage2;

    RightPageLogic visibleRightPage;
    LeftPageLogic visibleLeftPage;
    LeftPageLogic invisibleLeftPage;
    RightPageLogic invisibleRightPage;

    LeftPageLogicLalaSticking lalaLeftPage;

    bool flipBan = false;
    bool isEndOfTheGame = false;

    public float pageFlipDuration = 0.8f;

    public LeftPageLogic VisibleLeftPage { get => visibleLeftPage; set => visibleLeftPage = value; }
    public RightPageLogic VisibleRightPage { get => visibleRightPage; set => visibleRightPage = value; }
    public bool IsEndOfTheGame { get => isEndOfTheGame; set => isEndOfTheGame = value; }
    public bool IsAnacondaReward { get => isAnacondaReward; set => isAnacondaReward = value; }

    public CanvasGroup AlbumCanvasGroup;

    private void ResetAlbum()
    {
        visibleLeftPage.ResetLeftPage();
        invisibleLeftPage.ResetLeftPage();
        currentOpenedAlbumIndex = 0;
    }


    private void Start()
    {
        VisibleRightPage = RightPage1;
        VisibleLeftPage = LeftPage1;
        invisibleRightPage = RightPage2;
        invisibleLeftPage = LeftPage2;
    }

    void SetupForStickingAfterGame()
    {
        AlbumBeforeButton.SetActive(false);
        NextAlbumButton.SetActive(false);
        BackToMapButton.SetActive(false);
    }

    void SetupForScrolling()
    {
        AlbumBeforeButton.SetActive(true);
        NextAlbumButton.SetActive(true);
        BackToMapButton.SetActive(true);
        GoToMapAfterStickingButton.SetActive(false);
    }

    public void RewardAfterAnaconda(bool endGame = false)
    {
        
        StartCoroutine(RewardAfterAnacondaCoroutine(endGame));
    }

    public void GiveRewardAfterTheGame(char letterFinished)
    {
        StartCoroutine(RewardAfterGameCoroutine(letterFinished));
    }

    void FadeInAlbum()
    {
        StickerAlbumParent.SetActive(true);
        AlbumCanvasGroup.alpha = 0f;
        DOTween.To(() => AlbumCanvasGroup.alpha, x => AlbumCanvasGroup.alpha = x, 1, 0.3f);
    }

    IEnumerator RewardAfterAnacondaCoroutine(bool endgame = false)
    {
        isEndOfTheGame = endgame;
        isAnacondaReward = true;
        //ako je endgame ne treba da ceka 6 sekundi nego odma da popuni sve
        if (endgame)
        {
            StickerAlbumInfo currentAlbumInfo;
            yield return new WaitForSeconds(0.2f);
            AudioManagerMap.Instance.PlayLalaAlbumPageSpeech();
            StickerAlbumParent.SetActive(true);
            FadeInAlbum();
            SetupForStickingAfterGame();
            currentAlbumInfo = unitAlbumInfoList.Last();
            VisibleRightPage = RightPage1;
            VisibleLeftPage = LeftPage1;
            invisibleRightPage = RightPage2;
            invisibleLeftPage = LeftPage2;

            VisibleLeftPage.FillLeftPageForLalaForSticking(currentAlbumInfo, 'a', 7);
            VisibleRightPage.FillRightSide(currentAlbumInfo, true);
        }
        else
        {
            //treba da se otvori unit window i da lala prica 6 sekundi pa onda da se upali stiker album

            UIMapManager.Instance.OpenUnitWindow(GameManager.Instance.currentUnit);
            AudioManagerMap.Instance.PlayAfterAnacondaReward();
            yield return new WaitForSeconds(AudioManagerMap.Instance.RewardAfterAnaconda.length - 0.2f);
            FadeInAlbum();
            
            
            SetupForStickingAfterGame();
            StickerAlbumInfo currentAlbumInfo;
            if (!endgame)
                currentAlbumInfo = unitAlbumInfoList.FirstOrDefault(info => info.unitInfo == GameManager.Instance.currentUnit);
            else
                currentAlbumInfo = unitAlbumInfoList.Last();
            VisibleRightPage = RightPage1;
            VisibleLeftPage = LeftPage1;
            invisibleRightPage = RightPage2;
            invisibleLeftPage = LeftPage2;

            VisibleLeftPage.FillLeftPage(currentAlbumInfo, unitAlbumInfoList.IndexOf(currentAlbumInfo));
            VisibleRightPage.FillRightSideForSticking(currentAlbumInfo);
        }
      

        
    }

    IEnumerator RewardAfterGameCoroutine(char letterFinished)
    {


        yield return new WaitForSeconds(0f);
        VisibleRightPage = RightPage1;
        VisibleLeftPage = LeftPage1;
        invisibleRightPage = RightPage2;
        invisibleLeftPage = LeftPage2;

        FadeInAlbum();
        StickerAlbumParent.SetActive(true);

        //GameManager.Instance.ReturningFromGameIndicator = false;
        SetupForStickingAfterGame();

        var currentAlbumInfo = unitAlbumInfoList.FirstOrDefault(info => info.unitInfo == GameManager.Instance.currentUnit);

        bool completed = UnitStatisticsBase.Instance.CheckIfUnitIsFinished(currentAlbumInfo.unitInfo.unitId);

        VisibleLeftPage.FillLeftPageForSticking(currentAlbumInfo, letterFinished, unitAlbumInfoList.IndexOf(currentAlbumInfo));

        VisibleLeftPage.ActivateDragableCard(currentAlbumInfo, letterFinished);

        VisibleRightPage.FillRightSide(currentAlbumInfo, completed);
        //houseName.text = currentAlbumInfo.NameOfTheHouse;

        //ovde logika za lalinog drugara umesto postojeceg
        if (completed)
        {
            //lalaFriendImage.GetComponent<Animator>().SetTrigger("GrowUp");

            AudioManagerMap.Instance.PlayAfterAnacondaReward();

        }
        else
        {
            AudioManagerMap.Instance.PlayAfterManateeReward();
        }

        UIMapManager.Instance.OpenStickerAlbumWindow();
        StickerAlbumParent.SetActive(true);
    }

    private void OnEnable()
    {
        cardPairs = new Dictionary<DragableCard, CardSlot>();
        UIMapManager.OnStickerAlbumClosed += ResetAlbum;
        DragableCard.onCardPlacedInSlot += AllowToGoBackToMap;

        AlbumBeforeButton.GetComponent<Button>().interactable = false;
        if (!UnitStatisticsBase.Instance.CheckIfUnitIsFinished("afo"))
        {
            SetButtonVisibility(NextAlbumButton, false);
        }
    }

    private void OnDisable()
    {
        UIMapManager.OnStickerAlbumClosed -= ResetAlbum;
        DragableCard.onCardPlacedInSlot -= AllowToGoBackToMap;
    }

    public Vector3 GetCardSlotPosition(DragableCard card)
    {
        return RewardCardSlots.FirstOrDefault(s => s.cardSlotChar == card.CardLetter.ToString()).transform.position;
    }

    public void FillUpAlbum(int index = 0) 
    {
        InitialNavigationSetup();
        //dodao sad da vidim da li to prouzrokuje problem onaj sa nestajucim desnim stranama
        LeftPage1.transform.parent.localScale = new Vector3(1f, 1f, 1f);
        LeftPage2.transform.parent.localScale = new Vector3(0f, 1f, 1f);
        RightPage2.transform.parent.localScale = new Vector3(1f, 1f, 1f);
        RightPage1.transform.parent.localScale = new Vector3(1f, 1f, 1f);

        LeftPage1.transform.parent.SetSiblingIndex(0);
        LeftPage2.transform.parent.SetSiblingIndex(1);
        RightPage2.transform.parent.SetSiblingIndex(2);
        RightPage1.transform.parent.SetSiblingIndex(3);

        VisibleRightPage = RightPage1;
        VisibleLeftPage = LeftPage1;
        invisibleRightPage = RightPage2;
        invisibleLeftPage = LeftPage2;

        var currentAlbumInfo = unitAlbumInfoList[index];
        UIMapManager.Instance.OpenStickerAlbumWindow();
        SetupForScrolling();
        VisibleRightPage.FillRightSide(currentAlbumInfo, UnitStatisticsBase.Instance.CheckIfUnitIsFinished(currentAlbumInfo.unitInfo.unitId));
        VisibleLeftPage.FillLeftPage(currentAlbumInfo, index);
        UIMapManager.Instance.OpenStickerAlbumWindow();
    }

    void SetButtonVisibility(GameObject buttonObject, bool visibility)
    {
        buttonObject.GetComponent<Button>().interactable = visibility;
        Color c = buttonObject.GetComponent<Image>().color;
        c.a = visibility ? 1 : 0;
        buttonObject.GetComponent<Image>().color = c;
    }

    #region album flipping
    public void ShowAlbumBefore()
    {   

        if (currentOpenedAlbumIndex - 1 >= 0 && !flipBan)
        {
            var currentAlbumInfo = unitAlbumInfoList[currentOpenedAlbumIndex - 1];
            if(currentOpenedAlbumIndex == 0)
            {
                SetButtonVisibility(AlbumBeforeButton, false);
            }
                
            SetButtonVisibility(NextAlbumButton, true);
            AnimatePageTransitionBackward();
        }  
    }

    private void SwapRefference<T>(ref T param1, ref T param2)
    {
        T temp = param1;
        param1 = param2;
        param2 = temp;
    }

    public void SwapSibilingIndexesInHierarchy(Transform transform1, Transform transform2)
    {
        int ind1 = transform1.GetSiblingIndex();
        int ind2 = transform2.GetSiblingIndex();
        transform1.SetSiblingIndex(ind2);
        transform2.SetSiblingIndex(ind1);
    }

    void AnimatePageTransitionBackward()
    {
        AudioManagerMap.Instance.PlaySound(AudioManagerMap.Instance.PageFlippingSoundStickerAlbum);
        flipBan = true;
        var currentAlbumInfo = unitAlbumInfoList[--currentOpenedAlbumIndex];

        if (currentOpenedAlbumIndex == 0)
        {
            SetButtonVisibility(AlbumBeforeButton, false);

        }
            

        //treba mi visible left da se skalira na 0 i invisible right da se skalira na 1
        invisibleRightPage.FillRightSide(currentAlbumInfo, UnitStatisticsBase.Instance.CheckIfUnitIsFinished(currentAlbumInfo.unitInfo.unitId));
        invisibleLeftPage.FillLeftPage(currentAlbumInfo, currentOpenedAlbumIndex);

        SwapSibilingIndexesInHierarchy(VisibleLeftPage.transform.parent, invisibleLeftPage.transform.parent);

        invisibleLeftPage.transform.parent.localScale = new Vector3(1f, 1f, 1f);

        VisibleLeftPage.transform.parent.DOScaleX(0f, pageFlipDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            SwapRefference(ref visibleLeftPage, ref invisibleLeftPage);

            SwapSibilingIndexesInHierarchy(VisibleRightPage.transform.parent, invisibleRightPage.transform.parent);

            invisibleRightPage.transform.parent.DOScaleX(1f, pageFlipDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                VisibleRightPage.transform.parent.localScale = new Vector3(0f, 1f, 1f);

                SwapRefference(ref invisibleRightPage, ref visibleRightPage);

                flipBan = false;
            });

        });
    }

    void InitialNavigationSetup()
    {
        var currentAlbumInfo = unitAlbumInfoList[0];
        SetButtonVisibility(AlbumBeforeButton, false);
        if (UnitStatisticsBase.Instance.CheckIfUnitIsUnlocked(unitAlbumInfoList[1].unitInfo.unitId))
        {
            SetButtonVisibility(NextAlbumButton ,true);
        }
           
        else
            SetButtonVisibility(NextAlbumButton, false);

    }

    //Transform.SetSiblingIndex(index);.
    void AnimatePageTransitionForward()
    {
        AudioManagerMap.Instance.PlaySound(AudioManagerMap.Instance.PageFlippingSoundStickerAlbum);
        flipBan = true;
        var currentAlbumInfo = unitAlbumInfoList[++currentOpenedAlbumIndex];

        if (currentOpenedAlbumIndex == unitAlbumInfoList.Count - 1 || !UnitStatisticsBase.Instance.CheckIfUnitIsUnlocked(unitAlbumInfoList[currentOpenedAlbumIndex + 1].unitInfo.unitId))
            SetButtonVisibility(NextAlbumButton, false);

        invisibleRightPage.FillRightSide(currentAlbumInfo, UnitStatisticsBase.Instance.CheckIfUnitIsFinished(currentAlbumInfo.unitInfo.unitId));
        invisibleLeftPage.FillLeftPage(currentAlbumInfo, currentOpenedAlbumIndex);

        //pre nego sto krenemo da smanjujemo desnu stranu odmah uvecamo ovu ispod koja je prethodno smanjena
        invisibleRightPage.transform.parent.localScale = new Vector3(1f, 1f, 1f);
        //treba odmah popuniti sadrzaj nevidljivih stranica pre nego sto se okrenu stranice 
        Debug.Log($"(PRE SWITCHOVANJA) scale od nevidljivog desnog : {invisibleRightPage.transform.parent.gameObject.name} { invisibleRightPage.transform.parent.localScale}, scale od vidljivog {VisibleRightPage.transform.parent.gameObject.name} { visibleRightPage.transform.parent.localScale}");

        VisibleRightPage.transform.parent.DOScaleX(0f, pageFlipDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            Debug.Log($"(nakon sto se vidljiva desna strana resetovala na 0f) scale od nevidljivog desnog : {invisibleRightPage.transform.parent.gameObject.name} { invisibleRightPage.transform.parent.localScale}, scale od vidljivog {VisibleRightPage.transform.parent.gameObject.name} { visibleRightPage.transform.parent.localScale}");

            SwapSibilingIndexesInHierarchy(VisibleRightPage.transform.parent, invisibleRightPage.transform.parent);

            SwapRefference(ref visibleRightPage, ref invisibleRightPage);
            Debug.Log($"(Nakon switcha referenci i mesta u hierarhiji) scale od nevidljivog desnog : {invisibleRightPage.transform.parent.gameObject.name} { invisibleRightPage.transform.parent.localScale}, scale od vidljivog {VisibleRightPage.transform.parent.gameObject.name} { visibleRightPage.transform.parent.localScale}");

            invisibleLeftPage.transform.parent.DOScaleX(1f, pageFlipDuration).SetEase(Ease.Linear).OnComplete(() => {

                VisibleLeftPage.transform.parent.localScale = new Vector3(0f, 1f, 1f);

                SwapSibilingIndexesInHierarchy(VisibleLeftPage.transform.parent, invisibleLeftPage.transform.parent);

                SwapRefference(ref invisibleLeftPage, ref visibleLeftPage);
                flipBan = false;
                Debug.Log($"(nakon celog zavrsetka animacije )scale od nevidljivog desnog : {invisibleRightPage.transform.parent.gameObject.name} { invisibleRightPage.transform.parent.localScale}, scale od vidljivog {VisibleRightPage.transform.parent.gameObject.name} { visibleRightPage.transform.parent.localScale}");
            });
        });

      
        
    }

    public void ShowNextUnitAlbum()
    {
       
        if (currentOpenedAlbumIndex + 1 <= unitAlbumInfoList.Count && !flipBan)
        {
            var currentAlbumInfo = unitAlbumInfoList[currentOpenedAlbumIndex + 1];

            if (UnitStatisticsBase.Instance.CheckIfUnitIsUnlocked(currentAlbumInfo.unitInfo.unitId))
            {
                SetButtonVisibility(AlbumBeforeButton, true);

                if (currentOpenedAlbumIndex == unitAlbumInfoList.Count - 1)
                    SetButtonVisibility(NextAlbumButton, false);

                //reorder pagecontent refferences
                AnimatePageTransitionForward();
            }
        }
    }
    #endregion
    public void AllowToGoBackToMap()
    {

        GoToMapAfterStickingButton.SetActive(true);
    }
}
