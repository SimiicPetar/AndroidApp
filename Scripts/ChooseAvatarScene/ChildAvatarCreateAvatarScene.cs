using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChildAvatarCreateAvatarScene : MonoBehaviour
{
    public GameObject HairstylesParent;
    public GameObject OutfitsParent;
    public SpriteRenderer AvatarBody;

    AvatarInfo info;

    public void DressUp(AvatarInfo loadedStruct)
    {
        info = loadedStruct;
        if(loadedStruct.outfitIndex != -1)
            OutfitsParent.transform.GetChild(loadedStruct.outfitIndex).gameObject.SetActive(true);
        if(loadedStruct.hairstyleIndex != -1)
            HairstylesParent.transform.GetChild(loadedStruct.hairstyleIndex).gameObject.SetActive(true);
        AvatarBody.color = loadedStruct.skinColor;

        if(loadedStruct.hairstyleIndex != -1)
            HairstylesParent.transform.GetChild(loadedStruct.hairstyleIndex).GetComponent<SpriteRenderer>().color = loadedStruct.hairColor;

    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponentInParent<AvatarSlotLogic>().ChooseAvatar();
            SceneManager.LoadScene("MapScene");
        }
    }
}
