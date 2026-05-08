using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems; // Required for UI events

public class Dartpil : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public DartManager dartmanager;
    private RectTransform rectTransform;
    public RectTransform idlePosRectTransform;
    public RectTransform boardRectTransform;
    private Vector2 hitPosition;
    private Canvas canvas;
    public GameObject hitDart;

    [Header("Dart Animation")]

    public float dartVelocity = 200;
    public float dartShrinkSpeed = 200;
    public float flyAnimationDuration;

    [Header("Dart Sound")]
    public GameObject prefabAudioSource;
    public AudioClip hitAudioClip;
    public AudioClip throwAudioClip;


    public enum DartState
    {
        idle,
        held,
        thrown,
        hit
    }

    public DartState state;

    void Awake()
    {
        // Cache these for better performance
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        state = DartState.idle;
    }
    

    public void OnDrag(PointerEventData eventData)
    {
        if (state == DartState.idle) state = DartState.held;
        if (state == DartState.idle || state == DartState.held){
        // eventData.delta is the change in mouse position since last frame.
        // We divide by scaleFactor so it moves correctly even if the screen is resized.

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        

        Vector2 idlePoint = new Vector2(idlePosRectTransform.anchoredPosition.x, idlePosRectTransform.anchoredPosition.y);
        Vector2 dragPoint = idlePoint - new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
        
        float m = (idlePoint.y-dragPoint.y)/(idlePoint.x-dragPoint.x);

        float xHit = ((boardRectTransform.anchoredPosition.y - idlePoint.y)/m) + idlePoint.x;

        float distanceToIdle = Vector3.Magnitude(rectTransform.anchoredPosition - idlePosRectTransform.anchoredPosition);

        float dartAngle = Vector2.SignedAngle(Vector2.down, rectTransform.anchoredPosition - idlePosRectTransform.anchoredPosition);

        rectTransform.eulerAngles = new Vector3(0, 0, dartAngle);

        hitPosition = new Vector2(xHit, boardRectTransform.anchoredPosition.y - ((rectTransform.anchoredPosition.y - idlePosRectTransform.anchoredPosition.y + 125))*2);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (state == DartState.held){
            throwDart();
        }
    }  

    public void throwDart(){
        state = DartState.thrown;
        StopAllCoroutines();
        StartCoroutine(DartFly());
        PlayAudio(throwAudioClip,1f);
    }

    IEnumerator DartFly()
    {
        for (bool isRunning = true; isRunning;)
        {
            
            Vector2 direction = Vector2.Normalize(hitPosition - rectTransform.anchoredPosition);
            rectTransform.anchoredPosition += direction * Time.deltaTime * dartVelocity;
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one * 0.2f, dartShrinkSpeed * Time.deltaTime);

            float distanceToHit = (hitPosition - rectTransform.anchoredPosition).magnitude;


            if(distanceToHit < 1f)
            {

                
                Debug.Log("last pos of flying dart: " + rectTransform.anchoredPosition);

                GameObject tempHitDart = Instantiate(hitDart,this.transform.position,Quaternion.identity);
                tempHitDart.transform.SetParent(dartmanager.transform);
                Vector2 tempPos = hitPosition;
                dartmanager.hitDartList.Add(tempHitDart);
                // Debug.Log(dartmanager.hitDartList);
                tempHitDart.GetComponent<RectTransform>().anchoredPosition = tempPos;
                tempHitDart.GetComponent<RectTransform>().localScale = Vector3.one * 0.5f;
                
                PlayAudio(hitAudioClip, 1f);

                dartmanager.AddScore(10);

                isRunning = false;
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    void PlayAudio(AudioClip clip, float volume)
    {
        GameObject gameObjectAudioSource = Instantiate(prefabAudioSource,Vector3.zero,Quaternion.identity);
        AudioSource tempAudioSource = gameObjectAudioSource.GetComponent<AudioSource>();
        tempAudioSource.volume = volume;
        tempAudioSource.clip = clip;
        tempAudioSource.Play();
    }
}
