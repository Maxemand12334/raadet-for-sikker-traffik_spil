using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spillerens stats")]
    public int auraPoints = 0;
    public int money = 300;
    public int drunkLevel = 0;
    public string currentVenue = "";

    void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Denne manglede — tilføj den
    public void ApplyResult(int aura, int money, int drunk)
    {
        auraPoints = auraPoints + aura;
        this.money = Mathf.Max(this.money + money,   0);
        drunkLevel = Mathf.Clamp(drunkLevel + drunk, 0, 100);
    }
}