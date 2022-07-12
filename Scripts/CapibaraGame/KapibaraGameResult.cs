using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct GameResult
{
    public Dictionary<char, float> LetterScorePairs;
    public List<string> WrongWords;
    public List<char> GameLetters;
    public int score;

    public GameResult(Dictionary<char, float> dict, List<string> WWords, List<char> lettersInGame, int gameScore)
    {
        LetterScorePairs = new Dictionary<char, float>(dict);
        WrongWords = new List<string>(WWords);
        GameLetters = new List<char>(lettersInGame);
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
        foreach (var word in WrongWords)
            retval += word;
        return retval;
    }
}

public class KapibaraGameResult 
{
    Dictionary<char, float> LetterScorePairs;
    List<string> WrongWords;

    const int numberOfUniqueLetterPerGame = 4;

    float averagePercentage = 0;

    List<char> AllLetters;

    int score;
    public KapibaraGameResult(string letters)
    {
        WrongWords = new List<string>();
        AllLetters = new List<char>();
        LetterScorePairs = new Dictionary<char, float>();
        
        foreach (var letter in letters)
        {
            LetterScorePairs.Add(letter, 0f);
            AllLetters.Add(letter);

        }
            
    }

    public void GenerateAScoreForGameSkipping()
    {
        float average = 0;
        score = 12;
        foreach (var kvp in LetterScorePairs.Keys.ToList())
        {
            LetterScorePairs[kvp] = 4;
            average += LetterScorePairs[kvp];
            Debug.Log($"{kvp} {LetterScorePairs[kvp]} pokusaja");
            //  LetterScorePairs[kvp] = (numberOfUniqueLetterPerGame / LetterScorePairs[kvp]) * 100;

        }
        GameResult result = new GameResult(LetterScorePairs, WrongWords, AllLetters, score);
        ES3.Save<GameResult>(EasySaveKeys.KapibaraMiniGame, result);
        UnitStatisticsBase.Instance.SaveCapybara(result);
    }

    public void AddProgressForALetter(char letter, int num)
    {
        LetterScorePairs[letter] += num;
    }

    public void AddWrongWord(string word)
    {
        if (!WrongWords.Contains(word))
            WrongWords.Add(word);
    }

    public void CalculateScoreForEachLetter(int score)
    {
        float average = 0;
        this.score = score;
       
        foreach(var kvp in LetterScorePairs.Keys.ToList())
        {
            average += LetterScorePairs[kvp];
            Debug.Log($"{kvp} {LetterScorePairs[kvp]} pokusaja");
          //  LetterScorePairs[kvp] = (numberOfUniqueLetterPerGame / LetterScorePairs[kvp]) * 100;
            
        }
        averagePercentage = (LetterScorePairs.Count * numberOfUniqueLetterPerGame / average) * 100 ;

        GameResult result = new GameResult(LetterScorePairs, WrongWords, AllLetters, score);
        ES3.Save<GameResult>(EasySaveKeys.KapibaraMiniGame, result);
        UnitStatisticsBase.Instance.SaveCapybara(result);
    }

    public override string ToString()
    {
        string retval = "";
        retval += $"igrac je osvojio {this.score}\n";
        foreach(var kvp in LetterScorePairs)
        {
            retval += kvp.Key + ":" + kvp.Value.ToString();
        }

        retval += $"\n prosecan procenat ucinkovitosti:{averagePercentage}%";
        retval += $"\n broj reci sa greskom : {WrongWords.Count}, \n te reci su:";
        foreach (var word in WrongWords)
            retval += word + " ";



        return retval;
    }

}
