using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkinColorSliderHandleLogic : MonoBehaviour
{
    public void OnBeginDrag()
    {
        transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f);
    }


    public void OnEndDrag()
    {
        transform.DOScale(Vector3.one, 0.2f);
    }

}
