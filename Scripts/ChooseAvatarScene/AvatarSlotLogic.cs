using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarSlotLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public int SlotID;

    public XButtonLogic xButton;

    AvatarBase avatarBaseInstance;

    AvatarInfo slotInfo;

    AvatarSpineInfo slotInfoSpine;

    public TextMeshProUGUI avatarName;

    public ChildAvatarCreateAvatarScene avatar;

    public AddNewAvatarButton avatarButton;

    public SpriteRenderer TypeOfFontTag;

    public Sprite LowercaseTag;

    public Sprite UppercaseTag;

    public TypeOfFontTagButton TagButton;

    public AvatarSpineDressUp avatarDressup;

    private void Awake()
    {
        AvatarBase.onAvatarBaseLoaded += GetInfo;
    }
    private void OnDisable()
    {
        AvatarBase.onAvatarBaseLoaded -= GetInfo;
        AvatarBase.Instance.onAvatarDeleted -= RefreshSlot;
        ThrashCanButton.Instance.onTrashCanClicked -= ActivateXButton;
        ThrashCanButton.Instance.onClickedOnBackground -= DeactivateXButton;
        DeleteAvatarPopup.OnButtonClicked -= DeactivateXButton;

    }
    void Start()
    {
        
        ThrashCanButton.Instance.onTrashCanClicked += ActivateXButton;
        ThrashCanButton.Instance.onClickedOnBackground += DeactivateXButton;
        DeleteAvatarPopup.OnButtonClicked += DeactivateXButton;
        AvatarBase.Instance.onAvatarDeleted += RefreshSlot;
        avatarBaseInstance = AvatarBase.Instance;
    }
    
    void ActivateXButton()
    {
        if ( avatarDressup.gameObject.activeSelf)
        {
            //avatar.GetComponent<Collider>().enabled = false;
            xButton.gameObject.SetActive(true);
        }
 
    }

    void DeactivateXButton()
    {
        if ( avatarDressup.gameObject.activeSelf)
        {
           // avatar.GetComponent<Collider>().enabled = true;
            xButton.gameObject.SetActive(false);
        }
       
    }

    void RefreshSlot(int id)
    {
        if(id == SlotID)
        {
            avatarDressup.GetComponent<Collider>().enabled = true;
            slotInfoSpine = default;
            avatarDressup.gameObject.SetActive(false);
            xButton.gameObject.SetActive(false);
            avatarName.text = "";
            avatarButton.gameObject.SetActive(true);
            TagButton.SetSprite(null);
           // gameObject.SetActive(false);
            SlotLayoutManager.Instance.FillExistingIDList();
         
        }
        else
        {
            avatarDressup.GetComponent<Collider>().enabled = true;
            xButton.gameObject.SetActive(false);
        }
    }
    //avatarbase.instance.


    public void DetermineSlotsLayout()
    {
       // SlotLayoutManager.Instance.FillExistingIDList();
        // da li je ovaj slot sledeci u redu za aktivaciju tj njegov id
        if (SlotLayoutManager.Instance.ExistingIDList.Contains(SlotID))
        {
            var slotSpineInfo = AvatarBase.AvatarSpineDictionary[SlotID];
            slotInfoSpine = slotSpineInfo;
            avatarDressup.gameObject.SetActive(true);
            avatarDressup.Dressup(slotSpineInfo);
            TagButton.SetSprite(slotSpineInfo.typeOfChosenLetterFont == TypeOfChosenLetterFont.UPPERCASE ? UppercaseTag : LowercaseTag);
            avatarName.text = slotSpineInfo.AvatarName;
        }
        else if (SlotID == SlotLayoutManager.Instance.DetermineNextActivationID())
        {
            //to je onda slot za plus i ide u sredinu
          //  transform.position = SlotLayoutManager.Instance.middleSlotPosition;
            avatarButton.gameObject.SetActive(true);
           // avatarName.text = "Novo jogador";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    //ovo je sve ok
    void GetInfo()
    {
            
    }

    public void ChooseAvatar()
    {
        avatarBaseInstance.SetActiveAvatarKey(SlotID, slotInfoSpine.typeOfChosenLetterFont);
        ES3.Save(EasySaveKeys.ActiveAvatarDictionaryKey, SlotID);
        ES3.Save(EasySaveKeys.ActiveAvatarKey, slotInfoSpine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToAvatarCreation()
    {
        avatarBaseInstance.SetID(SlotID);
        SceneManager.LoadScene("AvatarCreation");
    }
}
