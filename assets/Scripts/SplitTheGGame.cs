using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SplitTheGGame : MonoBehaviour
{
    [Header("UI Referencer")]
    public RectTransform line;
    public RectTransform perfectZone;
    public TextMeshProUGUI resultText;
    public Button lukButton;
    public Image lineImage;

    [Header("Indstillinger")]
    public float speed = 180f;

    float minY;
    float maxY;
    float direction = 1f;
    bool isRunning = false;
    bool hasResult = false;

    void OnEnable()
    {
        // Nulstil hver gang panelet åbnes
        hasResult = false;
        isRunning = true;
        resultText.text = "Tryk for at stoppe!";
        resultText.color = Color.white;
        lukButton.gameObject.SetActive(false);
        lineImage.color = Color.white;

        // Sæt grænser baseret på glasset
        maxY =  120f;
        minY = -120f;
        line.anchoredPosition = new Vector2(0, 0);
        direction = 1f;

        // Gør linjen hurtigere jo mere fuld man er
        speed = 180f + GameManager.Instance.drunkLevel * 1.5f;
    }

    void Update()
    {
        if (!isRunning) return;

        // Bevæg linjen op og ned
        float y = line.anchoredPosition.y + direction * speed * Time.deltaTime;

        if (y >= maxY) { y = maxY; direction = -1f; }
        if (y <= minY) { y = minY; direction =  1f; }

        line.anchoredPosition = new Vector2(0, y);

        // Farv linjen baseret på position
        float pzMin = perfectZone.anchoredPosition.y - perfectZone.rect.height / 2f;
        float pzMax = perfectZone.anchoredPosition.y + perfectZone.rect.height / 2f;

        if (y >= pzMin && y <= pzMax)
            lineImage.color = new Color(0.11f, 0.62f, 0.46f); // grøn
        else if (y >= pzMin - 35f && y <= pzMax + 35f)
            lineImage.color = new Color(0.94f, 0.62f, 0.15f); // gul
        else
            lineImage.color = new Color(0.89f, 0.29f, 0.29f); // rød

        // Klik for at stoppe
        if (Input.GetMouseButtonDown(0) && !hasResult)
            Stop(y);

            if (Input.GetMouseButtonDown(0))
{
    Debug.Log("Klik registreret");
}
    }

    void Stop(float y)
    {
        isRunning = false;
        hasResult = true;

        float pzMin = perfectZone.anchoredPosition.y - perfectZone.rect.height / 2f;
        float pzMax = perfectZone.anchoredPosition.y + perfectZone.rect.height / 2f;

        if (y >= pzMin && y <= pzMax)
        {
            // Perfekt
            resultText.text = "godt splittet g";
            resultText.color = new Color(0.11f, 0.62f, 0.46f);
            GameManager.Instance.ApplyResult(20, -60, 20);
        }
        else if (y >= pzMin - 35f && y <= pzMax + 35f)
        {
            // Okay
            resultText.text = "Næsten... 😅";
            resultText.color = new Color(0.94f, 0.62f, 0.15f);
            GameManager.Instance.ApplyResult(5, -60, 20);
        }
        else
        {
            // Dårligt
            resultText.text = "Spildt! Det er pinligt 😬";
            resultText.color = new Color(0.89f, 0.29f, 0.29f);
            GameManager.Instance.ApplyResult(-20, -60, 20);
        }

        UIManager.Instance.AdvanceTime(30f);
        lukButton.gameObject.SetActive(true);
    }

    public void LukPanel()
    {
        gameObject.SetActive(false);
    }
}