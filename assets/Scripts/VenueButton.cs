using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VenueButton : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Venue info")]
    public string venueName = "Bodega";
    public int requiredAura = 0;
    public string aabningsTid = "21:00 - 02:00";

    [Header("Referencer")]
    public Image venueImage;
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    bool isUnlocked;

    void Start()
    {
        UpdateVisual();
        tooltipObject.transform.localScale = Vector3.zero;
    }

    void UpdateVisual()
    {
        isUnlocked = GameManager.Instance.auraPoints >= requiredAura;
        venueImage.color = isUnlocked 
            ? Color.white 
            : new Color(0.3f, 0.3f, 0.3f, 1f);
        tooltipObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData e)
{
    // Skalér op
    StopAllCoroutines();
    StartCoroutine(ScaleImage(Vector3.one, new Vector3(1.1f, 1.1f, 1f)));

    if (isUnlocked)
    {
        venueImage.color = new Color(1f, 1f, 0.7f, 1f);
        tooltipText.text = $"{venueName}\nÅben: {aabningsTid}";
    }
    else
    {
        tooltipText.text = $"Ikke swag nok\n{requiredAura - GameManager.Instance.auraPoints} aura\n{aabningsTid}";
    }

    tooltipObject.SetActive(true);
    StartCoroutine(ScaleTooltip(Vector3.zero, Vector3.one));
}

public void OnPointerExit(PointerEventData e)
{
    // Skalér tilbage
    StopAllCoroutines();
    StartCoroutine(ScaleImage(new Vector3(1.1f, 1.1f, 1f), Vector3.one));

    if (isUnlocked)
        venueImage.color = Color.white;
    else
        venueImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

    StartCoroutine(ScaleTooltip(Vector3.one, Vector3.zero, hideAfter: true));
}

IEnumerator ScaleImage(Vector3 from, Vector3 to)
{
    float duration = 0.12f;
    float t = 0f;
    while (t < 1f)
    {
        t += Time.deltaTime / duration;
        venueImage.transform.localScale = Vector3.Lerp(from, to, t);
        yield return null;
    }
    venueImage.transform.localScale = to;
}

    public void OnPointerClick(PointerEventData e)
    {
        if (!isUnlocked) return;
        GameManager.Instance.currentVenue = venueName;
        SceneManager.LoadScene("BodegaScene");
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