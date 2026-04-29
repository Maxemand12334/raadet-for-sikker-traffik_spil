using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD tekster")]
    public TextMeshProUGUI auraText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    [Header("Drunk indikator")]
    public GameObject drunkContainer;
    public Image drunkFill;

    [Header("Fade")]
    public Image fadeOverlay;

    [Header("Lyd")]
    public AudioSource audioSource;
    public AudioClip crashSound;

    float currentHour = 21f;

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    bool inBodega = scene.name == "BodegaScene";
    bool inTransport = scene.name == "TransportScene";

    if (timeText != null)
        timeText.gameObject.SetActive(inBodega);

    if (drunkContainer != null)
        drunkContainer.SetActive(inBodega || inTransport);

    // Kun fade ind på hospitalsscenen
    if (scene.name == "HospitalScene")
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    else
    {
        // Alle andre scener — ingen fade, bare sørg for overlay er væk
        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
    }
}

    void Update()
    {
        if (GameManager.Instance == null) return;

        auraText.text  = "Aura: " + GameManager.Instance.auraPoints;
        moneyText.text = "Kr: "   + GameManager.Instance.money;

        if (timeText != null && timeText.gameObject.activeSelf)
            timeText.text = GetTimeString();

        if (drunkFill != null)
            drunkFill.fillAmount = GameManager.Instance.drunkLevel / 100f;
    }

    public void StartBarNight()
    {
        currentHour = 21f;
        if (timeText != null)
            timeText.gameObject.SetActive(true);
        if (drunkContainer != null)
            drunkContainer.SetActive(true);
    }

    public void AdvanceTime(float minutes)
    {
        currentHour += minutes / 60f;
        Debug.Log("Tid nu: " + GetTimeString());

        if (currentHour >= 24f)
            BarCloses();
    }

    void BarCloses()
    {
        currentHour = 21f;
        SceneManager.LoadScene("TransportScene");
    }

    // Kaldes fra TransportOption når man vælger cykel og er fuld
    public void CrashSequence()
    {
        StartCoroutine(DoCrashSequence());
    }

IEnumerator DoCrashSequence()
{
    // Fade til sort over 1 sekund
    yield return StartCoroutine(FadeOut(1f));

    // Spil lyd når det er helt sort
    if (crashSound != null && audioSource != null)
    {
        audioSource.clip = crashSound;
        audioSource.Play();
    }

    // Bliv sort i 4 sekunder mens lyden spiller
    yield return new WaitForSeconds(4f);

    // Load hospital — FadeIn coroutine klarer faden ind
    SceneManager.LoadScene("Hospital");
}

IEnumerator FadeOut(float duration)
{
    if (fadeOverlay == null) yield break;

    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        fadeOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, t));
        yield return null;
    }
    fadeOverlay.color = new Color(0f, 0f, 0f, 1f);
}

IEnumerator FadeIn()
{
    if (fadeOverlay == null) yield break;

    fadeOverlay.color = new Color(0f, 0f, 0f, 1f);
    yield return new WaitForSeconds(0.1f);

    // Langsom fade ind — 3 sekunder
    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime / 3f;
        fadeOverlay.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, t));
        yield return null;
    }
    fadeOverlay.color = new Color(0f, 0f, 0f, 0f);
}

    string GetTimeString()
    {
        int h = (int)currentHour % 24;
        int m = Mathf.RoundToInt((currentHour % 1) * 60);
        return $"{h:00}:{m:00}";
    }
}