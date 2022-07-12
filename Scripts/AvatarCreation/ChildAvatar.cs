using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HairstylesParent;
    public GameObject OutfitsParent;
    public SpriteRenderer AvatarBody;

    AvatarInfo loadedStruct;

    GameObject _savedOutfit;
    GameObject _savedHairstyle;


    void Start()
    {
        LoadAvatarInfo();
    }

    void LoadAvatarInfo()
    {
        if (ES3.KeyExists(EasySaveKeys.ActiveAvatarKey))
        {
            AvatarInfo info = ES3.Load<AvatarInfo>(EasySaveKeys.ActiveAvatarKey);
            loadedStruct = info;
            DressUpWithSavedInfo();
        }
        else if (ES3.KeyExists(EasySaveKeys.AvatarKey))
        {
            AvatarInfo info = ES3.Load<AvatarInfo>(EasySaveKeys.AvatarKey);
            loadedStruct = info;
            Debug.Log(info);
            DressUpWithSavedInfo();
        }
    }

    void DressUpWithSavedInfo()
    {
        if(loadedStruct.outfitIndex != -1)
        {
            _savedOutfit = OutfitsParent.transform.GetChild(loadedStruct.outfitIndex).gameObject;
            _savedOutfit.SetActive(true);
        }
            
        if(loadedStruct.hairstyleIndex != -1)
        {
            _savedHairstyle = HairstylesParent.transform.GetChild(loadedStruct.hairstyleIndex).gameObject;
            _savedHairstyle.SetActive(true);
        }
           
        AvatarBody.color = loadedStruct.skinColor;
        if (loadedStruct.hairstyleIndex != -1)
            _savedHairstyle.GetComponent<SpriteRenderer>().color = loadedStruct.hairColor;

    }

}
