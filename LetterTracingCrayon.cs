using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTracingCrayon : MonoBehaviour
{
    public Color CrayonColor;
    Animator CrayonAnimator;
    // Start is called before the first frame update
    bool isPressed = false;
    Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    public void ChangeDrawColor()
    {
        if (!Paintable.Instance.crayonClickBan)
        {
            Paintable.Instance.ChangeDrawPrefabColor(CrayonColor, this);
            Paintable.Instance.crayonClickBan = true;
            transform.DOMoveX(startPosition.x + LetterTracingManager.Instance.FixedDistance, 0.5f).OnComplete(() => { Paintable.Instance.crayonClickBan = false; });
            isPressed = true;
        }
      

    }

    public void BackToOrigin()
    {
        if(isPressed || transform.position.x != startPosition.x)
            transform.DOMoveX(startPosition.x, 0.2f);
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
