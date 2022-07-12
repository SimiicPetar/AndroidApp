using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderBackgroundLogic : MonoBehaviour, IPointerClickHandler
{
    public Slider colorSlider;

    public void OnPointerClick(PointerEventData ed)
    {
        Vector3 localHit = transform.InverseTransformPoint(ed.pressPosition);
        colorSlider.normalizedValue = localHit.x / GetComponent<RectTransform>().rect.width;
    }

}
