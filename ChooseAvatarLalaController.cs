using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseAvatarLalaController : MonoBehaviour
{
    // Start is called before the first frame update

   public static  bool interactable = false;

    public bool Interactable { get => interactable; set => interactable = value; }

    void Start()
    {
        
    }


    private void OnMouseOver()
    {
        if(interactable && Input.GetMouseButtonDown(0))
        {
            interactable = false;
            AudioManagerPlayerSelection.Instance.RepeatLalaInstruction();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
