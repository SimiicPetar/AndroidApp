using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAvatarPopup : MonoBehaviour
{
    int slotID;
    public delegate void ButtonClicked();
    public static ButtonClicked OnButtonClicked;
    public void Init(int id)
    {
        slotID = id;
    }

    private void OnEnable()
    {
        OnButtonClicked += ThrashCanButton.Instance.ResetCan;
        ThrashCanButton.Instance.canClicked = true;
        ThrashCanButton.Instance.onClickedOnBackground.Invoke();
    }

    private void OnDisable()
    {
        OnButtonClicked -= ThrashCanButton.Instance.ResetCan;
        ThrashCanButton.Instance.canClicked = false;
    }
    // Start is called before the first frame update
    public void OnYesClicked()
    {
        AvatarBase.Instance.RemoveAvatar(slotID);
        gameObject.SetActive(false);
        OnButtonClicked?.Invoke();
    }

    public void OnNoClicked()
    {
        OnButtonClicked?.Invoke();
        gameObject.SetActive(false);

    }
}
