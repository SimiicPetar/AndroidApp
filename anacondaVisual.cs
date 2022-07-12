using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anacondaVisual : MonoBehaviour
{
    // Start is called before the first frame update

    public AnacondaBehaviourTest behTest;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowHead()
    {
        Debug.Log(gameObject.name);
        behTest.EnableSnakeHead(true);
    }
    
}
