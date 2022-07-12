using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingBrush : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer brushTip;

    public Vector3 currentPosition;

    public SpriteRenderer brush;

    private void Update()
    {
        currentPosition = transform.position;
    }
    void Start()
    {
        brushTip = GetComponent<SpriteRenderer>();
        Color c = Paintable.Instance.CurrentColor1;
        brush.color = new Color(c.r, c.g, c.b, 1f);
    }

    public void Initialize(Color color)
    {
        brush.color = color;
    }

    public void StartNextTrail()
    {
        //brushTip.enabled = false;
       // GetComponent<Collider>().enabled = false;
    }

    private void FixedUpdate()
    {
        
    }
}
