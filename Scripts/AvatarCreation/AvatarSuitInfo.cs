using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SuitObjectInfo", menuName = "ScriptableObjects/AvatarInfoObjects/SuitInfo", order = 1)]
public class AvatarSuitInfo : ScriptableObject
{
    public string HatSlotName;
    public string SuitSlotName;
    public string LArmSlotName;
    public string RArmSlotName;
    public string LForearmSlotName;
    public string RForearmSlotName;
    public string RThighSlotName;
    public string LThighSlotName;
    public string LLegSlotName;
    public string RLegSlotName;

    public void GetDictOfValues(out string hatSlotName, out string suitSlotName)
    {
        hatSlotName = HatSlotName;
        suitSlotName = SuitSlotName;
    }

    public bool CheckIfSuitContainsSleeves()
    {
        return LForearmSlotName != "" || RForearmSlotName != "";
    }

    public bool CheckIfSuitContainsLegs()
    {
        return LLegSlotName != "" || RLegSlotName != "";
    }
}
