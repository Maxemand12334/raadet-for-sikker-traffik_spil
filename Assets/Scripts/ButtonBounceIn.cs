using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBounce : MonoBehaviour
{

    public float endScale = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        this.transform.localScale = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ScaleButton(Vector3.zero, Vector3.one * endScale,false));
    }

    IEnumerator ScaleButton(Vector3 from, Vector3 to, bool hideAfter = false)
    {
        float duration = 0.15f;
        float t = 0f;


        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float scale = Mathf.LerpUnclamped(from.x, to.x, EaseOutBack(t));
            this.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        this.transform.localScale = to;
        if (hideAfter) this.gameObject.SetActive(false);
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }
}
