using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUnitProgress 
{
    public List<char> unitID;
    public List<UnitLetterProgress> unitLetterProgressList;
    public bool CapibaraCompleted = false;
    public bool AnteaterCompleted = false;
    public bool BlendingCompleted = false;

    public CurrentUnitProgress()
    {
        unitLetterProgressList = new List<UnitLetterProgress>();
        unitID = new List<char>();
    }

    public CurrentUnitProgress(List<char> id)
    {
        unitLetterProgressList = new List<UnitLetterProgress>();
        unitID = new List<char>(id);
        foreach (var letter in unitID)
        {
            unitLetterProgressList.Add(new UnitLetterProgress(letter));
        }
    }
    
    public bool CheckIfUnlockableGameIsFinished(BasicGames code)
    { 
        if (code == BasicGames.CAPIBARA)
            return CapibaraCompleted;
        if (code == BasicGames.ANTEATER)
            return AnteaterCompleted;
        if (code == BasicGames.BLENDING)
            return BlendingCompleted;
        else return false;
    }

    public void AddProgressForUnlockableGame(BasicGames code)
    {
        if (code == BasicGames.CAPIBARA)
            CapibaraCompleted = true;
        else if (code == BasicGames.ANTEATER)
            AnteaterCompleted = true;
        else if (code == BasicGames.BLENDING)
            BlendingCompleted = true;
    }

    public void AddProgressForALetter(char letter, BasicGames code)
    {
        foreach(var item in unitLetterProgressList)
        {
            if(item.letter == letter)
            {
                item.MiniGameCompleted(code);
                break;
            }
        }
    }

    public override string ToString()
    {
        string retval = "";
        foreach (var item in unitLetterProgressList)
            retval += item.ToString() + "\n";
        retval += $"Capibara game finished :{CapibaraCompleted} \n";
        retval += $"Anteater game finished:{AnteaterCompleted} \n";
        return retval;
       
    }

    public bool CheckIfUnitIsPlayed()
    {
        foreach (var item in unitLetterProgressList)
        {
            if (item.CheckIfUnitIsPlayedOnce())
                return true;
        }
        return false;
    }

    public bool LetterFinishedCheck(char letter)
    {
        foreach (var item in unitLetterProgressList)
        {
            if (item.letter == letter)
            {
                return item.CheckIfLetterIsFinished();
            }
        }
        return false;
    }
}
