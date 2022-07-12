using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite SoundOnSprite;
    public Sprite SoundOffSprite;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        { 
            GameManager.Instance.MuteBGMusic();
            ChangeImage();
        }
        );
        GetComponent<Image>().sprite = GameManager.Instance.MuteBGMusicBool ? SoundOffSprite : SoundOnSprite;
    }

    void ChangeImage()
    {
        GetComponent<Image>().sprite = GameManager.Instance.MuteBGMusicBool ? SoundOffSprite : SoundOnSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
