using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct GameResultAnteater
{
    public Dictionary<char, float> LetterScorePairs;
    public List<char> WrongLetters;
    public List<char> AllLetters;

    public int score;

    public GameResultAnteater(Dictionary<char, float> dict, List<char> wrong, List<char> all, int gameScore)
    {
        LetterScorePairs = new Dictionary<char, float>(dict);
        WrongLetters = new List<char>(wrong);
        AllLetters = new List<char>(all);
        score = gameScore;
    }

    public override string ToString()
    {
        string retval = "";
        retval += $"igrac je osvojio {score}/12 poena\n";
        retval += "Rezultat igre :";
        foreach (var kvp in LetterScorePairs)
            retval += $"{kvp.Key} : {kvp.Value}%";
        retval += "\n reci koje su pogresne:";
        foreach (var word in WrongLetters)
            retval += word + " ";
        return retval;
    }
}

public class AnteaterGameResult 
{
    Dictionary<char, float> LetterScorePairs;
    List<char> WrongLetters;

    const int numberOfUniqueLetterPerGame = 4;

    float averagePercentage = 0;

    List<char> AllLetters;

    public AnteaterGameResult(string letters)
    {
        WrongLetters = new List<char>();
        AllLetters = new List<char>();
        LetterScorePairs = new Dictionary<char, float>();

        foreach(var letter in letters)
        {
            LetterScorePairs.Add(letter, 0f);
            AllLetters.Add(letter);
        }
    }

    public void AddProgressForALetter(char letter, int num)
    {
        LetterScorePairs[letter] += num;
    }

    public void AddWrongLetter(char letter)
    {
        if (!WrongLetters.Contains(letter))
            WrongLetters.Add(letter);
    }

    public void GenerateResultForSkipping()
    {
        float average = 0;
        var score = 12;
        foreach (var kvp in LetterScorePairs.Keys.ToList())
        {
            LetterScorePairs[kvp] = 4;
            average += LetterScorePairs[kvp];
            Debug.Log($"{kvp} {LetterScorePairs[kvp]} pokusaja");
            // LetterScorePairs[kvp] = (numberOfUniqueLetterPerGame / LetterScorePairs[kvp]) * 100;

        }
        averagePercentage = (LetterScorePairs.Count * numberOfUniqueLetterPerGame / average) * 100;

        GameResultAnteater result = new GameResultAnteater(LetterScorePairs, WrongLetters, AllLetters, score);
        Debug.Log(result);
        ES3.Save<GameResultAnteater>(EasySaveKeys.AnteaterMiniGame, result);
        UnitStatisticsBase.Instance.SaveAnteater(result);
    }

    public void CalculateScoreForEachLetter(int score)
    {
        float average = 0;


        foreach (var kvp in LetterScorePairs.Keys.ToList())
        {
            average += LetterScorePairs[kvp];
            Debug.Log($"{kvp} {LetterScorePairs[kvp]} pokusaja");
           // LetterScorePairs[kvp] = (numberOfUniqueLetterPerGame / LetterScorePairs[kvp]) * 100;

        }
        averagePercentage = (LetterScorePairs.Count * numberOfUniqueLetterPerGame / average) * 100;

        GameResultAnteater result = new GameResultAnteater(LetterScorePairs, WrongLetters, AllLetters, score);
        Debug.Log(result);
        ES3.Save<GameResultAnteater>(EasySaveKeys.AnteaterMiniGame, result);
        UnitStatisticsBase.Instance.SaveAnteater(result);
        
    }
}
