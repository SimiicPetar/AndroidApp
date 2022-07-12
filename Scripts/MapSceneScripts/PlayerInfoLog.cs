using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoLog : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI PlayerNameText;
    public Image LetterBadgeImage;
    public Sprite LowercaseSprite;
    public Sprite UppercaseSprite;
    void Start()
    {
        PlayerNameText.text = AvatarBase.AvatarSpineDictionary[AvatarBase.Instance.ActiveAvatarKey].AvatarName.ToUpper();
        if (AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE)
            LetterBadgeImage.sprite = UppercaseSprite;
        else
            LetterBadgeImage.sprite = LowercaseSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
