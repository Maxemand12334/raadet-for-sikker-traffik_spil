using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BodegaElement : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Element info")]
    public string elementNavn = "Guinness";
    public string beskrivelse = "Split the G";
    public int auraCost  = 10;
    public int moneyCost = 30;
    public int drunkAmount = 10;
    public float tidsMinutter = 30f;

    [Header("Mini-game")]
    public GameObject miniGamePanel;

    [Header("Referencer")]
    public Image elementImage;
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;
    public GameObject soundPlayer;
    public AudioClip hoverClip;

    void Start()
    {
        tooltipObject.SetActive(false);
        tooltipObject.transform.localScale = Vector3.zero;
    }

public void OnPointerEnter(PointerEventData e)
{
    bool harPenge = GameManager.Instance.money >= moneyCost;

    if (!harPenge)
    {
        elementImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);
        tooltipText.text = $"{elementNavn}\ntoo broke";
    }
    else
    {
        elementImage.color = new Color(1f, 0.95f, 0.7f, 1f);

        string moneyStr = moneyCost > 0 ? $"{moneyCost} kr" : "for free";
        string tidStr = tidsMinutter >= 60 
            ? $"{tidsMinutter/60f:0.#} time" 
            : $"{tidsMinutter} min";

        tooltipText.text = $"{elementNavn}\n{moneyStr}\n{tidStr}";

        Instantiate(soundPlayer, Vector3.zero, Quaternion.identity);
        soundPlayer.GetComponent<AudioSource>().clip = hoverClip;
    }

    tooltipObject.SetActive(true);
    StopAllCoroutines();
    StartCoroutine(ScaleTooltip(Vector3.zero, Vector3.one));
}

public void OnPointerExit(PointerEventData e)
{
    bool harPenge = GameManager.Instance.money >= moneyCost;
    elementImage.color = harPenge ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
    StopAllCoroutines();
    StartCoroutine(ScaleTooltip(Vector3.one, Vector3.zero, hideAfter: true));
}

public void OnPointerClick(PointerEventData e)
{
    Debug.Log("Klikkede på " + elementNavn);

    if (GameManager.Instance.money < moneyCost)
    {
        Debug.Log("Ikke råd!");
        return;
    }

    if (miniGamePanel != null)
    {
        Debug.Log("Åbner " + miniGamePanel.name);
        miniGamePanel.SetActive(true);
        return;
    }

    GameManager.Instance.ApplyResult(auraCost, -moneyCost, drunkAmount);
    UIManager.Instance.AdvanceTime(tidsMinutter);
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