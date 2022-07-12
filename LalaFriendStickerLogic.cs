using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LalaFriendStickerLogic : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Sprite Stickersprite;
    Vector3 StickingSlotLocation;
    Vector3 StartingPosition;
    public AudioClip CardFitnInSound;
    bool dragable = false;

    public void Init(Sprite sticker,Vector3 EmptyStickerPosition,bool owned = false)
    {
        
            StartingPosition = transform.position;
            GetComponent<Image>().sprite = sticker;
            GetComponent<Image>().color = Color.white;
            StickingSlotLocation = EmptyStickerPosition;
            dragable = true;   
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragable)
        {
            var temp = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position = new Vector3(temp.x, temp.y, 0f);
        }
    }

    IEnumerator WaitForFitSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AudioManagerMap.Instance.PlayBinSound();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragable)
        {
            if (Vector2.Distance(transform.position, StickingSlotLocation) > 20f)
            {
                transform.position = StartingPosition;
            }
            else
            {
                transform.position = StickingSlotLocation;
                DragableCard.onCardPlacedInSlot?.Invoke(); // dozvoljava povratak na mapu
                GetComponent<Animator>().SetTrigger("GrowUp");
                AudioManagerMap.Instance.PlaySound(CardFitnInSound);
                StartCoroutine(WaitForFitSound(CardFitnInSound.length));
                dragable = false;
                //UIMapManager.Instance.CloseStickerAlbumWindow();
            }
        }
    }
}
