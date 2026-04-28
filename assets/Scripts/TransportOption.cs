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
    public bool isCykel = false;  // special — tjekker drunkLevel

    [Header("Referencer")]
    public Image transportImage;
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    void Start()
    {
        tooltipObject.SetActive(false);
        tooltipObject.transform.localScale = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        // Lys op
        transportImage.color = new Color(1f, 0.95f, 0.7f, 1f);

        // Byg tooltip tekst
        string auraStr  = auraCost  >= 0 
            ? $"+{auraCost} aura" 
            : $"{auraCost} aura";
        string moneyStr = moneyCost >= 0 
            ? $"{moneyCost} kr" 
            : $"{moneyCost} kr";

        tooltipText.text = $"{transportNavn}\n{auraStr}  ·  {moneyStr}";

    

        tooltipObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.zero, Vector3.one));
    }

    public void OnPointerExit(PointerEventData e)
    {
        transportImage.color = Color.white;
        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.one, Vector3.zero, hideAfter: true));
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (isCykel && GameManager.Instance.drunkLevel >= 60)
        {
            SceneManager.LoadScene("HospitalScene");
            return;
        }

        GameManager.Instance.ApplyResult(auraCost, -moneyCost, 0);
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