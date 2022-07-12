using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldLogic : MonoBehaviour
{
    TMP_InputField inputField;
    public Vector3 startingPositionOfThePanel;
    public Vector3 endPositionOfThePanel;


    private void Awake()
    {
        
        inputField = gameObject.GetComponent<TMP_InputField>();
      // startingPositionOfThePanel = transform.parent.position;
       inputField.onTouchScreenKeyboardStatusChanged.AddListener(ReportChangeStatus);
        inputField.onSelect.AddListener(OnSelectAction);

    }

    void OnSelectAction(string action)
    {
        //if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        transform.parent.localPosition = endPositionOfThePanel;
    }
    private void ReportChangeStatus(TouchScreenKeyboard.Status keyboardStatus)
    {
        Debug.Log($"Event je okinut, keyboard status je : {keyboardStatus}");
        if(keyboardStatus == TouchScreenKeyboard.Status.Visible)
        {
            //pozovi metodu za podizanje panela
            transform.parent.localPosition = endPositionOfThePanel;
        }else if( keyboardStatus == TouchScreenKeyboard.Status.Canceled || keyboardStatus == TouchScreenKeyboard.Status.LostFocus || keyboardStatus == TouchScreenKeyboard.Status.Done)
        {
            transform.parent.localPosition = startingPositionOfThePanel;
        }
    }
}
