using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public int PlayerX;
    public int PlayerY;

    readonly string JumpTrigger = "Jump";
    readonly string LandingTrigger = "Land";
    Vector3 startingScale;

    Animator PlayerAnimator;
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    public void Jump()
    {
        PlayerAnimator.SetTrigger("Jump");
    }

    public void Land()
    {
        PlayerAnimator.SetTrigger(LandingTrigger);
    }
}
