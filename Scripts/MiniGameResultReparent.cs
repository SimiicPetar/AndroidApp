using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniGameResultReparent : MonoBehaviour
{
    public Transform gameResultProgressBarPosition;
    public GameObject progressBarPrefab;
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    private void Start()
    {
        sprites = progressBarPrefab.GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    private void OnEnable()
    {
        progressBarPrefab.transform.parent = gameResultProgressBarPosition;
        progressBarPrefab.transform.position = gameResultProgressBarPosition.transform.position;

        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].sortingOrder = 100;
        }
    }
}
