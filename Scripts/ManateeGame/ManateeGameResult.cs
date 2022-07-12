using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameResultManatee
{
    public List<string> WrongPictures;
    public int score;

    public GameResultManatee(List<string> wrongPics, int gameScore)
    {
        WrongPictures = new List<string>(wrongPics);
        score = gameScore;
    }

    public override string ToString()
    {
        string retval = "";
        retval += "Slike koje ste pogresili :\n";
        foreach(var item in WrongPictures)
        {
            retval += $"{item}";
        }
        retval += $"Prosli put ste osvojili : {score} poena";

        return retval;
    }
}

public class ManateeGameResult 
{
    List<string> WrongWords;

  
    public  ManateeGameResult()
    {
        WrongWords = new List<string>();
    }

    private void Start()
    {
        WrongWords = new List<string>();
    }

    public void AddWrongWord(string word)
    {
        WrongWords.Add(word);
    }

    public void SaveGameReport(int gameScore)
    {
        GameResultManatee result = new GameResultManatee(WrongWords, gameScore);
        ES3.Save<GameResultManatee>(EasySaveKeys.ManateeMiniGame, result);
        UnitStatisticsBase.Instance.SaveManatee(result);
    }

}
