using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarSpineDressUp : MonoBehaviour
{
    public int nesto;
    public Skeleton skeleton;

    public delegate void OnActiveCharacterChanged();
    public static OnActiveCharacterChanged onActiveCharacterChanged;

    Animator _animator;

    void Awake()
    {   if(skeleton == null)
            skeleton = GetComponent<SkeletonRenderer>().skeleton;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }
    // Start is called before the first frame update


    private void Start()// start AnimatorDesync
    {
        if (skeleton == null)
            skeleton = GetComponent<SkeletonRenderer>().skeleton;
        StartCoroutine(AnimatorDesync());
        
    }

    IEnumerator AnimatorDesync()// delay _animator.enabled = true;
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.8f));
        _animator.enabled = true;
    }
    public void Dressup(AvatarSpineInfo info)//Dressup avatar gamelogic 
    {
        if (skeleton == null)
            skeleton = GetComponent<SkeletonRenderer>().skeleton;

        foreach (var slot in info.spineAvatarInfo)
        {
            if (slot.Value.attachmentName != "" && slot.Value.attachmentName != "")
            {    
                skeleton.SetAttachment(slot.Key, slot.Value.attachmentName);        

                skeleton.FindSlot(slot.Key).SetColor(slot.Value.attachmentColor);
            }
           
        }
    }

    void OnMouseOver()// onclick load scene mapscene 
    {
        if (Input.GetMouseButtonDown(0) && SceneManager.GetActiveScene().name.Equals("ChooseAvatarScene") && !ThrashCanButton.Instance.canClicked)
        {
            GetComponentInParent<AvatarSlotLogic>().ChooseAvatar();
            onActiveCharacterChanged?.Invoke();
            SceneManager.LoadScene("MapScene");
        }
    }
}
