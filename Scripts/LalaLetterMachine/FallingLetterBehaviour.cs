using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingLetterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float destroyPositionOffset = 0.3f;
    float lerpDuration = 0.9f;
    float FINALLERPDURATION = 1.5f; 
    bool thisIsFinalLetter = false;
    char letter;
    LetterMachineAudioManager audioManager;

    private void Awake()
    {
        Debug.Log($"instanciran sam na poziciju :{transform.position}");
    }
    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < EndPosition.y - destroyPositionOffset && !thisIsFinalLetter)
            Destroy(gameObject);
            
    }
    private void Start()
    {
        audioManager = LetterMachineAudioManager.Instance;
    }

    public void Init(Vector3 endPos, Vector3 startingPos, Sprite letterSprite,char Letter = '\0', bool final = false, string specificWord = null)
    {
        letter = Letter;
        EndPosition = endPos;
        transform.position = startingPos;
        transform.DOMove(final ? transform.parent.position :endPos - new Vector3(0, 1f, 0), final ? FINALLERPDURATION : lerpDuration, false);
        if (final)
            thisIsFinalLetter = true;
        else
            thisIsFinalLetter = false;
        GetComponent<SpriteRenderer>().sprite = letterSprite;
        if (final)
        {
           /* if(specificWord != null)
            {
                StartCoroutine(PlayLetterSound(specificWord));
            }
            else
            {
                StartCoroutine(PlayLetterSound(specificWord));
            }*/
            
        }
            
    }
    IEnumerator PlayLetterSound(string word)
    {
        yield return new WaitForSeconds(1f);
        LetterMachineAudioManager.Instance.PlayLetterSound(letter, word);
    }
    
   
}
