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
        if (isUnlocked)
        {
            venueImage.color = new Color(1f, 1f, 0.7f, 1f);
            tooltipText.text = $"{venueName}\nÅben: {aabningsTid}";
        }
        else
        {
            tooltipText.text = $"{venueName}\nDu mangler {requiredAura - GameManager.Instance.auraPoints} point\nÅben: {aabningsTid}";
        }

        tooltipObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.zero, Vector3.one));
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (isUnlocked)
            venueImage.color = Color.white;
        else
            venueImage.color = new Color(0.3f, 0.3f, 0.3f, 1f);

        StopAllCoroutines();
        StartCoroutine(ScaleTooltip(Vector3.one, Vector3.zero, hideAfter: true));
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