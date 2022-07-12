using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotLayoutManager : Singleton<SlotLayoutManager>
{
    // Start is called before the first frame update

    public List<AvatarSlotLogic> SlotActivationOrder;
    public List<int> SlotActivationIDOrder = new List<int>() { 1, 3, 10, 4, 2 };
    public List<Vector3> SlotPositions;
    public List<int> ExistingIDList;
    public Vector3 middleSlotPosition;
    public int nextActivationID;

    private void OnEnable() //OnEnable
    {
       // AvatarBase.onAvatarBaseLoaded += FillExistingIDList;
    }
    void Start()// start
    {
        FillExistingIDList();
    }


    private void OnDisable() //OnDisable
    {
       // AvatarBase.onAvatarBaseLoaded -= FillExistingIDList;
    }


    public int DetermineNextActivationID()// DetermineNextActivationID 
    {
        ExistingIDList = AvatarBase.Instance.AvatarIdList;
        List<int> helpList = SlotActivationIDOrder.Except(ExistingIDList).ToList();
        if (helpList.Count > 0)
            return helpList[0];
        else return -1;
    } 

    void DetermineSlotsPosition()// gamelogic for slots position
    {
        int middleSlotInd = DetermineNextActivationID();
        int i = 0;
        //ovde nam da sve id-jeve koji postoje 
        List<AvatarSlotLogic> helpList = SlotActivationOrder.Except(SlotActivationOrder.FindAll(x => !ExistingIDList.Contains(x.SlotID))).ToList();
        var middleSlot = SlotActivationOrder.FirstOrDefault(x => x.SlotID == middleSlotInd);
        int NumberOfSlots = helpList.Count;
        if(NumberOfSlots == 0)
        {   
            middleSlot.transform.position = middleSlotPosition;
        }
        else if(NumberOfSlots == 1)
        {
            int j = 1;
            foreach (var slot in helpList)
                slot.transform.position = SlotPositions[j];
            middleSlot.transform.position = middleSlotPosition;
        }
        else if(NumberOfSlots == 2)
        {
            helpList[0].transform.position = SlotPositions[1];
            middleSlot.transform.position = middleSlotPosition;
            helpList[1].transform.position = SlotPositions[3];
        }
        else if(NumberOfSlots == 3)
        {
            helpList[0].transform.position = SlotPositions[0];
            helpList[1].transform.position = SlotPositions[1];
            middleSlot.transform.position = middleSlotPosition;
            helpList[2].transform.position = SlotPositions[3];
        }
        else if(NumberOfSlots == 4)
        {
            helpList[0].transform.position = SlotPositions[0];
            helpList[1].transform.position = SlotPositions[1];
            middleSlot.transform.position = middleSlotPosition;
            helpList[2].transform.position = SlotPositions[3];
            helpList[3].transform.position = SlotPositions[4];
        }
        else
        {
            int j = 0;
            foreach(var slot in helpList)
            {
                slot.transform.position = SlotPositions[j];
                j++;
            }
        }
        
    }

    public void FillExistingIDList()// foreach slot in SlotActivationOrder    DetermineSlotsLayout and  DetermineSlotsPosition
    {
        ExistingIDList = AvatarBase.Instance.AvatarIdList;
        foreach (var slot in SlotActivationOrder)
            slot.DetermineSlotsLayout();
        DetermineSlotsPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
