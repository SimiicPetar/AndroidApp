using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitManager : MonoBehaviour
{
    static UnitManager _instance = null;

    public static UnitManager Instance { get { return _instance; } }
    public List<UnitInfo> AllUnitsList;
    [Header("for anteater game")]
    public LetterSoundPairs letterSoundPairsAnteater;
    public LetterSoundPairs letterSoundPairsManatee;
    [Header("for letter introduction game")]
    public WordLetterSoundPairs wordLetterSoundPairs;

    List<char> completedLetters;

    char currentLetter;

    UnitInfo currentUnit = null;

    GameManager gameManager;

    Dictionary<char, List<string>> LetterWordPairs;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }            
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LetterWordPairs = new Dictionary<char, List<string>>();
        gameManager = GameManager.Instance;
    }

    public void LoadWordsForUnit(UnitInfo unit)
    {
        //ovde sam obrisao jedan if
        if(LetterWordPairs == null)
            LetterWordPairs = new Dictionary<char, List<string>>();
        currentUnit = unit;
            var chosenLetters = unit.unitLetters;
            LetterWordPairs.Clear();
            foreach (var letter in chosenLetters)
            {
                var obj = LoadWords(letter);
                LetterWordPairs.Add(letter, obj.Words);
            }
       
        
    }


    public List<char> GetAllUnitLetters()
    {
        if (LetterWordPairs == null)
            LoadWordsForUnit(gameManager.currentUnit);
        var letterList = new List<char>();
        foreach (var key in LetterWordPairs.Keys)
            letterList.Add(key);

        return letterList;
    }


    UnitWordsScriptableObject LoadWords(char letter)
    {
        return Resources.Load<UnitWordsScriptableObject>(Paths.UnitWords + char.ToUpper(letter) + "Words");
    }

    public char GetCurrentLetter()
    {
        return currentLetter;
    }

    public void SetCurrentLetter(char letter)
    {
        currentLetter = letter;
        SceneManager.LoadScene("LetterSoundScene");
    }

}
