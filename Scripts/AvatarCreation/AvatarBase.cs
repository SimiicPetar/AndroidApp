using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public struct AvatarSpineInfo
{
    public Dictionary<string, AttachmentColorPair> spineAvatarInfo ;
    public string AvatarName;
    public TypeOfChosenLetterFont typeOfChosenLetterFont;
    

    public AvatarSpineInfo(Dictionary<string, AttachmentColorPair> avatarInfo, string name, TypeOfChosenLetterFont fontType)
    {
        spineAvatarInfo = new Dictionary<string, AttachmentColorPair>();
        spineAvatarInfo = avatarInfo;
        AvatarName = name;
        typeOfChosenLetterFont = fontType;
    }
}
public class AvatarBase : MonoBehaviour
{
    public delegate void AvatarBaseLoaded();
    public static AvatarBaseLoaded onAvatarBaseLoaded;

    public delegate void AvatarDeleted(int id);
    public AvatarDeleted onAvatarDeleted;

    // Start is called before the first frame update
    static AvatarBase _instance = null;
    public static AvatarBase Instance { get { return _instance; } }

    public List<int> AvatarIdList;
    public static Dictionary<int, AvatarInfo> AvatarDictionary;

    public static Dictionary<int, AvatarSpineInfo> AvatarSpineDictionary;

    public int SelectedID;

    public int ActiveAvatarKey;

    public TypeOfChosenLetterFont typeOfChosenFont;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SetActiveAvatarKey(int key, TypeOfChosenLetterFont type)
    {
        ActiveAvatarKey = key;
        typeOfChosenFont = type;
    }

    public void SetID(int id)
    {
        SelectedID = id;
    }

    public AvatarSpineInfo GetActiveAvatar()
    {
        return AvatarSpineDictionary[ActiveAvatarKey];
    }
    public void SaveSpineDictionary(AvatarSpineInfo spineInfo)
    {
        if (AvatarSpineDictionary.ContainsKey(SelectedID))
        {
            AvatarSpineDictionary[SelectedID] = spineInfo;
        } else
            AvatarSpineDictionary.Add(SelectedID, spineInfo);
          ES3.Save(EasySaveKeys.AvatarSpineDictionaryKey, AvatarSpineDictionary);
    }

    public void SaveDictionary(AvatarInfo info)
    {
        try
        {
            if (AvatarDictionary.ContainsKey(SelectedID))
                AvatarDictionary[SelectedID] = info;
            else
                AvatarDictionary.Add(SelectedID, info);
            ES3.Save(EasySaveKeys.AvatarDictionaryKey, AvatarDictionary);
           
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }   
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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

        if(scene.name == "ChooseAvatarScene")
        {
            if(AvatarDictionary == null)
                AvatarDictionary = new Dictionary<int, AvatarInfo>();
            if (AvatarSpineDictionary == null)
                AvatarSpineDictionary = new Dictionary<int, AvatarSpineInfo>();
            if (AvatarDictionary.Count > 0)
            {
                onAvatarBaseLoaded.Invoke();
            }
        
            else
            {
                if (ES3.KeyExists(EasySaveKeys.AvatarDictionaryKey))
                {
                    AvatarDictionary = ES3.Load<Dictionary<int, AvatarInfo>>(EasySaveKeys.AvatarDictionaryKey);
              
                }

                if (ES3.KeyExists(EasySaveKeys.AvatarSpineDictionaryKey))
                {
                    AvatarSpineDictionary = ES3.Load<Dictionary<int, AvatarSpineInfo>>(EasySaveKeys.AvatarSpineDictionaryKey);
                   
                }
               
            }
            //AvatarIdList
            AvatarIdList.Clear();
            foreach(var key in AvatarSpineDictionary.Keys)
            {
                AvatarIdList.Add(key);
            }
            onAvatarBaseLoaded.Invoke();
        }
    }

    private void LoadSavedAvatars()
    {
        
    }

    public void RemoveAvatar(int avatarID)
    {
        if (AvatarDictionary.ContainsKey(avatarID))
        {
            AvatarDictionary.Remove(avatarID);
            UnitStatisticsBase.Instance.RemoveAProgress(avatarID);
            ES3.Save(EasySaveKeys.AvatarDictionaryKey, AvatarDictionary);

            AvatarIdList.Clear();
            foreach (var key in AvatarDictionary.Keys)
            {
                AvatarIdList.Add(key);
            }

            onAvatarDeleted.Invoke(avatarID);
        }
        if (AvatarSpineDictionary.ContainsKey(avatarID))
        {
            AvatarSpineDictionary.Remove(avatarID);
            ES3.Save(EasySaveKeys.AvatarSpineDictionaryKey, AvatarSpineDictionary);
        }
    }

}
