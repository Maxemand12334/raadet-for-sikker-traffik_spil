using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransportOption : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Transport info")]
    public string transportNavn = "Taxa";
    public int auraCost = 0;
    public int moneyCost = 0;
    public bool isCykel = false;

    [Header("Referencer")]
    public Image transportImage;
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    bool harRaad;

    void Start()
    {
        tooltipObject.SetActive(false);
        tooltipObject.transform.localScale = Vector3.zero;
    }

    // Tjek råd hver gang musen går over
    void UpdateRaad()
    {
        harRaad = GameManager.Instance.money >= moneyCost;
        transportImage.color = harRaad 
            ? Color.white 
            : new Color(0.3f, 0.3f, 0.3f, 1f);
    }

    public void OnPointerEnter(PointerEventData e)
    {
        UpdateRaad();

        if (!harRaad)
        {
            transportImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            tooltipText.text = $"{transportNavn}\ntoo broke";
        }
        else
        {
            transportImage.color = new Color(1f, 0.95f, 0.7f, 1f);

            string auraStr  = auraCost >= 0 
                ? $"+{auraCost} aura" 
                : $"{auraCost} aura";
            string moneyStr = moneyCost > 0 
                ? $"-{moneyCost} kr" 
                : "gratis";

            tooltipText.text = $"{transportNavn}\n{auraStr}  ·  {moneyStr}";

        }

        tooltipObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.zero, Vector3.one));
    }

    public void OnPointerExit(PointerEventData e)
    {
        UpdateRaad();
        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.one, Vector3.zero, hideAfter: true));
    }

    public void OnPointerClick(PointerEventData e)
    {
        UpdateRaad();

        if (!harRaad) return;

        if (isCykel && GameManager.Instance.drunkLevel >= 60)
        {
            SceneManager.LoadScene("HospitalScene");
            return;
        }

        GameManager.Instance.ApplyResult(auraCost, -moneyCost, 0);
           GameManager.Instance.ResetForNewDay();
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator ScaleTooltip(Vector3 from, Vector3 to, bool hideAfter = false)
    {
        float duration = 0.15f;
        float t = 0f;
        tooltipObject.transform.localScale = from;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float scale = Mathf.LerpUnclamped(from.x, to.x, EaseOutBack(t));
            tooltipObject.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        tooltipObject.transform.localScale = to;
        if (hideAfter) tooltipObject.SetActive(false);
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}