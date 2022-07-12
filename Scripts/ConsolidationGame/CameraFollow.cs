using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 offset;
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    ConsolidationGameManagerNew gameManager;

    bool activateFollow = false;
    public Transform lalaTransform;


    private void Start()
    {
        gameManager = ConsolidationGameManagerNew.Instance;
        gameManager.onPlayerInstantiated += ActivateFollow;
    }

    void ActivateFollow()
    {
        activateFollow = true;
        target = FindObjectOfType<PlayerBehaviour>().transform;
    }
    //Vector3(54.2475052,175,19.4630737)
    void Update()
    {
        if (transform.position.y <= 2.7f)
            lalaTransform.parent = transform;
        if (activateFollow)
        {
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10) + offset);
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3 (transform.position.x, target.position.y - 2.5f, transform.position.z), ref velocity, smoothTime);

        }

    }

}
