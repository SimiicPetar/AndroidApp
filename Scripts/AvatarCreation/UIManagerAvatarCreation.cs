using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManagerAvatarCreation : MonoBehaviour
{

    static UIManagerAvatarCreation _instance = null;
    public static UIManagerAvatarCreation Instance { get { return _instance; } }

    public Button createAvatarButton;
    [Header("Buttons for choosing font type")]
    public Image UppercaseButton;
    public Image LowercaseButton;
    public GameObject ConfirmFontTypeButton;
    [Header("Background sprites for choosing font type buttons")]
    public Sprite SelectedButtonBackground;
    public Sprite UnSelectedButtonBackground;
    [Header("Panels/Tables")]
    public GameObject AvatarCustomizationPanel;
    //public GameObject TypeOfFontSelectionPanel;
    public GameObject InputAvatarNamePanel;
    public GameObject ConfirmAvatarOverlayCanvas;

    [Header("Ime Avatara na panju")]
    public TextMeshProUGUI AvatarNameOnLog;


    [Space]
    public GameObject Panj;
    public GameObject AvatarParent;
    Vector3 startPosition;
    public TextMeshProUGUI PlayerNameText;
    public Image LetterTag;

    public Sprite lowercaseTag;
    public Sprite uppercaseTag;



    //Vector3(5.11999989,0,0)

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
       
        startPosition = AvatarParent.transform.position;
    }

   

    public void BackToCustomization()
    {
        UIMapManager.OnWindowClosed?.Invoke(InputAvatarNamePanel.GetComponent<UIWindow>());
        //TypeOfFontSelectionPanel.SetActive(false);
        InputAvatarNamePanel.SetActive(false);
        AvatarCustomizationPanel.SetActive(true);
    }

    public void BackToChooseAvatar()
    {
        ColorChanger.SkinColorChosen = false;
        SceneManager.LoadScene("ChooseAvatarScene"); 
    }

    private void OnEnable()
    {
        TypeOfFontSelectionButton.onSelectFonTypeButtonClicked += SetButtonSprites;
    }

    private void OnDisable()
    {
        TypeOfFontSelectionButton.onSelectFonTypeButtonClicked -= SetButtonSprites;
    }

    public void BackToAvatarCustomizationFromConfirm()
    {
        UIMapManager.OnWindowClosed?.Invoke(ConfirmAvatarOverlayCanvas.GetComponent<UIWindow>());
        AvatarParent.transform.position = startPosition;
        //TypeOfFontSelectionPanel.SetActive(false);
        AvatarCustomizationPanel.SetActive(true);
        InputAvatarNamePanel.SetActive(false);
        ConfirmAvatarOverlayCanvas.SetActive(false);
        Panj.SetActive(true);
    }

    public void GoToConfirmScreen()
    {
       
        UIMapManager.OnWindowClosed?.Invoke(AvatarCustomizationPanel.GetComponent<UIWindow>());
        //UIMapManager.OnWindowClosed?.Invoke(TypeOfFontSelectionPanel.GetComponent<UIWindow>());
        AvatarParent.transform.position = new Vector3(5.11999989f, 0f, 0f);

        PlayerNameText.text = AvatarAssetManager.Instance.inputField.text;
        //TypeOfFontSelectionPanel.SetActive(false);
        AvatarCustomizationPanel.SetActive(false);
        InputAvatarNamePanel.SetActive(false);
        ConfirmAvatarOverlayCanvas.SetActive(true);
     //   AudioManagerAvatarCreation.Instance.PlaySaveAvatarclip();
        if (AvatarAssetManager.Instance.chosenLetterFont == TypeOfChosenLetterFont.LOWERCASE)
            LetterTag.sprite = lowercaseTag;
        else
            LetterTag.sprite = uppercaseTag;
        Panj.SetActive(false);
    }

    public void GoToFontTypeSelection()
    {
        UIMapManager.OnWindowClosed?.Invoke(InputAvatarNamePanel.GetComponent<UIWindow>());
     //   AudioManagerAvatarCreation.Instance.PlayChooseLetterTypeSound();
        AvatarNameOnLog.text = AvatarAssetManager.Instance.inputField.text;
        //TypeOfFontSelectionPanel.SetActive(true);
        AvatarCustomizationPanel.SetActive(false);
        InputAvatarNamePanel.SetActive(false);
    }

    public void GoToInputNamePanel()
    {
        //ovde moramo proveriti da li su selektovani : skin color i odeca
        if (AvatarAssetManager.Instance.CheckIfValuesAreChosen())
        {
           // AudioManagerAvatarCreation.Instance.PlayChooseANameClip();
            UIMapManager.OnWindowClosed?.Invoke(AvatarCustomizationPanel.GetComponent<UIWindow>());
            //UIMapManager.OnWindowClosed?.Invoke(TypeOfFontSelectionPanel.GetComponent<UIWindow>());
            AvatarCustomizationPanel.SetActive(false);
            InputAvatarNamePanel.SetActive(true);
            //TypeOfFontSelectionPanel.SetActive(false);
        }
        
    }

    void SetButtonSprites(TypeOfChosenLetterFont chosenFont)
    {
        ConfirmFontTypeButton.gameObject.SetActive(true);
        if(chosenFont == TypeOfChosenLetterFont.UPPERCASE)
        {
            UppercaseButton.sprite = SelectedButtonBackground;
            LowercaseButton.sprite = UnSelectedButtonBackground;
        }
        else
        {
            UppercaseButton.sprite = UnSelectedButtonBackground;
            LowercaseButton.sprite = SelectedButtonBackground;
        }
    }

}
