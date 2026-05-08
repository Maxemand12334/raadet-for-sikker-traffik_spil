using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;

public class DartManager : MonoBehaviour
{
    public GameManager gameManager;
    public DartBoard dartBoard;
    public GameObject dartpilPrefab;
    public Transform idleDartPosition;

    public Button closeButton;
    public TextMeshProUGUI resultText;

    public int initialDartAmount = 3;

    private GameObject activeDart;
    private int score;
    private int dartAmount;

    public List<GameObject> hitDartList;

    public float bounceAmount = 1;
    public float bounceDuration = 1;

    public enum GameState
    {
        Playing,
        results
    }

    public GameState State;

    void OnEnable()
    {
        resultText.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        ClearDarts();
        State = GameState.Playing;
        dartAmount = initialDartAmount;
        CreateDart();
    }

    public void AddScore(int points)
    {
        State = dartAmount <= 0 ? GameState.results : GameState.Playing; 
        StopAllCoroutines();
        StartCoroutine(BoardBounce(bounceDuration));

        score += points;
        if(State == GameState.Playing)
        {
            CreateDart();
        } else
        {
            Results();
        }
    }

    public void ApplyScore()
    {
        gameManager.auraPoints += score;
                score = 0;
    }

    private void CreateDart()
    {
        activeDart = Instantiate(dartpilPrefab, idleDartPosition);
        Dartpil tempDartPil = activeDart.GetComponent<Dartpil>();

        tempDartPil.dartmanager = this;
        tempDartPil.boardRectTransform = dartBoard.GetComponent<RectTransform>();
        tempDartPil.idlePosRectTransform = idleDartPosition.GetComponent<RectTransform>();
        
        activeDart.transform.SetParent(this.transform);
        dartAmount--;
    }

    private void Results()
    {
        resultText.SetText("Aura optjent: " + score);
        resultText.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
    }

    public void ClosePanel(){
        UIManager.Instance.AdvanceTime(30f);
        ApplyScore();
        this.gameObject.SetActive(false);
    }

    private void ClearDarts()
    {
        foreach(GameObject dart in hitDartList)
        {
            Destroy(dart);
        }
        hitDartList.Clear();
    }

    IEnumerator BoardBounce(float duration){
        Vector3 defaultScale = dartBoard.GetComponent<RectTransform>().localScale;
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            float scaleAdd = (Mathf.Cos(i * Mathf.PI * 2 / duration + Mathf.PI) + 1) * bounceAmount;
            dartBoard.transform.localScale = defaultScale + Vector3.one * scaleAdd;
            yield return null;
        }
        dartBoard.GetComponent<RectTransform>().localScale = defaultScale;
    }
}
