using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public enum TypeOfChosenLetterFont { UPPERCASE, LOWERCASE};

/*
 * 
 * 
 * */

public struct AvatarInfo
{
    public Color skinColor;
    public Color hairColor;
    public int hairstyleIndex;
    public int outfitIndex;
    public string AvatarName;
    public TypeOfChosenLetterFont typeOfChosenLetterFont;
    
    public override string ToString()
    {
        string retval = "";
        retval += $"avatarName : {AvatarName}";
        retval += $"skinColor : {skinColor}";
        retval += $"hairColor : {hairColor}";
        retval += $"hairstyleIndex : {hairstyleIndex} ";
        retval += $"outfitIndex : {outfitIndex}";
        retval += $"type of chosen letters: {typeOfChosenLetterFont}"; 
        return retval;
    }
}



public class AvatarAssetManager : MonoBehaviour
{
    List<string> avatarNames = new List<string> {"pedro", "diego", "amanda", "irene", "thiago", "ronaldo", "felipe", "sanchez", "maria"};

    static AvatarAssetManager _instance = null;
    public static AvatarAssetManager Instance { get { return _instance; } }

    public bool HairSelected { get => hairSelected; set => hairSelected = value; }

    // Start is called before the first frame update
    public List<GameObject> HairstyleObjects;
    public List<GameObject> Outfits;
    public ColorChanger skinColorSlider;

    public TextMeshProUGUI avatarNameText;

    public TMP_InputField inputField;

    bool hairSelected = false;

    GameObject currentHairstyle = null;
    GameObject currentOutfit = null;
    Color currentHairColor;

    AvatarInfo loadedStruct;

    AvatarSuitInfo selectedSuit;

    public TypeOfChosenLetterFont chosenLetterFont = TypeOfChosenLetterFont.UPPERCASE;

    [Header("Default avatar atributes")]
    public int DefaultOutfitIndex;
    public Color DefaultSkinColor;
    public Color DefaultHairColor;
    public int DefaultHairstyleIndex;



    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
        }       
        else
            Destroy(gameObject);     
    }

    private void OnEnable()
    {
        TypeOfFontSelectionButton.onSelectFonTypeButtonClicked += SetChosenTypeOfFont;
    }

    private void OnDisable()
    {
        TypeOfFontSelectionButton.onSelectFonTypeButtonClicked -= SetChosenTypeOfFont;
    }

    void LoadAvatarInfo()
    {
       
            if (ES3.KeyExists(EasySaveKeys.AvatarKey))
            {
                AvatarInfo info = ES3.Load<AvatarInfo>(EasySaveKeys.AvatarKey);
                loadedStruct = info;
                Debug.Log(info);
                DressUpWithSavedInfo();
            }

    }

    public void OnNameValueChanged(string letters)
    {
        string text = inputField.text;
        string[] textArray = text.Split(' ');
        if (textArray.Length > 1 )
        {
            foreach (string item in textArray)
            {
                char.ToUpper(item[0]);
            }
        }
        
        

        //inputField.text = inputField.text.ToUpper();
        //UIManagerAvatarCreation.Instance.AvatarNameOnLog.text = inputField.text.ToUpper();
    }

    public bool CheckIfValuesAreChosen()
    {
        print("Color Changer " + ColorChanger.SkinColorChosen);
        return currentOutfit != null && ColorChanger.SkinColorChosen;
    }

    void SetChosenTypeOfFont(TypeOfChosenLetterFont chosenType)
    {
        chosenLetterFont = chosenType;
    }

    void DressUpWithSavedInfo()
    {
        
    }

    private void Start()
    {
        inputField.characterLimit = 12;
        LoadAvatarInfo();
    }
    //248 221 161
    public void SaveAvatar()
    {
        UIManagerAvatarCreation.Instance.createAvatarButton.interactable = false;
        AvatarInfo info = new AvatarInfo();

        if (currentHairColor == new Color(0f, 0f, 0f, 0f))
            info.hairColor = new Color(0f, 0f, 0f, 1f);
        else
            info.hairColor = currentHairColor;

        if(skinColorSlider.ChosenColor == new Color(0f, 0f, 0f, 0f))
            info.skinColor = new Color(1f, 1f, 1f, 1f);
        else
            info.skinColor = skinColorSlider.ChosenColor;

        if (HairstyleObjects.FindIndex(o => o.activeInHierarchy) == -1)
            info.hairstyleIndex = -1;
        else
            info.hairstyleIndex = HairstyleObjects.FindIndex(o => o.activeInHierarchy);

        if (Outfits.FindIndex(o => o.activeInHierarchy) == -1)
            info.outfitIndex = -1;
        else
            info.outfitIndex = Outfits.FindIndex(o => o.activeInHierarchy);

        Debug.Log("ime koje ste uneli je " + avatarNameText.text);
        
        if(string.IsNullOrEmpty(inputField.text.Trim()))
        {
            info.AvatarName = $"Jugador{AvatarBase.Instance.ActiveAvatarKey}";
        }
        else
        {
            info.AvatarName = inputField.text;
        }
        info.typeOfChosenLetterFont = chosenLetterFont;
        Debug.Log("sacuvana struktura : " + info);
        ES3.Save<AvatarInfo>(EasySaveKeys.AvatarKey, info);
        if(AvatarBase.Instance != null)
        {
            AvatarBase.Instance.SaveDictionary(info);
            AvatarBase.Instance.SaveSpineDictionary(new AvatarSpineInfo(AvatarSpriteAttacher.Instance.SaveAvatarDictionary(), inputField.text, chosenLetterFont));
            //AvatarBase.Instance.SaveSpineDictionary(new AvatarSpineInfo(AvatarSpriteAttacher.Instance.GetAvatarDictionary(), inputField.text, chosenLetterFont));
            UnitStatisticsBase.Instance.AddNewProgress(AvatarBase.Instance.ActiveAvatarKey);
        }
        AvatarBase.Instance.SetActiveAvatarKey(AvatarBase.Instance.ActiveAvatarKey, chosenLetterFont);
        ES3.Save(EasySaveKeys.ActiveAvatarDictionaryKey, AvatarBase.Instance.ActiveAvatarKey);
        ES3.Save(EasySaveKeys.ActiveAvatarKey, chosenLetterFont);
        SceneManager.LoadScene("MapScene");
        AvatarSpineDressUp.onActiveCharacterChanged?.Invoke();
    }

    public void ActivateClickedOutfit(GameObject outfitRef , AvatarSuitInfo suit)
    {
        AvatarSpriteAttacher.Instance.SetSuit(suit);
        CheckIfAnyOutfitActive();
        GameObject obj = Outfits.FirstOrDefault(o => o == outfitRef);
        currentOutfit = obj;
        obj.SetActive(true);
    }

    public void ActivateClickedHair(GameObject hairstyleRef, string hairstyleSlotName)
    {
        AvatarSpriteAttacher.Instance.ChangeAvatarHairstyle(hairstyleSlotName);
        CheckIfAnyHairActive();
        GameObject obj = HairstyleObjects.FirstOrDefault(o => o == hairstyleRef);
        obj.SetActive(true);
        currentHairstyle = obj;
        hairSelected = true;
    }

    void CheckIfAnyOutfitActive()
    {
        foreach(var obj in Outfits)
        {
            if (obj.activeSelf)
                obj.SetActive(false);
        }
    }
    void CheckIfAnyHairActive()
    {
        foreach(var obj in HairstyleObjects)
        {
            if (obj.activeSelf)
                obj.SetActive(false);
        }
    }

    public void ChangeHairColor(Color newHairColor)
    {
        if(currentHairstyle != null)
        {
            AvatarSpriteAttacher.Instance.ChangeAvatarHairstyleColor(newHairColor);
            currentHairstyle.GetComponent<SpriteRenderer>().color = newHairColor;
            currentHairColor = newHairColor;
        }
    }

}
