using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum WinLoseState
    {
        None,
        Win,
        Lose,
        Draw
    }
    public Button rollDiceBtn;
    public Button tryAgainBtn;

    [SerializeField]
    GameObject wonTxtObj;
    [SerializeField]
    GameObject loseTxtObj;
    [SerializeField]
    GameObject drawTxtObj;

    [SerializeField]
    GameObject gameOverScreen;

    int rollState = 0;
    int resultNumberTotal = 0;

    [SerializeField]
    WinLoseState crntWnLsState = WinLoseState.None;

    [SerializeField]
    Dice dceScrpt1;
    [SerializeField]
    Dice dceScrpt2;
    // Start is called before the first frame update
    void Awake()
    {
        rollDiceBtn.onClick.AddListener(RollTheDice);
        tryAgainBtn.onClick.AddListener(RestartLevel);

        DisableObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (rollState == 1)
        {
            if (dceScrpt1.resultNumberReceived && dceScrpt2.resultNumberReceived)
            {
                CheckWinLoseState();
                DisplayTheResponse();
                rollState = 0;
            }
        }
    }
    void DisableObjects()
    {
        wonTxtObj.SetActive(false);
        loseTxtObj.SetActive(false);
        drawTxtObj.SetActive(false);
    }
    void DisplayTheResponse()
    {
        DisableObjects();

        if (crntWnLsState == WinLoseState.Win)
        {
            wonTxtObj.SetActive(true);
        }
        else
            if (crntWnLsState == WinLoseState.Lose)
        {
            loseTxtObj.SetActive(true);
        }
        else
            if (crntWnLsState == WinLoseState.Draw)
        {
            drawTxtObj.SetActive(true);
        }
    }
    void CheckWinLoseState()
    {
        resultNumberTotal = dceScrpt1.resultNum + dceScrpt2.resultNum;

        crntWnLsState = WinLoseState.None;

        if (resultNumberTotal == 7 || resultNumberTotal == 11)
        {
            crntWnLsState = WinLoseState.Win;
        }
        else
            if (resultNumberTotal == 2 || resultNumberTotal == 3 || resultNumberTotal == 12)
        {
            crntWnLsState = WinLoseState.Lose;
            StartCoroutine(ShowTryAgain());
        }
        else
        {
            crntWnLsState = WinLoseState.Draw;
            StartCoroutine(RollAgain());
        }        
    }
    void RollTheDice()
    {
        if(!dceScrpt1.IsItRolling())
            dceScrpt1.StartRollingDice();

        if (!dceScrpt2.IsItRolling())
            dceScrpt2.StartRollingDice();

        rollDiceBtn.gameObject.SetActive(false);

        rollState = 1;

        DisableObjects();
    }
    void RestartLevel()
    {
        DisableObjects();

        rollDiceBtn.gameObject.SetActive(true);
        gameOverScreen.SetActive(false);        
    }
    IEnumerator ShowTryAgain()
    {
        yield return new WaitForSeconds(1.75f);

        gameOverScreen.SetActive(true);
    }
    IEnumerator RollAgain()
    {
        yield return new WaitForSeconds(1.75f);

        rollDiceBtn.gameObject.SetActive(true);
    }
    public static float GetDiff(float val1, float val2)
    {
        if (val1 > val2)
        {
            return val1 - val2;
        }
        return val2 - val1;
    }
}
