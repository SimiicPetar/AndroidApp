using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UnlockableUnitGame : MonoBehaviour
{
    Color interactableColor = Color.green;
    Color uninteracltableColor = Color.red;
    // Start is called before the first frame update
    Button button;
    TextMeshProUGUI gameText;
    public BasicGames gameCode;

    public Image Katanac;
    public GameObject DoneIndicator;

    UnitStatisticsBase unitStatisticsBase;
    UnitProgressManager unitProgressManager;
    bool currentInteractionStatus = false;



    private void Awake()
    {
        unitStatisticsBase = UnitStatisticsBase.Instance;
        
    }

    private void OnEnable()
    {
        //UIMapManager.OnWindowClosed += Reset;
       
    }

    public void StartClickBanWhenEnteringUnit()
    {
        StartCoroutine(SmallClickBanWhenEnabled(0.4f));
    }

    //kad se otvara prozor da ne kliknemo slucajno
    IEnumerator SmallClickBanWhenEnabled(float duration)
    {
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(duration);
        GetComponent<Button>().interactable = currentInteractionStatus;
    }

    private void OnDisable()
    {
        // UIMapManager.OnWindowClosed -= Reset;
        StopAllCoroutines();
    }

    private void Start()
    {
        
        unitProgressManager = UnitProgressManager.Instance;
        button = GetComponent<Button>();
        gameText = GetComponent<TextMeshProUGUI>();
        if (gameText != null)
            gameText.color = uninteracltableColor;
    }


    public void Reset()
    {
       
        Katanac.gameObject.SetActive(true);
        DoneIndicator.SetActive(false);
        GetComponent<Button>().interactable = false;
    }

    public void SetButtonInteractable(bool interact)
    {
        Reset();
        unitProgressManager = UnitProgressManager.Instance;
        button = GetComponent<Button>();
        gameText = GetComponent<TextMeshProUGUI>();
        if (gameCode == BasicGames.ANTEATER)
        {
            if (interact && UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.CAPIBARA))
            {
                Katanac.gameObject.SetActive(false);
                
                button.interactable = true;
                if(gameText != null)
                    gameText.color = Color.green;
                if (UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.ANTEATER))
                {
                    DoneIndicator.SetActive(true);
                }
            }
            else
                button.interactable = false;
        }else if(gameCode == BasicGames.CAPIBARA)
        {

            button.interactable = interact;
            if (interact)
            {
                Katanac.gameObject.SetActive(false);
                if (gameText != null)
                    gameText.color = interactableColor;
                if (UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.CAPIBARA))
                {
                    DoneIndicator.SetActive(true);
                }
            }
            else
            {
                if (gameText != null)
                    gameText.color = uninteracltableColor;
            }
                
        }else if (gameCode == BasicGames.BLENDING)
        {
            button.interactable = true;
            if(UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.CAPIBARA) && UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.ANTEATER))
            {
                Katanac.gameObject.SetActive(false);
                if (unitProgressManager.CheckIfUnclockableGameIsFinished(BasicGames.BLENDING))
                {
                    DoneIndicator.SetActive(true);
                }
            }
            else
            {
                button.interactable = false;
            }
        }
        if (gameText != null)
            gameText.alpha = 0;
        currentInteractionStatus = button.interactable;
    }

    public void GoToMiniGame()
    {
        if(gameCode == BasicGames.CAPIBARA)
        {
            SceneManager.LoadScene("KapibaraScena");
        }else if(gameCode == BasicGames.ANTEATER)
        {
            SceneManager.LoadScene("AntEaterGame");
        }else if(gameCode == BasicGames.BLENDING)
        {
            SceneManager.LoadScene("BlendingGame");
        }
    }
}
