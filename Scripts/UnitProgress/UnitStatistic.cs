using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UnitStatistic 
{
    // Start is called before the first frame update

    public UnitStatistic(string id)
    {
        unitID = id;
        unitProgress = new CurrentUnitProgress(id.ToCharArray().ToList());
        manateeGameResultDictionary = new Dictionary<char, GameResultManatee>();
        foreach(var letter in id)
        {
            manateeGameResultDictionary.Add(letter, new GameResultManatee());
        }
    }
    public UnitStatistic()
    {

    }
    string unitID;

    public CurrentUnitProgress unitProgress;

    public GameResult capybaraGameResult;

    public GameResultAnteater anteaterGameResult;

    public GameResultManatee manateeGameResult;

    public Dictionary<char,GameResultManatee> manateeGameResultDictionary;

    public string UnitID { get => unitID; set => unitID = value; }
}
