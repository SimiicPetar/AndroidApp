using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitStatisticsBase : MonoBehaviour
{

    /*
     1.LENJIVAC AFO
     2.TUKAN SEM
     3.MAJMUN VIP
     4.DELFIN DUR
     5.VUK CLJ
     6.TAPIR BNT
     7.JAGUAR GZX
     * 
     * */



    private static UnitStatisticsBase instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
       
    }
    #region Singleton
    public static UnitStatisticsBase Instance
    {
        get {
          
            if (instance == null)
                instance = FindObjectOfType<UnitStatisticsBase>();
    
            return instance;
                    }

    }
    #endregion
    public Dictionary<int, List<UnitStatistic>> UnitProgressBase { get => unitProgressBase; set => unitProgressBase = value; } // recnik svih progresa za svakog avatara

    Dictionary<int, List<UnitStatistic>> unitProgressBase;


    public Dictionary<string, int> letterNumberPairs = new Dictionary<string, int> { { "afo", 0 }, { "sem" , 1}, { "vip", 2}, {"dur", 3 }, {"cjl", 4 }, { "bnt", 5} , {"gxz", 6 } };

    public Dictionary<int,List<int>> passedHoles;

    public Dictionary<int, bool> FinishedGameStatus;

    const string passedHolesESKey = "Passed Holes";

    const string FinishedGameESKey = "FinishedGameDictionary";

    void Start()
    {
       
        LoadProgressBase();
        SceneManager.activeSceneChanged += ChangedActiveScene;
        
    }


  

    public int GetMostCurrentHoleId()
    {
        Debug.Log($"sta se nalazi ovde molim vas");
        if(passedHoles != null)
        {
            if (passedHoles.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            {
                if (passedHoles[AvatarBase.Instance.ActiveAvatarKey].Count >= 1)
                {
                    int index = passedHoles[AvatarBase.Instance.ActiveAvatarKey][passedHoles[AvatarBase.Instance.ActiveAvatarKey].Count - 1];
                    return passedHoles[AvatarBase.Instance.ActiveAvatarKey].Count;
                }
                else
                    return -1;
            } 
            else
                return -1;
        } 
        else return -1;
    }

    public bool CheckIfFinishedGame()
    {
        return passedHoles[AvatarBase.Instance.ActiveAvatarKey].Contains(4);
    }

    public void AddNewHole(int holeID)
    {
        if (passedHoles == null)
            passedHoles = new Dictionary<int, List<int>>();
        if (!passedHoles[AvatarBase.Instance.ActiveAvatarKey].Contains(holeID))
        {
            passedHoles[AvatarBase.Instance.ActiveAvatarKey].Add(holeID);
            SaveAHole(holeID);
        }
    }

    public bool CheckIfHoleIsPassed(int holeID)
    {
        return passedHoles[AvatarBase.Instance.ActiveAvatarKey].Contains(holeID);
    }

    public void SaveAHole(int holeID = -1)
    {

        ES3.Save(passedHolesESKey, passedHoles);
    }

    public void UpdateFinishedGameStatus()
    {
        if(FinishedGameStatus == null)
            FinishedGameStatus = new Dictionary<int, bool>();

        if (FinishedGameStatus.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
        {
            FinishedGameStatus[AvatarBase.Instance.ActiveAvatarKey] = true;
        }else
        {
            FinishedGameStatus.Add(AvatarBase.Instance.ActiveAvatarKey, true);
        }
    }

    public bool CheckIfCharacterFinishedGame()
    {
        if (FinishedGameStatus == null)
            FinishedGameStatus = new Dictionary<int, bool>();
        if (FinishedGameStatus.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            return FinishedGameStatus[AvatarBase.Instance.ActiveAvatarKey];
        else
            return false;
    }

    public void SaveFinishedGameDictionary()
    {
        ES3.Save(FinishedGameESKey, FinishedGameStatus);
    }

    private void ChangedActiveScene(Scene arg0, Scene arg1)
    {
        SaveProgressBase();
    }

    public int GetMostCurrentUnitIDAsInt()
    {
        return UnitManager.Instance.AllUnitsList.FirstOrDefault(x => x.unitId == GetMostCurrentUnitID()).unitNumber;
    }

    public string GetMostCurrentUnitID()
    {
        string ret =  unitProgressBase[AvatarBase.Instance.ActiveAvatarKey][unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Count - 1].UnitID;
        return ret;
       
    }

    void LoadProgressBase()
    {
        Debug.Log("usao sam u loadovanje progress baze na pocetku");


        if (ES3.KeyExists(FinishedGameESKey))
            FinishedGameStatus = ES3.Load<Dictionary<int, bool>>(FinishedGameESKey);
        if (ES3.KeyExists(EasySaveKeys.AvatarProgressBaseKey))
        {
            Debug.Log($"postoji kljuc {EasySaveKeys.AvatarProgressBaseKey}");
            unitProgressBase = ES3.Load<Dictionary<int, List<UnitStatistic>>>(EasySaveKeys.AvatarProgressBaseKey);
            Debug.Log("u unitprogressbase se nalazi : " + unitProgressBase);
        }
        else
        {
            unitProgressBase = new Dictionary<int, List<UnitStatistic>>();
            Debug.Log("nije postojao progress base pa ga kreiramo");
        }
            

        if(unitProgressBase == null)
            unitProgressBase = new Dictionary<int, List<UnitStatistic>>();

        if(ES3.KeyExists(passedHolesESKey))
            passedHoles = ES3.Load<Dictionary<int, List<int>>>(passedHolesESKey);
        if (passedHoles == null)
        {
            Debug.Log("nisu postojale predjene rupe pa ih kreiramo");
            passedHoles = new Dictionary<int, List<int>>();
        }
            

        
    }

    public List<UnitStatistic> GetAvatarunitStatisticList(int id)
    {
        return unitProgressBase[id];
    }

    public bool CheckIfUnitIsUnlocked(string unitID)
    {
        if(unitID == "")//usecase za lalu
        {
            bool retValue = CheckIfUnitIsFinished("gxz") && CheckIfHoleIsFinished(4);
            return retValue;
        }
        foreach(var item in unitProgressBase[AvatarBase.Instance.ActiveAvatarKey])
        {
            if (item.UnitID.Equals(unitID))
                return true;
        }
        return false;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("ovih istih skripti ima : " + FindObjectsOfType<UnitStatisticsBase>().Length + " u sceni " + scene.name);
    }

    public bool CheckIfHoleIsFinished(int holeID)
    {
        if (instance == null)
            instance = this;
        if (holeID == -1)
            return true;
        else
        {
            if (passedHoles == null)
                passedHoles = new Dictionary<int, List<int>>();
            if (!passedHoles.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
                passedHoles.Add(AvatarBase.Instance.ActiveAvatarKey, new List<int>());
            if(passedHoles[AvatarBase.Instance.ActiveAvatarKey].Contains(holeID))
                return passedHoles[AvatarBase.Instance.ActiveAvatarKey].Contains(holeID);
        }
        return false;
            
    }

    public bool CheckIfUnitIsFinished(string unitID)
    {
        if (instance == null)
            instance = this;
        if (unitID.Equals(""))
        {
            return true;
        }
        if (!unitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            unitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
        foreach (var item in unitProgressBase[AvatarBase.Instance.ActiveAvatarKey])
        {
            if (item.UnitID.Equals(unitID))
            {
                if (item.unitProgress.AnteaterCompleted && item.unitProgress.BlendingCompleted && item.unitProgress.CapibaraCompleted)
                    return true;
            }
        }

        return false;
    }

    UnitStatistic GetUnitStatistic(string unitID)
    {
        if (!unitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            unitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
        return unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == unitID);
    }

    public UnitStatistic GetCurrentUnitStatistic()
    {
        return unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId);
    }

    public CurrentUnitProgress GetUnitProgressAtIndex (int index)
    {
        return unitProgressBase[AvatarBase.Instance.ActiveAvatarKey][index].unitProgress;
    }

    public float GetLetterSuccessRate(char letter, string UnitID)
    {
        var unitStatistic = GetUnitStatistic(UnitID);
        float Percentage = 0f;
        int numberOfGamesPlayed = 0;
        if (unitStatistic == null)
            return -1f;
        if(unitStatistic.capybaraGameResult.LetterScorePairs != null)
        {
            numberOfGamesPlayed++;
            Percentage += unitStatistic.capybaraGameResult.LetterScorePairs[letter];
        }
        if(unitStatistic.anteaterGameResult.LetterScorePairs != null)
        {
            numberOfGamesPlayed++;
            Percentage += unitStatistic.anteaterGameResult.LetterScorePairs[letter];
        }
        /*if(unitStatistic.unitProgress.unitLetterProgressList.FirstOrDefault(l => l.letter == letter).manateeGameCompleted)
        {
            numberOfGamesPlayed++;
            Percentage += unitStatistic.manateeGameResultDictionary[letter].score * 1.0f / 7.0f * 100.0f;
        }*/
        return Percentage;
    }

    public CurrentUnitProgress GetCurrentUnitProgress()
    {
        CurrentUnitProgress progress;
        UnitStatistic unitStatistic = null;
        if (unitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey)) // da li avatar postoji provera
        {
             unitStatistic = unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId);
            if (unitStatistic != null)
            {
                progress = unitStatistic.unitProgress;
                return progress;
            }
            else
            {
                unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));
            }
                
        }
        else
        {
            unitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
            unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));   
        }

        return unitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId).unitProgress;
    }

    void OnApplicationQuit()
    {
        SaveProgressBase();
    }



    public void SaveProgressBase()
    {
        if(unitProgressBase.Count > 0)
            ES3.Save(EasySaveKeys.AvatarProgressBaseKey, UnitProgressBase);
        SaveAHole();
        SaveFinishedGameDictionary();
    }

    public void AddNewProgress(int id)
    {
        if (unitProgressBase == null)
            unitProgressBase = new Dictionary<int, List<UnitStatistic>>();
        Debug.Log($"vrednost unit progress base :{unitProgressBase}");
        Debug.Log(AvatarBase.Instance != null);
        Debug.Log($"vrednost selektovanog ID : {AvatarBase.Instance.SelectedID}");
        unitProgressBase.Add(AvatarBase.Instance.SelectedID, new List<UnitStatistic>());
        unitProgressBase[AvatarBase.Instance.SelectedID].Add(new UnitStatistic("afo"));
        //ES3.Save<Dictionary<int, List<UnitStatistic>>>("FSAFSAZ", unitProgressBase);
    }

    public void RemoveAProgress(int id)
    {
        unitProgressBase.Remove(id);
        passedHoles.Remove(id);
        ES3.Save(EasySaveKeys.AvatarProgressBaseKey, UnitProgressBase);
        SaveProgressBase();
    }

    public void SaveCurrentunitProgress(CurrentUnitProgress progress)
    {
        
        try
        {
            if (!UnitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            {
                UnitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
                if(UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId) == null)
                {
                    UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));
              
                }
            }
            
            UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId).unitProgress =
                        progress;

        }catch(Exception e)
        {
            Debug.Log(e);
        }
        
    }

    public void SaveAnteater(GameResultAnteater gameResult)
    {
        try
        {
            if (!UnitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            {
                UnitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
                if (UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId) == null)
                {
                    UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));

                }
            }
            UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId).anteaterGameResult = gameResult;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void SaveManatee(GameResultManatee gameResult)
    {
        try
        {
            if (!UnitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            {
                UnitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
                if (UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId) == null)
                {
                    UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));

                }
            }
            UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId).manateeGameResult = gameResult;
            UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId).manateeGameResultDictionary[UnitManager.Instance.GetCurrentLetter()] = gameResult;
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
        
    }

    public void SaveCapybara(GameResult gameResult)
    {
        try
        {
            if (!UnitProgressBase.ContainsKey(AvatarBase.Instance.ActiveAvatarKey))
            {
                UnitProgressBase.Add(AvatarBase.Instance.ActiveAvatarKey, new List<UnitStatistic>());
                if (UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Find(l => l.UnitID == GameManager.Instance.currentUnit.unitId) == null)
                {
                    UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].Add(new UnitStatistic(GameManager.Instance.currentUnit.unitId));

                }
            }

            UnitProgressBase[AvatarBase.Instance.ActiveAvatarKey].FirstOrDefault(l => l.UnitID == GameManager.Instance.currentUnit.unitId).capybaraGameResult = gameResult;
        }catch(Exception e)
        {
            Debug.Log(e);
        }
        
    }
}
