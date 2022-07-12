using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameResultWindow : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI zvezdiceText;

    const int threeStarLimit = 12;
    const int twoStarLimit = 11;
    const int downLimit = 9;
    public List<GameObject> Zvezdice;
    public TextMeshProUGUI numberOfCorrectAnswersText;
    public TextMeshProUGUI numberOfWrongAnswersText;
    

    private void OnEnable()
    {
        zvezdiceText = GetComponentInChildren<TextMeshProUGUI>();
    }


    //12 8 4 0

    public void SetZvezdicaTextNew(int score,  int numberOfWrongAnswers, GameScoring scoring = null, bool repeat = false)
    {
        var audioManager = FindObjectOfType<AudioManagerBase>();
         
        numberOfCorrectAnswersText.text = score.ToString(); // change int to string
        numberOfWrongAnswersText.text = numberOfWrongAnswers.ToString(); // change int to string
            int brojZaAktiviranje = 0;
            if (score == scoring.fourStarLimit)
				
            { // When player wins four stars in this game. This script play this sound from this code.
                audioManager.PlaySound(audioManager.soundPack.fourStarSound);
                brojZaAktiviranje = 4;
                
            }    
            else if (score < scoring.fourStarLimit && score >= scoring.threeStarLimit)
            { // When player wins three stars in this game. This script play this sound from this code.
                audioManager.PlaySound(audioManager.soundPack.threeAndTwoStarSound);
                brojZaAktiviranje = 3;
            }
            else if (score > scoring.twoStarLimit && score < scoring.threeStarLimit)
            { // When player wins two stars in this game. This script play this sound from this code.
                audioManager.PlaySound(audioManager.soundPack.threeAndTwoStarSound);
                brojZaAktiviranje = 2;
            }
            else 
            { // When player wins one star in this game. This script play this sound from this code.
                audioManager.PlaySound(audioManager.soundPack.oneStarSound);
                brojZaAktiviranje = 1;
            }

            if (score == 0)
                brojZaAktiviranje = 0;
            for(int i = 0; i < brojZaAktiviranje; i++)
            {
                Zvezdice[i].SetActive(true);
            }
        
        
    }
}
