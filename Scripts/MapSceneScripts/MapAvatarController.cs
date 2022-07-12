using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Spine.Unity;

public class MapAvatarController : MonoBehaviour
{
    static MapAvatarController _instance = null;
    public static MapAvatarController Instance { get { return _instance; } }

    public static UnitWoodPrefabScript CurrentWood { get => currentWood; set => currentWood = value; }

    const string IdleTrigger = "Idle";
    const string WalkTrigger = "Walk";
    const string JumpTrigger = "Jump";
    const string DanceTrigger = "Dance";

    List<Collider> allColliders;

    int currentWaypointIndex = -1;

     Vector3 beginningWoodPosition = new Vector3(-8.74f, 1.5f, 0);

    public Vector3 LalaGatePosition;

    public AvatarSpineDressUp mapAvatar;

    public SkeletonMecanim avatarMecanim;
    Animator avatarAnimator;

    public List<Waypoint> WaypointList;

    public List<UnitWoodPrefabScript> WoodPrefabs;

    public static bool FinishedWholeGame = false;

    static UnitWoodPrefabScript currentWood;

    ArmadilloHoleScript currentHole;

    UnitInfo currentUnit;

    bool movingAllowed = true;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        UIMapManager.OnStickerAlbumClosed += EnableMovementWhenAlbumIsClosed;
        UIMapManager.OnWindowOpened += DisableMovementWhenInAlbum;
        allColliders = FindObjectsOfType<Collider>(true).ToList();
    }

    private void OnDisable()
    {

        UIMapManager.OnStickerAlbumClosed -= EnableMovementWhenAlbumIsClosed;
        UIMapManager.OnWindowOpened -= DisableMovementWhenInAlbum;
    }


    private void Start()
    {
        
        avatarAnimator = mapAvatar.gameObject.GetComponent<Animator>();
        if(UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4 && GameManager.Instance.CurrentHoleIndex == 4)
        {
            FinishedWholeGame = true;
        }
        DressAvatarUp();
        if (GameManager.Instance.ReturningFromHoleIndicator)
        {
            AvatarJumpOutOfTheHole();
            GameManager.Instance.ReturningFromHoleIndicator = false;
            // PlaceAvatarAtCurrentUnit();
        }
        else if (UnitStatisticsBase.Instance.CheckIfCharacterFinishedGame())
            PlaceAvatarAtTheStart();
        else    
            StartCoroutine(WaitForUnitStatisticInitializationForPlaceAvatarAtCurrentUnit());
    }

    public void DressAvatarUp()
    {
        mapAvatar.Dressup(AvatarBase.AvatarSpineDictionary[AvatarBase.Instance.ActiveAvatarKey]);
    }

    IEnumerator SceneChangeHelp(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("ConsolidationGame");
    }

    public void MoveAvatarToTheWaypoint(Waypoint waypoint)
    {
        if(movingAllowed)
            StartCoroutine(MoveAvatarToTheWaypointCoroutine(waypoint));
    }

    private void EnableMovementWhenAlbumIsClosed()
    {
            movingAllowed = true;
        foreach (var coll in allColliders)
            coll.enabled = true;
    }
    

    void DisableMovementWhenInAlbum(UIWindow win)
    {
        allColliders = FindObjectsOfType<Collider>(true).ToList();
        if (win == UIMapManager.Instance.StickerAlbum)
        {
            movingAllowed = false;
            foreach (var coll in allColliders)
                coll.enabled = false;
        }
    }

    public void AvatarJumpOutOfTheHole()
    {
        StartCoroutine(JumpOutOfTheHole());
    }

    IEnumerator JumpOutOfTheHole()
    {
        int currentHoleWaypointIndex = 0;
        foreach(var wayp in WaypointList)
        {
            if(wayp.GetComponent<ArmadilloHoleScript>() != null && wayp.GetComponent<ArmadilloHoleScript>().holeID == GameManager.Instance.CurrentHoleIndex)
            {
                currentHoleWaypointIndex = wayp.WayPointIndex;
            }
        }


        var currentHole = WaypointList[currentHoleWaypointIndex].GetComponent<ArmadilloHoleScript>();
        mapAvatar.transform.parent.position = WaypointList[currentHoleWaypointIndex].transform.position - new Vector3(0f, 0.3f + 1f, 0f);
        mapAvatar.transform.parent.DOMove(currentHole.transform.position + new Vector3(0f, 0.3f + 1.4f, 0f), 0.7f, false).OnComplete(() =>
        {
            mapAvatar.transform.parent.DOMove(currentHole.PointToWalkTo.transform.position, 0.2f, false).OnComplete(() => {
                currentWaypointIndex = currentHoleWaypointIndex;
                GameManager.Instance.CurrentHoleIndex = -1;
                DetermineWherePlayerShouldGo();
                if (GameManager.Instance.EndOfLalaGameIndicator)
                {
                    foreach (var coll in allColliders)
                        coll.enabled = false;
                    avatarAnimator.SetTrigger("Walk");
                    mapAvatar.transform.parent.DOMove(LalaGatePosition, 2f).OnComplete(() =>
                    {
                        GoToEndScene();
                    });
                }

                
            });
            
        });
        yield return new WaitForSeconds(0.9f);

    }

    void GoToEndScene()
    {
        StartCoroutine(Jump());
    }
    //192 253.8
    IEnumerator Jump()
    {
        AudioManagerMap.Instance.PlayAvatarJumpingSoundEffect();
        avatarAnimator.SetTrigger(JumpTrigger);
        yield return new WaitForSeconds(AudioManagerMap.Instance.AvatarJumpingSoundEffect.length);
        AudioManagerMap.Instance.PlayEndGameLalaSpeech();
        yield return new WaitForSeconds(AudioManagerMap.Instance.EndGameMapLalaSpeech.length + 0.3f);
        SceneManager.LoadScene("EndGameScene");
    }

    IEnumerator MoveAvatarToTheWaypointCoroutine(Waypoint wayPoint)
    {
      
        bool isAHole = wayPoint.GetComponent<ArmadilloHoleScript>() != null; // ovde odredjujemo da li se radi o rupi 
        bool increment = currentWaypointIndex < wayPoint.WayPointIndex; // ovde odredjujemo da li cemo ici u napred ili u nazad
        UnitInfo newUnit = null;
        if (wayPoint.GetComponent<UnitWoodPrefabScript>() != null)
            newUnit = wayPoint.GetComponent<UnitWoodPrefabScript>().unitInfo;

        int wayPointIndex = wayPoint.WayPointIndex;

        int numberOfLoops = Mathf.Abs(wayPointIndex - currentWaypointIndex);

        int count = numberOfLoops;

        if(wayPoint.WayPointIndex == currentWaypointIndex && !isAHole)
        {
            currentWood = WaypointList[WaypointList.IndexOf(wayPoint)].GetComponent<UnitWoodPrefabScript>();
            currentUnit = newUnit;
            GameManager.Instance.SetCurrentUnit(newUnit);
            UIMapManager.Instance.OpenUnitWindow(newUnit);
            foreach (var obj in FindObjectsOfType<UnlockableUnitGame>().ToList())
            {
                obj.StartClickBanWhenEnteringUnit();
            }
            yield break;
        }

        if (increment)
            currentWaypointIndex++;
        else
            currentWaypointIndex--;

        if(numberOfLoops == 0 && isAHole)
        {
            currentHole = wayPoint.GetComponent<ArmadilloHoleScript>();
            wayPoint.GetComponent<ArmadilloHoleScript>().MaskTheHole();
            avatarAnimator.SetTrigger(IdleTrigger);
            GameManager.Instance.CurrentHoleIndex = currentHole.holeID;
            mapAvatar.transform.parent.DOMove(wayPoint.GetComponent<ArmadilloHoleScript>().transform.position + new Vector3(0f, 0.3f + 1f, 0f), 0.3f, false).OnComplete(() =>
            {
                mapAvatar.transform.parent.DOMove(mapAvatar.transform.parent.position - new Vector3(0f, 4.5f, 0f), 0.7f, false).OnComplete(() =>
                {
                    SceneManager.LoadScene("ConsolidationGame");
                });
            });
        }

        avatarAnimator.SetTrigger(WalkTrigger);
        foreach (var coll in allColliders)
            coll.enabled = false;
        while (count > 0)
        {
            mapAvatar.transform.parent.DOMove(WaypointList[currentWaypointIndex].PointToWalkTo.transform.position, 1.8f, false).SetEase(Ease.Linear).OnComplete(() => { 
               //mapAvatar.transform.parent.DOMove(WaypointList[currentWaypointIndex].transform.position + new Vector3(0f, 0.3f, 0f), 1.8f, false).SetEase(Ease.Linear).OnComplete(() => {

               count--;
                if (increment)
                    currentWaypointIndex++;
                else
                    currentWaypointIndex--;
                if(count == 0 && isAHole)
                {
                    CurrentWood = WaypointList[WaypointList.IndexOf(wayPoint) - 1].GetComponent<UnitWoodPrefabScript>();
                    currentHole = wayPoint.GetComponent<ArmadilloHoleScript>();
                    wayPoint.GetComponent<ArmadilloHoleScript>().MaskTheHole();
                    avatarAnimator.SetTrigger(IdleTrigger);
                    GameManager.Instance.CurrentHoleIndex = currentHole.holeID; 
                    mapAvatar.transform.parent.DOMove(wayPoint.GetComponent<ArmadilloHoleScript>().transform.position + new Vector3(0f, 0.3f + 1f, 0f), 0.3f, false).OnComplete(() =>
                    {
                        mapAvatar.transform.parent.DOMove(mapAvatar.transform.parent.position - new Vector3(0f, 4.5f, 0f), 0.7f, false).OnComplete(() =>
                        {
                            SceneManager.LoadScene("ConsolidationGame");
                        });
                    });
                }
            });
            yield return new WaitForSeconds(2f);
        }

        currentWaypointIndex = wayPoint.WayPointIndex;
        avatarAnimator.SetTrigger(IdleTrigger);

        foreach (var coll in allColliders)
            coll.enabled = true;
        if (newUnit != null)
        {
            currentWood = WaypointList[WaypointList.IndexOf(wayPoint)].GetComponent<UnitWoodPrefabScript>();
            currentUnit = newUnit;
            GameManager.Instance.SetCurrentUnit(newUnit);
            UIMapManager.Instance.OpenUnitWindow(newUnit);
            foreach (var obj in FindObjectsOfType<UnlockableUnitGame>().ToList())
            {
                obj.StartClickBanWhenEnteringUnit();
            }
        }
        
       
    }

    public void PlaceAvatarAtTheStart()
    {
        mapAvatar.transform.parent.transform.position = beginningWoodPosition;
    }

    IEnumerator WaitForUnitStatisticInitializationForPlaceAvatarAtCurrentUnit()
    {
        yield return new WaitUntil(() => UnitStatisticsBase.Instance != null);

        PlaceAvatarAtCurrentUnit();
    }

    void PlaceAvatarAtCurrentUnit()
    {
        int maxindex = -1;
        Waypoint foundWaypoint;
        foreach (var waypoint in WaypointList)
        {
            if (waypoint.CheckIfWalkable())
            {

                maxindex = waypoint.WayPointIndex;

            }
                
        }

        foundWaypoint = WaypointList[maxindex];
        Debug.Log(foundWaypoint.gameObject.name);
        if (maxindex == 0)
        {
            if(!UnitProgressManager.Instance.CheckIfAnyGamePlayedInAnyUnit())
                mapAvatar.transform.parent.position = beginningWoodPosition;
            else
            {
                mapAvatar.transform.parent.position = WaypointList[maxindex].PointToWalkTo.transform.position;
                currentWaypointIndex = foundWaypoint.WayPointIndex;
            }
                


        }
        else
        {
            if(GameManager.Instance.currentUnit != null)
            {
                int unitIndex = 0;
                foreach (var waypoint in WaypointList)
                {
                    if (waypoint.GetComponent<UnitWoodPrefabScript>() != null)
                    {
                        if (waypoint.GetComponent<UnitWoodPrefabScript>().unitInfo == GameManager.Instance.currentUnit)
                            unitIndex = waypoint.WayPointIndex;
                    }
                    
                }
                // mapAvatar.transform.parent.position = WaypointList[unitIndex].transform.position + new Vector3(0f, 0.3f, 0f);
                mapAvatar.transform.parent.position = WaypointList[unitIndex].PointToWalkTo.transform.position;
                currentWaypointIndex = unitIndex;
            }
            else
            {
                //  mapAvatar.transform.parent.position = WaypointList[maxindex - 1].transform.position + new Vector3(0f, 0.3f, 0f);
                mapAvatar.transform.parent.position = WaypointList[maxindex - 1].PointToWalkTo.transform.position;
                currentWaypointIndex = maxindex - 1;
            }
            
        }

        DetermineWherePlayerShouldGo();

    }

    public GameObject GetHandPointer() // da bih resio bag
    {
        int maxindex = 0;
        //imam indekse predjenih rupa i imam indekse predjenih unita, proverim da li je veci zadnji indeks od predjene rupe od zadnjeg indeksa predjenog unita i onda odlucim de cu da stavim ovog mog
        foreach (var waypoint in WaypointList)
        {
            if (waypoint.CheckIfWalkable())
                maxindex = waypoint.WayPointIndex;
        }

        WaypointList[maxindex].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        return WaypointList[maxindex].gameObject.transform.GetChild(0).gameObject;
    }

    void DetermineWherePlayerShouldGo()
    {
        if (UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4)
            return;
        int maxindex = 0;
        //imam indekse predjenih rupa i imam indekse predjenih unita, proverim da li je veci zadnji indeks od predjene rupe od zadnjeg indeksa predjenog unita i onda odlucim de cu da stavim ovog mog
        foreach(var waypoint in WaypointList)
        {
            if (waypoint.CheckIfWalkable())
                maxindex = waypoint.WayPointIndex;
        }
        if (!FinishedWholeGame)
        {
            NextUnitIndicatorController.Instance.InitiateFiveSecondsWaitForPointer(WaypointList[maxindex].gameObject.transform.GetChild(0).gameObject, WaypointList[maxindex]);
            WaypointList[maxindex].gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
       
    }


}
