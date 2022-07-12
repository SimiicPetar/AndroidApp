using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum BasicGames {LETTER_SOUND, TRACING, MANATEE, CAPIBARA, ANTEATER, BLENDING};
public class UnitProgressManager : MonoBehaviour
{

    static UnitProgressManager _instance = null;

    public static UnitProgressManager Instance { get { return _instance; } }

    public CurrentUnitProgress CurrentUnitProgress { get => currentUnitProgress; set => currentUnitProgress = value; }

    UnitManager unitManager;

    CurrentUnitProgress currentUnitProgress;

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
        unitManager = UnitManager.Instance;
        //LoadProgressForAUnit();
    }

    public bool CheckIfLetterIsFinished(char letter, int index)
    {
        return UnitStatisticsBase.Instance.GetUnitProgressAtIndex(index).LetterFinishedCheck(letter);
    }

    public bool CheckIfLetterIsFinished(char letter)
    {
        unitManager = UnitManager.Instance;
        if (currentUnitProgress == null)
            LoadProgressForAUnit();
        if (currentUnitProgress == null)
            return false;
        else
            return UnitStatisticsBase.Instance.GetCurrentUnitProgress().LetterFinishedCheck(letter);
    }

    public bool CheckIfAllLettersAreFinished()
    {
        unitManager = UnitManager.Instance;
        var currentProgress = UnitStatisticsBase.Instance.GetCurrentUnitProgress();
        if (currentProgress != null)
        {
            bool[] temp = new bool[] { false, false, false };
            int i = 0;
            foreach (var letter in unitManager.GetAllUnitLetters())
            {
                temp[i] = currentProgress.LetterFinishedCheck(letter);
                i++;
            }

            foreach (var item in temp)
            {
                if (!item)
                    return false;
            }

            return true;
        }
        else
            return false;
        
    }

    public bool CheckIfAnyGamePlayedInAnyUnit()
    {    // ako nijedno od ovih slova nije presao onda nije ni igrao
        var UnitStatisticList = UnitStatisticsBase.Instance.GetAvatarunitStatisticList(AvatarBase.Instance.ActiveAvatarKey);
        foreach (var item in UnitStatisticList)
        {
            if (item.unitProgress.CheckIfUnitIsPlayed())
                return true;
        }
        return false;
    }

    public bool CheckIfUnclockableGameIsFinished(BasicGames gameCode)
    {
        unitManager = UnitManager.Instance;
        if (UnitStatisticsBase.Instance != null)
        {
            Debug.Log($"unit progress koji smo izvukli iz baze je : {UnitStatisticsBase.Instance.GetCurrentUnitProgress()}");
            Debug.Log($"igra {gameCode.ToString()} je zavrsena {UnitStatisticsBase.Instance.GetCurrentUnitProgress().CheckIfUnlockableGameIsFinished(gameCode)}");
            return UnitStatisticsBase.Instance.GetCurrentUnitProgress().CheckIfUnlockableGameIsFinished(gameCode);
        }
        else
            return false;
    }

    public void AddProgressForUnlockableGame(BasicGames code)
    {
        UnitStatisticsBase.Instance.GetCurrentUnitProgress().AddProgressForUnlockableGame(code);
        UnitStatisticsBase.Instance.SaveCurrentunitProgress(UnitStatisticsBase.Instance.GetCurrentUnitProgress());
    }

    public void SaveProgressForALetterInUnit(char letter, BasicGames gameCode)
    {


        UnitStatisticsBase.Instance.GetCurrentUnitProgress().AddProgressForALetter(letter, gameCode);

        //ES3.Save<CurrentUnitProgress>(temp + EasySaveKeys.UnitProgress, currentUnitProgress);
        UnitStatisticsBase.Instance.SaveCurrentunitProgress(UnitStatisticsBase.Instance.GetCurrentUnitProgress());
    }
    
    public void  LoadProgressForAUnit()
    {
        if (ES3.KeyExists(EasySaveKeys.AvatarProgressBaseKey))
        {
            currentUnitProgress = UnitStatisticsBase.Instance.GetCurrentUnitProgress();
            Debug.Log("ovo je izvuceni iz avatara" + currentUnitProgress);
            return;
        }  
    }
}
