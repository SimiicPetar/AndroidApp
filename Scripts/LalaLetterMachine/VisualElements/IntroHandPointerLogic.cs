using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHandPointerLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public delegate void IntroDone();
    public IntroDone onIntroDone;
    LetterTracingManager gameManager;
    void Start()
    {
        gameManager = LetterTracingManager.Instance;
        onIntroDone += gameManager.AllowDrawing;
    }



    public void AnimationIsOver()
    {
        onIntroDone.Invoke();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
