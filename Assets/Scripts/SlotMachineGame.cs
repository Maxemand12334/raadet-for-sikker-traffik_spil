using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SlotMachineGame : MonoBehaviour
{
    [Header("Hjul billeder")]
    public Image reel1;
    public Image reel2;
    public Image reel3;

    [Header("Symboler — sæt 4-5 sprites i Inspector")]
    public Sprite[] symbols;

    [Header("UI")]
    public TextMeshProUGUI resultText;
    public Button spinButton;
    public Button lukButton;

    // Hvad hvert hjul lander på
    int result1, result2, result3;
    bool isSpinning = false;

    void OnEnable()
    {
        // Nulstil hver gang panelet åbnes
        resultText.text = "Træk i armen!";
        resultText.color = Color.white;
        spinButton.gameObject.SetActive(true);
        lukButton.gameObject.SetActive(false);

        // Sæt første symbol på alle hjul
        if (symbols.Length > 0)
        {
            reel1.sprite = symbols[0];
            reel2.sprite = symbols[0];
            reel3.sprite = symbols[0];
        }
    }

    public void Spin()
    {
        if (isSpinning) return;
        spinButton.gameObject.SetActive(false);
        StartCoroutine(SpinReels());
    }

    IEnumerator SpinReels()
    {
        isSpinning = true;

        // Spin animation — skift symboler hurtigt i 1.5 sekunder
        float spinDuration = 1.5f;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            reel1.sprite = symbols[Random.Range(0, symbols.Length)];
            reel2.sprite = symbols[Random.Range(0, symbols.Length)];
            reel3.sprite = symbols[Random.Range(0, symbols.Length)];

            elapsed += 0.08f;
            yield return new WaitForSeconds(0.08f);
        }

        // Land på endelige resultater
        result1 = Random.Range(0, symbols.Length);
        result2 = Random.Range(0, symbols.Length);
        result3 = Random.Range(0, symbols.Length);

        // Hjul stopper et ad gangen med lille pause
        reel1.sprite = symbols[result1];
        yield return new WaitForSeconds(0.2f);
        reel2.sprite = symbols[result2];
        yield return new WaitForSeconds(0.2f);
        reel3.sprite = symbols[result3];
        yield return new WaitForSeconds(0.3f);

        isSpinning = false;
        ShowResult();
    }

    void ShowResult()
    {
        if (result1 == result2 && result2 == result3)
        {
            // 3 ens — stor gevinst
            resultText.text = "JACKPOT! 🎰\n+100 kr";
            resultText.color = new Color(0.11f, 0.62f, 0.46f);
            resultText.fontSize = 28;
            GameManager.Instance.ApplyResult(5, 100, 0);
        }
        else if (result1 == result2 || result2 == result3 || result1 == result3)
        {
            // 2 ens — lille gevinst
            resultText.text = "2 ens!\n+50 kr";
            resultText.color = new Color(0.94f, 0.62f, 0.15f);
            resultText.fontSize = 22;
            GameManager.Instance.ApplyResult(0, 50, 0);
        }
        else
        {
            // Ingen ens — tab
            resultText.text = "Ingen held...\n-20 kr";
            resultText.color = new Color(0.89f, 0.29f, 0.29f);
            resultText.fontSize = 18;
            GameManager.Instance.ApplyResult(0, -20, 0);
        }

        UIManager.Instance.AdvanceTime(15f);
        lukButton.gameObject.SetActive(true);
    }

    public void LukPanel()
    {
        gameObject.SetActive(false);
    }
}