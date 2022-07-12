using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paintable : MonoBehaviour
{

    float zValue = -1f;

    public GameObject TrailPrefab;

    GameObject trail;

    List<GameObject> trails;

    public bool IsDrawingDone = false;

    public List<Collider2D> letterPartColliders;

    public List<CheckpointLogic> letterCheckPoints;

    public List<CheckpointParent> letterCheckPointParents;

    public GameObject InstructionHandPointer;

    GameObject nextCheckPoint;

    static Paintable _instance = null;

    public static Paintable Instance { get { return _instance; } }

    public float ZValue { get => zValue; set => zValue = value; }
    public float StartingZValue { get => startingZValue; set => startingZValue = value; }
    public Collider2D ActivePath { get => activePath; set => activePath = value; }
    public int PathIndex { get => pathIndex; set => pathIndex = value; }
    public Color CurrentColor1 { get => CurrentColor; set => CurrentColor = value; }

    LetterTracingManager gameManager;

    float startingZValue;

    public char letter;

    Task ShowingHelpHandPrompt;

    bool skipCheckPointReached = false;

    private Vector3 lastMousePos;

    public List<Collider2D> Paths;

    public List<GameObject> Covers;

    Collider2D activePath;

    int pathIndex = 0;

    const float dist = 24f;

    public bool startedToDraw = false;

    public float safetyOffset = 25f;

    public float safetyOffset2 = 1f;

    bool instantiated = false;

    private Color CurrentColor = default;

    public float startCoef = 0.00000001f;

    LetterTracingCrayon activeCrayon;

    float initialzvalue;

    public bool crayonClickBan = false;
    

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        letterCheckPoints = GetComponentsInChildren<CheckpointLogic>().ToList();
        letterCheckPointParents = GetComponentsInChildren<CheckpointParent>().ToList();
        initialzvalue = zValue;

        //transform.GetChild(1).position = transform.GetChild(0).position;
    }


    public void ActivateNextPath(CheckpointLogic checkpoint)
    {
    
    }

    public void ChangeDrawPrefabColor(Color newColor, LetterTracingCrayon crayon)
    {

            if (activeCrayon != null)
            {
            //  foreach (var crayonn in LetterTracingManager.Instance.allCrayons)
            // crayonn.BackToOrigin();
            activeCrayon.BackToOrigin();
            }

            CurrentColor1 = newColor;
            activeCrayon = crayon;
            zValue -= 0.001f;
            
     
       
    }

    IEnumerator ShowHelpingPrompt()
    {
        yield return new WaitForSeconds(6f);
        //InstructionHandPointer.SetActive(true);
    }

    public void ShowHelpPrompt()
    {
        ShowingHelpHandPrompt = new Task(ShowHelpingPrompt());
    }

    void HideHelpingPrompt()
    {
        if (ShowingHelpHandPrompt != null)
        {
          //  InstructionHandPointer.SetActive(false);
            //ShowingHelpHandPrompt.Stop();
        }

    }

    private void Start()
    {
        pathIndex = 0;
        if(Paths.Count > 0)
            ActivePath = Paths[0];
        StartingZValue = ZValue;
        gameManager = LetterTracingManager.Instance;
        trails = new List<GameObject>();
        LetterTracingAudioManager.Instance.introEnded += ShowHelpPrompt;
        //if (AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE)
        //    transform.localScale = new Vector3(0.9f, 0.9f);
        Debug.Log("poceli smo");
    }


    

    public void DestroyTrail()
    {
        startCoef = 0.00000001f;
        if (trails != null &&  trails.Count > 0)
        {
            foreach (var trail in trails)
                Destroy(trail);
            trails.Clear();
        }
        zValue = initialzvalue;
    }


    public void DrawingDone()
    {
        StartCoroutine(DrawingIsDone());
    }

    IEnumerator DrawingIsDone()
    {
        yield return new WaitWhile(() => Input.GetMouseButton(0));
        IsDrawingDone = true;
        gameManager.ShowTargetWordIllustration();
    }

    public void Restart()
    {
        IsDrawingDone = false;
    }

    public void SetZValue(float newValue)
    {
        zValue = newValue;
    }

    public bool CheckIfInBounds(Vector2 hitPoint)
    {
        foreach (var collider in letterPartColliders)
        {
            if (collider.ClosestPoint(hitPoint) == hitPoint)
                return true;
        }
        return false;
    }


    public void InstantiateLinePrefab()
    {
        var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        trail = Instantiate(TrailPrefab, null);
        var result = Physics.Raycast(Ray, out hit);
        trails.Add(trail);
        trail.name = "trail" + trails.Count;
        trails[trails.Count - 1].transform.position = new Vector3(hit.point.x, hit.point.y, zValue);
    }

    private void Update()
    {
       
            if (!IsDrawingDone)
            {
                instantiated = false;

                var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                var result1 = Physics.Raycast(Ray, out hit);
                Debug.DrawRay(
                      Camera.main.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.blue, 0.5f);
                if (Input.GetMouseButton(0))
                {
                    HideHelpingPrompt();

                    startedToDraw = true;

                    var result = Physics.Raycast(Ray, out hit);
                    Debug.DrawRay(
                        Camera.main.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.green, 0.5f);
                    if (result && (hit.collider.CompareTag("LetterCheckpoint")) && CurrentColor1 != default)
                    {
                            trail = Instantiate(TrailPrefab, null);
                            trails.Add(trail);
                            trail.name = "trail" + trails.Count;
                            trails[trails.Count - 1].transform.position = new Vector3(hit.point.x, hit.point.y, zValue);
                            if(trails.Count > 2)
                            {
                                trails[trails.Count - 1].transform.position = new Vector3(hit.point.x, hit.point.y, trails[trails.Count - 1].transform.position.z - 0.01f);
                            }
                           
                    }

                    TurnOffLineColliders();
                    if(trails != null )
                    {
                        if (trails.Count > 0) 
                        {
                            if (trails[trails.Count - 1].transform.position.x == 0f && trails[trails.Count - 1].transform.position.y == 0f)
                            {
                                Destroy(trails[trails.Count - 1]);
                                trails.RemoveAt(trails.Count - 1);
                            }
                          
                        }
                    }
                    

                }
            }

    }

    

    public void TurnOffLineColliders()
    {
        for (int i = 0; i < trails.Count - 1; i++)
        {
            trails[i].gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    bool CheckIfDragIsValid( Vector3 lastPosition, Vector3 mouseHitPos)
    {
        Vector3 nextCheckPointLocation = gameManager.NextCheckPoint.GetComponent<Collider>().bounds.center;
        Vector3 differenceVectLastPosition = nextCheckPointLocation - lastPosition;
        Vector3 differenceVectCurrentPosition = nextCheckPointLocation - mouseHitPos;
        if (Mathf.Abs(Vector3.Magnitude(differenceVectLastPosition)) < Mathf.Abs(Vector3.Magnitude(differenceVectCurrentPosition)))
            return true;
        else return false;
    }

}




