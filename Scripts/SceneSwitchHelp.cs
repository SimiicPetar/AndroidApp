using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchHelp : MonoBehaviour
{
    // Start is called before the first frame update
    public static void OpenBlendingLetterGame()
    {
        SceneManager.LoadScene("BlendingGame");
    }

    public static void OpenLetterMachineGame()
    {
        SceneManager.LoadScene("LalaLetterMachine");
    }

    public static void OpenLetterTracingGame()
    {
        SceneManager.LoadScene("LetterTracingScene");
    }

    public static void OpenConsolidationGame()
    {
        SceneManager.LoadScene("ConsolidationGame");
    }
}
