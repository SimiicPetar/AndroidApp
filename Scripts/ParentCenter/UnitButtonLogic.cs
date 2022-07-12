using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonLogic : MonoBehaviour
{
    public string UnitID;
    public Sprite SelectedSprite;
    public Sprite DisabledSprite;
    public delegate void OnUnitTabClicked(string unitID, UnitButtonLogic button);
    public static OnUnitTabClicked onUnitTabClicked;

    Button UnitButton;
    Image image;

    void SetInteractable()
    {
        UnitButton.interactable = UnitStatisticsBase.Instance.CheckIfUnitIsUnlocked(UnitID);
    }

    private void Start()
    {
        UnitButton = GetComponent<Button>();
        image = GetComponent<Image>();
        SetInteractable();
       // UnitButton.onClick.AddListener(() => UnitTabClicked());
    }



    public void UnitTabClicked()
    {
        onUnitTabClicked.Invoke(UnitID, this);
        image.sprite = SelectedSprite;
    }
}
