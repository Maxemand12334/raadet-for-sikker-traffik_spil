using UnityEngine;
using UnityEngine.SceneManagement;

public class TransportController : MonoBehaviour
{
    public void ChooseTaxa()
    {
        GameManager.Instance.ApplyResult(aura: 10, money: -150, drunk: 0);
        GoHome();
    }

    public void ChooseBus()
    {
        GameManager.Instance.ApplyResult(aura: 5, money: -25, drunk: 0);
        GoHome();
    }

    public void ChooseCykel()
    {
        // Hvis man er for fuld og cykler — hospital!
        if (GameManager.Instance.drunkLevel >= 60)
        {
            SceneManager.LoadScene("HospitalScene");
        }
        else
        {
            GameManager.Instance.ApplyResult(aura: -20, money: 0, drunk: 0);
            GoHome();
        }
    }

    void GoHome()
    {
        SceneManager.LoadScene("MapScene");
    }
}