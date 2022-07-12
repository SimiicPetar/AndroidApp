using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerAlbumButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("kliknuto je na dugme za sticker album");
            StickerAlbumManager.Instance.FillUpAlbum();
        }
            
    }
}
