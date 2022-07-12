using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XButtonLogic : MonoBehaviour
{
    

    public void OnClickXButton()
    {
        ThrashCanButton.Instance.DeleteAvatarPopup.SetActive(true);
        ThrashCanButton.Instance.DeleteAvatarPopup.GetComponent<DeleteAvatarPopup>().Init(GetComponentInParent<AvatarSlotLogic>().SlotID);
    }
}
