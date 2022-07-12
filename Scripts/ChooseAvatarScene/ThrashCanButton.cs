using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrashCanButton : MonoBehaviour
{
    static ThrashCanButton _instance = null;
    public static ThrashCanButton Instance { get { return _instance; } }
    public Color ClickedColor;
    public bool canClicked = false;
    SpriteRenderer renderer;
    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        int i = 1;
        renderer = GetComponent<SpriteRenderer>();
        Debug.Log($"vrednost i : {i++ }");
        Debug.Log($"vrednost i nakon : {i}");
    }

    public delegate void ThrashCanClicked();
    public ThrashCanClicked onTrashCanClicked;

    public delegate void DontWantToDelete();
    public  DontWantToDelete onClickedOnBackground;

    public GameObject DeleteAvatarPopup;
    // Start is called before the first frame update

 
    public void OnCanClicked()
    {
        canClicked = true;

        onTrashCanClicked.Invoke();
    }

    public void ResetCan()
    {
        canClicked = false;
    }

    //ovde treba logika kad kliknes na kantu da ti se isto ugase x-evi
    //ukoliko je kanta vec kliknuta naravno
    private void Update()
    {
        if (canClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
;
                if (!IsPointerOverUIObject())
                {
                    onClickedOnBackground.Invoke();
                    canClicked = false;
                }

            }
        }
    }


    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
