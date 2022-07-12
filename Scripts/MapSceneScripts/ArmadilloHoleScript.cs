using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmadilloHoleScript : Waypoint
{
    // Start is called before the first frame update
    public int holeID;
    public string LastUnitId;
    public string NextUnitId;
    public UnitWoodPrefabScript UnitBeforeHole;
    public UnitWoodPrefabScript UnitAfterHole;
    public SpriteRenderer HoleMask;
    public List<UnitInfo> unitsBeforeHole;
    public GameObject HandPointerAnimator;

    public Transform PointAboveHole;

    bool HandPointerWasActive = false;
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

    //pitacu da li je zavrsen unit pre i posle rupe, ako je zavrsen tad nikom nista ako nije zavrsen unit posle rupe a jeste ovaj pre onda ta rupa treba da svetli hehee

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


    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && UnitStatisticsBase.Instance.CheckIfUnitIsFinished(UnitBeforeHole.unitInfo.unitId))
        {
            // MapAvatarController.Instance.MoveToTheHole(this);
            if (HandPointerAnimator.activeSelf)
                HandPointerAnimator.SetActive(false);
            if (transform.GetChild(1).gameObject.activeSelf)//da se ugasi glow 
                transform.GetChild(1).gameObject.SetActive(false);
            MapAvatarController.Instance.MoveAvatarToTheWaypoint(this);
            GameManager.Instance.SetLettersForConsolidation(LastUnitId, NextUnitId, holeID);
        }
    }

    public void MaskTheHole()
    {
        List<ArmadilloHoleScript> otherHoles = FindObjectsOfType<ArmadilloHoleScript>().ToList();
        if(holeID == 3)
        {
            var anteater = GameObject.FindGameObjectsWithTag("anteatermap")[0];
            anteater.GetComponent<SkeletonMecanim>().gameObject.GetComponent<MeshRenderer>().sortingOrder = 4;
        }
        
        foreach (var hole in otherHoles)
        {
            if (hole != this)
                hole.transform.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
        MapAvatarController.Instance.avatarMecanim.gameObject.GetComponent<MeshRenderer>().sortingOrder = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckIfWalkable()
    {
        if (UnitStatisticsBase.Instance.CheckIfUnitIsFinished(UnitBeforeHole.unitInfo.unitId))
            return true;
        else return false;
    }
}
