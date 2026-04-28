using UnityEngine;

public class BodegaController : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.StartBarNight();
    }

    // Denne kaldes af klikbare objekter i baren
    public void DoAction(int aura, int money, int drunk, float minutes)
    {
        GameManager.Instance.ApplyResult(aura, money, drunk);
        UIManager.Instance.AdvanceTime(minutes);
    }
}