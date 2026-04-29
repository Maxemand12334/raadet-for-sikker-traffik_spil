using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        bool inBodega = scene.name == "BodegaScene";
        bool inTransport = scene.name == "TransportScene";

        // Tid kun i BodegaScene
        if (timeText != null)
            timeText.gameObject.SetActive(inBodega);

        // Drunk bar kun i BodegaScene og TransportScene
        if (drunkContainer != null)
            drunkContainer.SetActive(inBodega || inTransport);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        auraText.text  = "Aura: " + GameManager.Instance.auraPoints;
        moneyText.text = "Kr: "   + GameManager.Instance.money;

        // Opdater tid hvis TimeText er aktiv
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

    string GetTimeString()
    {
        int h = (int)currentHour % 24;
        int m = Mathf.RoundToInt((currentHour % 1) * 60);
        return $"{h:00}:{m:00}";
    }
}