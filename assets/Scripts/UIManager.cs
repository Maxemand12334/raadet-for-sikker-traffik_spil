using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD tekster")]
    public TextMeshProUGUI auraText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI timeText;

    float currentHour = 21f;

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Lyt på scene-skift
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Slå TimeText til/fra baseret på hvilken scene der loader
        bool inBodega = scene.name == "BodegaScene";
        if (timeText != null)
            timeText.gameObject.SetActive(inBodega);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        auraText.text  = "Aura: " + GameManager.Instance.auraPoints;
        moneyText.text = "Kr: "   + GameManager.Instance.money;

        // Opdater tid hvis TimeText er aktiv
        if (timeText != null && timeText.gameObject.activeSelf)
            timeText.text = GetTimeString();
    }

    public void StartBarNight()
    {
        currentHour = 21f;
        if (timeText != null)
            timeText.gameObject.SetActive(true);
    }

    public void AdvanceTime(float minutes)
    {
        currentHour += minutes / 60f;
        Debug.Log("Tid nu: " + GetTimeString());

        if (currentHour >= 26f)
            BarCloses();
    }

    void BarCloses()
    {
        Debug.Log("Baren lukker!");
        SceneManager.LoadScene("TransportScene");
    }

    string GetTimeString()
    {
        int h = (int)currentHour % 24;
        int m = Mathf.RoundToInt((currentHour % 1) * 60);
        return $"{h:00}:{m:00}";
    }
}